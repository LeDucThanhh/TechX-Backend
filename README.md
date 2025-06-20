# 🚀 TechX Backend API

Backend API hoàn chỉnh cho ứng dụng TechX - Hệ thống quản lý tài chính và cashback cho sinh viên.

## 📋 **Yêu cầu hệ thống**

- Visual Studio 2022 (Community/Professional/Enterprise)
- .NET 8.0 SDK
- PostgreSQL 12+
- Git

## 🛠️ **Cài đặt và chạy**

### 1. Clone repository

```bash
git clone <repository-url>
cd TechX-Backend
```

### 2. Cài đặt PostgreSQL

Xem hướng dẫn chi tiết trong file `POSTGRESQL_SETUP.md`

### 3. Restore NuGet packages

```bash
dotnet restore
```

### 4. Cấu hình database

- Đảm bảo PostgreSQL đã được cài đặt và chạy
- Tạo database: `CREATE DATABASE techx;`
- Cập nhật connection string trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=techx;Username=postgres;Password=your_password"
  }
}
```

### 5. Tạo database và migration

```bash
# Cài đặt Entity Framework tools (nếu chưa có)
dotnet tool install --global dotnet-ef

# Tạo migration đầu tiên
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### 6. Chạy ứng dụng

```bash
dotnet run
```

API sẽ chạy tại: `https://localhost:7001` và `http://localhost:5001`

## 📚 **API Endpoints**

### Authentication

- `POST /api/auth/register` - Đăng ký tài khoản
- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/google-callback` - Google Sign-In
- `GET /api/auth/profile` - Lấy thông tin user (cần JWT)

### Transactions

- `GET /api/transaction` - Lấy danh sách giao dịch (cần JWT)
- `GET /api/transaction/{id}` - Lấy chi tiết giao dịch (cần JWT)
- `POST /api/transaction` - Tạo giao dịch mới (cần JWT)
- `PUT /api/transaction/{id}` - Cập nhật giao dịch (cần JWT)
- `DELETE /api/transaction/{id}` - Xóa giao dịch (cần JWT)
- `GET /api/transaction/summary` - Tóm tắt giao dịch (cần JWT)

### Stores

- `GET /api/store` - Lấy danh sách cửa hàng
- `GET /api/store/{id}` - Lấy chi tiết cửa hàng
- `POST /api/store/nearby` - Tìm cửa hàng gần đây
- `POST /api/store` - Tạo cửa hàng mới
- `PUT /api/store/{id}` - Cập nhật cửa hàng
- `DELETE /api/store/{id}` - Xóa cửa hàng
- `GET /api/store/partners` - Lấy danh sách cửa hàng đối tác

### Budget (Coming Soon)

- `GET /api/budget` - Lấy danh sách ngân sách (cần JWT)

### Cashback (Coming Soon)

- `GET /api/cashback/history` - Lịch sử cashback (cần JWT)

### Loyalty (Coming Soon)

- `GET /api/loyalty/points` - Lấy điểm tích lũy (cần JWT)
- `GET /api/loyalty/rewards` - Danh sách phần thưởng (cần JWT)

## 🔐 **Authentication**

API sử dụng JWT (JSON Web Token) cho authentication:

1. **Đăng ký**: Gửi POST request đến `/api/auth/register`
2. **Đăng nhập**: Gửi POST request đến `/api/auth/login`
3. **Sử dụng token**: Thêm header `Authorization: Bearer <token>` vào các request cần authentication

### Ví dụ request đăng ký:

```json
POST /api/auth/register
{
  "email": "student@example.com",
  "password": "password123",
  "fullName": "Nguyễn Văn A",
  "phoneNumber": "0123456789",
  "dateOfBirth": "2000-01-01"
}
```

### Ví dụ request đăng nhập:

```json
POST /api/auth/login
{
  "email": "student@example.com",
  "password": "password123"
}
```

## 🗄️ **Database Schema**

### Users

- Thông tin người dùng, authentication, loyalty points, cashback

### Transactions

- Giao dịch thu chi của người dùng

### Stores

- Thông tin cửa hàng, địa chỉ, cashback rate

### Budgets

- Ngân sách theo danh mục và thời gian

### CashbackTransactions

- Giao dịch cashback từ cửa hàng

### LoyaltyPoints

- Lịch sử tích điểm và đổi thưởng

## 🚀 **Deploy lên Chlpay**

### 1. Build cho production

```bash
dotnet build --configuration Release
dotnet publish --configuration Release --output ./publish
```

### 2. Cấu hình production

- Cập nhật `appsettings.Production.json` với connection string thực tế
- Thay đổi JWT secret key
- Cấu hình CORS cho domain thực tế

### 3. Upload lên Chlpay

- Tạo tài khoản Chlpay
- Tạo ứng dụng mới
- Upload thư mục `publish`
- Cấu hình domain và SSL

## 🔧 **Development**

### Cấu trúc project

```
TechX-Backend/
├── Controllers/          # API Controllers
├── Models/              # Entity Models
│   └── DTOs/           # Data Transfer Objects
├── Services/            # Business Logic
│   ├── Interfaces/     # Service Interfaces
│   └── Implementations/ # Service Implementations
├── Data/               # Database Context
├── Helpers/            # Utility Classes
├── Mappings/           # AutoMapper Profiles
└── Program.cs          # Application Entry Point
```

### Thêm tính năng mới

1. Tạo Model trong `Models/`
2. Tạo DTO trong `Models/DTOs/`
3. Tạo Service Interface trong `Services/Interfaces/`
4. Implement Service trong `Services/Implementations/`
5. Tạo Controller trong `Controllers/`
6. Cập nhật AutoMapper Profile
7. Test với Swagger UI

## 📝 **Logging**

Ứng dụng sử dụng Serilog để logging:

- Console logging trong development
- File logging với rotation hàng ngày
- Logs được lưu trong thư mục `logs/`

## 🔒 **Security**

- JWT Authentication với expiration time
- Password hashing với BCrypt
- CORS configuration
- Input validation với Data Annotations
- SQL injection protection với Entity Framework

## 📊 **Monitoring**

- Swagger UI cho API documentation
- Health checks (có thể thêm)
- Structured logging với Serilog

## 🤝 **Contributing**

1. Fork repository
2. Tạo feature branch
3. Commit changes
4. Push to branch
5. Tạo Pull Request

## 📄 **License**

MIT License - xem file LICENSE để biết thêm chi tiết.

## 📞 **Support**

Nếu có vấn đề hoặc câu hỏi, vui lòng tạo issue trên GitHub repository.
