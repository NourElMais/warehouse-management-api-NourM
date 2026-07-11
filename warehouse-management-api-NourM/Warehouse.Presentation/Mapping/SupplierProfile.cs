using AutoMapper;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Presentation.Mapping;

public class SupplierProfile:Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, SupplierViewModel>();
    }
}