using MediatR;
using Warehouse.Application.Products.GetProductsStatistics;
using Warehouse.Application.Products.Queries;
using Warehouse.Application.Suppliers.Queries;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Inventory.Queries.GetInventoryDashboard;

public class GetInventoryDashboardHandler : IRequestHandler<GetInventoryDashboardQuery, GetInventoryDashboardResponse>
{
    
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;

    public GetInventoryDashboardHandler(IProductRepository productRepository, ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<GetInventoryDashboardResponse> Handle(GetInventoryDashboardQuery request, CancellationToken cancellationToken)
    {
        var productsTask = _productRepository.GetAllAsync(cancellationToken);

        var suppliersTask = _supplierRepository.GetAllAsync(cancellationToken);

        await Task.WhenAll(productsTask, suppliersTask);

        var products = await productsTask;
        var suppliers = await suppliersTask;
        int totalProducts = products.Count;
        int activeProducts = 0;
        int archivedProducts = 0;
        int lowStockProducts = 0;

        foreach (var product in products)
        {
            if (product.IsArchived)
            {
                archivedProducts++;
            }
            else
            {
                activeProducts++;

                if (product.QuantityInStock <= 5)
                    lowStockProducts++;
            }
        }
        
        int totalSuppliers = suppliers.Count;
        int activeSuppliers = 0;
        int inactiveSuppliers = 0;

        foreach (var supplier in suppliers)
        {
            if (supplier.IsActive)
                activeSuppliers++;
            else
                inactiveSuppliers++;
        }
        
        return new GetInventoryDashboardResponse
        {
            ProductStatistics = new GetProductsStatisticsResponse
            {
                TotalProducts = totalProducts,
                ActiveProducts = activeProducts,
                ArchivedProducts = archivedProducts,
                LowStockProducts = lowStockProducts
            },

            SupplierStatistics = new GetSupplierStatisticsResponse
            {
                TotalSuppliers = totalSuppliers,
                ActiveSuppliers = activeSuppliers,
                InactiveSuppliers = inactiveSuppliers
            }
        };
        
    }
}