﻿using AutoMapper;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Resolvers;

namespace WarehouseAppR.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PendingSaleProduct, Sale>()
               .ForMember(dest => dest.DateSaled, c => c.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
               .ForMember(dest => dest.ProductId, c => c.MapFrom(src => src.Stock.Product.ProductId))
               .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Stock.Series))
               .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
               .ForMember(dest => dest.UserId, c => c.MapFrom((src, dest, _, context) =>
                   context.Items.TryGetValue("UserId", out var userId) ? (Guid)userId
                   : throw new AutoMapperMappingException("PendingSaleProduct->Sale: Can't resolve userId")))
                .AfterMap((src, dest) =>
                {
                    decimal amountPaid = decimal.Round(dest.Quantity * (src.Stock.Product.Price * (1.0M 
                        + (decimal)(src.Stock.Product.Category.Vat / 100.0M))), 2);
                    dest.AmountPaid = amountPaid;
                    decimal pricePaidPerUnit = decimal.Round(src.Stock.StockDelivery.PricePaid / src.Stock.StockDelivery.Quantity, 2);
                    dest.Profit = (src.Stock.Product.Price * src.Quantity) - (pricePaidPerUnit * src.Quantity);
                });
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<ManufacturerDTO, Manufacturer>().ReverseMap();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ManufacturerName, c => c.MapFrom(src => src.Manufacturer.Name))
                .ForMember(dest => dest.Name, c => c.MapFrom(src => src.Name))
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.TradeName))
                .ForMember(dest => dest.CategoryName, c => c.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.UnitType, c => c.MapFrom(src => src.UnitType))
                .ForMember(dest => dest.Price, c => c.MapFrom(src => src.Price))
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Ean))
                .ForMember(dest => dest.Description, c => c.MapFrom(src => src.Description));

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ManufacturerId, c => c.MapFrom<ManufacturerIdResolver>())
                .ForMember(dest => dest.CategoryId, c => c.MapFrom<CategoryIdResolver>())
                .ForMember(dest => dest.Name, c => c.MapFrom(src => src.Name))
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.TradeName))
                .ForMember(dest => dest.UnitType, c => c.MapFrom(src => src.UnitType))
                .ForMember(dest => dest.Price, c => c.MapFrom(src => decimal.Round(src.Price, 2)))
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Ean))
                .ForMember(dest => dest.Description, c => c.MapFrom(src => src.Description));

            CreateMap<AddNewStockDeliveryDTO, StockDelivery>()
                .ForMember(dest => dest.ProductId, c => c.MapFrom<ProductIdResolver>())
                .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
                .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.PricePaid, c => c.MapFrom(src => decimal.Round(src.PricePaid, 2)))
                .ForMember(dest => dest.DateDelivered, c => c.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.UserId, c => c.MapFrom((src, dest, _, context) =>
                        context.Items.TryGetValue("UserId", out var id) ? (Guid)id : throw new AutoMapperMappingException("AddNewStockDeliveryDto->StockDelivery: Can't resolve userid value ")));
            CreateMap<AddNewStockDeliveryDTO, Stock>()
                .ForMember(dest => dest.ProductId, c => c.MapFrom<ProductIdResolver>())
                .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
                .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ExpirationDate, c => c.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.StorageLocationCode, c => c.MapFrom(src => src.StorageLocationCode));

            CreateMap<StockDelivery, StockDeliveryDTO>()
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Product.Ean))
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.Product.TradeName))
                .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
                .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.DateDelivered, c => c.MapFrom(src => src.DateDelivered))
                .ForMember(dest => dest.UserId, c => c.MapFrom(src => src.UserId));

            CreateMap<Stock, StockDTO>()
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.Product.TradeName))
                .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
                .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ExpirationDate, c => c.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.StorageLocationCode, c => c.MapFrom(src => src.StorageLocationCode))
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Product.Ean))
                .ForMember(dest => dest.UnitType, c => c.MapFrom(src => src.Product.UnitType))
                .ForMember(dest => dest.PricePaid, c => c.MapFrom(src => src.StockDelivery.PricePaid)); //New

            //CreateMap<Stock, SaleDTO>()
            //    .ForMember(dest => dest.ProductId, c => c.MapFrom(src => src.ProductId))
            //    .ForMember(dest => dest.Quantity, c => c.MapFrom((src, dest, _, context) => 
            //            context.Items.TryGetValue("quantity", out var quantity) ? (decimal)quantity : throw new AutoMapperMappingException("Stock->SaleDTO: Can't resolve quantity value ")))
            //    .ForMember(dest => dest.DateSaled, c => c.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
            //    .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
            //    .ForMember(dest => dest.UserId, c => c.MapFrom(src => 1))
            //    .AfterMap((src, dest) =>
            //    {
            //        dest.Price = decimal.Round(dest.Quantity * (src.Product.Price * (1.0M + (decimal)(src.Product.Category.Vat / 100.0M))),2);
            //    }); //Unused I guess, will see
            CreateMap<Sale, SaleDTO>()
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.Product.TradeName))
                .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.AmountPaid, c => c.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.Profit, c => c.MapFrom(src => src.Profit))
                .ForMember(dest => dest.DateSaled, c => c.MapFrom(src => src.DateSaled))
                .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Product.Ean))
                .ForMember(dest => dest.UserFullName, c => c.MapFrom(src => src.User.FirstName + " " + src.User.LastName ));
            //CreateMap<SaleDTO, Sale>()
            //    .ForMember(dest => dest.UserId, c => c.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.ProductId, c => c.MapFrom(src => src.ProductId))
            //    .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
            //    .ForMember(dest => dest.AmountPaid, c => c.MapFrom(src => src.AmountPaid))
            //    .ForMember(dest => dest.DateSaled, c => c.MapFrom(src => src.DateSaled))
            //    .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series)); ////Unused I guess, will see


            //CreateMap<Stock, SaleListItemPreviewDTO>()
            //    .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
            //    .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Product.Ean))
            //    .ForMember(dest => dest.ProductName, c => c.MapFrom(src => src.Product.Name))
            //    .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.Product.TradeName))
            //    .ForMember(dest => dest.Quantity, c => c.MapFrom((src, dest, _, context) =>
            //            context.Items.TryGetValue("quantity", out var quantity) ? (decimal)quantity : throw new AutoMapperMappingException("Stock->SaleListItemPreview: Can't resolve quantity value ")))
            //    .AfterMap((src, dest) =>
            //    {
            //        dest.Price = decimal.Round(dest.Quantity * (src.Product.Price * (1.0M + (decimal)(src.Product.Category.Vat / 100.0M))), 2);
            //    });

            //CreateMap<SaleListItemPreviewDTO, PendingSaleProduct>()
            //    .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
            //    .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Series))
            //    .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Ean))
            //    .ForMember(dest => dest.ProductSaleId, c => c.MapFrom((src, dest, _, context) =>
            //            context.Items.TryGetValue("ProductSaleId", out var id) ? (Guid)id : throw new AutoMapperMappingException("SaleListItemPreview->SaleList: Can't resolve userid value ")));

            CreateMap<PendingSaleProduct, PendingSaleProductPreviewDTO>()
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Stock.Product.Ean))
                .ForMember(dest => dest.Name, c => c.MapFrom(src => src.Stock.Product.Name))
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.Stock.Product.TradeName))
                .ForMember(dest => dest.Series, c => c.MapFrom(src => src.Stock.Series))
                .ForMember(dest => dest.Quantity, c => c.MapFrom(src => src.Quantity))
                .AfterMap((src, dest) =>
                {
                    decimal amountToBePaid = decimal.Round(dest.Quantity * (src.Stock.Product.Price * (1.0M + (decimal)(src.Stock.Product.Category.Vat / 100.0M))), 2);
                    dest.AmountToBePaid = amountToBePaid;
                    decimal pricePaidPerUnit = decimal.Round(src.Stock.StockDelivery.PricePaid / src.Stock.StockDelivery.Quantity, 2);
                    dest.Profit = (src.Stock.Product.Price * src.Quantity) - (pricePaidPerUnit * src.Quantity);
                });



            CreateMap<User, ShowUserDTO>().
                ForMember(dest => dest.RoleName, c => c.MapFrom(src => src.Role.RoleName));



        }
    }
}
