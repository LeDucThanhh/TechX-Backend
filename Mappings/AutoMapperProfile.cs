using AutoMapper;
using TechX.API.Models;
using TechX.API.Models.DTOs;

namespace TechX.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Chỉ giữ lại mapping đúng với model và DTO hiện tại
            CreateMap<User, UserDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Budget, BudgetDTO>();
            CreateMap<CashbackTransaction, CashbackTransactionDTO>();
            CreateMap<LoyaltyPoint, LoyaltyPointsDTO>().ReverseMap();
            CreateMap<CreateLoyaltyPointsDTO, LoyaltyPoint>();
            CreateMap<UpdateLoyaltyPointsDTO, LoyaltyPoint>();
            CreateMap<Store, StoreDTO>();
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<Setting, SettingsDTO>().ReverseMap();
            CreateMap<UpdateSettingsDTO, Setting>();
            CreateMap<Item, ItemDTO>();
            CreateMap<Receipt, ReceiptDTO>();
            CreateMap<ReceiptItem, ReceiptItemDTO>();
        }
    }
}