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
            CreateMap<LoyaltyPoints, LoyaltyPointsDTO>();
            CreateMap<Store, StoreDTO>();
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<Settings, SettingsDTO>();
            CreateMap<Item, ItemDTO>();
            CreateMap<Receipt, ReceiptDTO>();
            CreateMap<ReceiptItem, ReceiptItemDTO>();
        }
    }
}