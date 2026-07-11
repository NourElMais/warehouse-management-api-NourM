using AutoMapper;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
namespace Warehouse.Presentation.Mapping;

//This class contains the mapping rules (to map from Product to ProductViewModel)
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductViewModel>() //(Create a mapping from Product to ProductViewModel)
            .ForMember(
                destination => destination.SupplierName,
                options => options.MapFrom(
                    source => source.Supplier != null
                        ? source.Supplier.Name
                        : null));
    } 
}