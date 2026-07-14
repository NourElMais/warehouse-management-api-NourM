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
        //Note: I did not use Task.When because the two EF Core queries share the same DbContext, so they cannot run together.
        var products = await _productRepository.GetAllAsync(cancellationToken);

        var suppliers = await _supplierRepository.GetAllAsync(cancellationToken);
        
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