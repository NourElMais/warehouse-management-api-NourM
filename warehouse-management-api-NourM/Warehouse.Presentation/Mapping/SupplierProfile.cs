using AutoMapper;
using Warehouse.Domain.Suppliers;
using Warehouse.Presentation.ViewModels;

namespace Warehouse.Presentation.Mapping;

public class SupplierProfile:Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, SupplierViewModel>();
    }
}