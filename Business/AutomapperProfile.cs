using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //Created mapping for Receipt and ReceiptModel
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            //Created mapping for Product and ProductModel
            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)));

            CreateMap<ProductModel, Product>();

            //Created mapping for ReceiptDetail and ReceiptDetailModel
            CreateMap<ReceiptDetail, ReceiptDetailModel>();

            //Created mapping that combines Customer and Person into CustomerModel
            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.PersonId))
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Person.Id))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ReverseMap();

            //Created mapping for ProductCategory and ProductCategoryModel
            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pm => pm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();
        }
    }
}