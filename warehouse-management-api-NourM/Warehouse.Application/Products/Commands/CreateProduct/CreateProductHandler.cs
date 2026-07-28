using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.IntegrationEvents;
using Warehouse.Application.Interfaces;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;
    public CreateProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<CreateProductHandler> logger, IRabbitMqPublisher rabbitMqPublisher)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public async Task<ProductViewModel> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product(
            command.Name,
            command.SKU,
            command.Description,
            command.Price,
            command.QuantityInStock,
            command.SupplierId,
            command.ExpiryDate
        );
        var sameSKU = await _productRepository.GetBySkuAsync(command.SKU, cancellationToken);
        if (sameSKU is not null)
        {
            throw new BusinessRuleException("Cannot create product, as this SKU already exists");
        }
       
        await _productRepository.AddAsync(product, cancellationToken);
       var productCreatedEvent = new ProductCreatedEvent
       {
           ProductId = Guid.Parse(product.Id),
           ProductName = product.Name,
           SKU = product.SKU
       };

       await _rabbitMqPublisher.PublishAsync(
           "product.created",
           productCreatedEvent,
           cancellationToken);
       
       _logger.LogInformation("Product {ProductId} created successfully", product.Id);
       
       return _mapper.Map<ProductViewModel>(product);
    }
}