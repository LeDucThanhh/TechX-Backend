# 🚀 TechX Backend API

**Complete Backend API for TechX - Financial Management & Cashback System for Students**

Built with .NET 8, PostgreSQL, and Supabase. **Production Ready** for Android app integration.

## 📱 **Android App Ready - Complete Backend System**

- ✅ **15 Controllers** - Full API endpoints for mobile app
- ✅ **19 Database Tables** - Complete data model
- ✅ **33+ DTOs** - Optimized for mobile data exchange
- ✅ **JWT + Google OAuth** - Secure authentication system
- ✅ **Supabase Integration** - Reliable cloud database
- ✅ **CORS Configured** - Mobile app compatibility

---

## 🛠️ **Technology Stack**

- **.NET 8.0** - Latest C# framework
- **PostgreSQL** - Reliable database via Supabase
- **Entity Framework Core** - ORM with Code First
- **JWT Authentication** - Secure token-based auth
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation

---

## ⚡ **Quick Start**

### 1. **Clone & Setup**

```bash
git clone <repository-url>
cd TechX-Backend
dotnet restore
```

### 2. **Supabase Configuration**

Set environment variable:

```bash
SUPABASE_CONNECTION_STRING="Host=db.rvkrhsfkcfawmobywexf.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YEDrCrRUuOkT6LQE;SSL Mode=Require;Trust Server Certificate=true"
```

Or PostgreSQL URL format (will be auto-converted):

```bash
SUPABASE_CONNECTION_STRING="postgresql://postgres:YEDrCrRUuOkT6LQE@db.rvkrhsfkcfawmobywexf.supabase.co:5432/postgres"
```

### 3. **Run Application**

```bash
dotnet run
```

- **API**: `https://localhost:7001`
- **Swagger**: `https://localhost:7001/swagger`

---

## 📚 **Complete API Endpoints**

### 🔐 **Authentication** (`/api/auth`)

- `POST /register` - User registration
- `POST /login` - Email/password login
- `POST /google-login` - Google OAuth login
- `POST /refresh-token` - Refresh JWT token
- `GET /profile` - Get user profile

### 👤 **User Management** (`/api/user`)

- `GET /{id}` - Get user details
- `PUT /{id}` - Update user profile
- `GET /{id}/summary` - User financial summary

### 💰 **Transactions** (`/api/transaction`)

- `GET /user/{userId}` - User transactions
- `POST /` - Create transaction
- `PUT /{id}` - Update transaction
- `DELETE /{id}` - Delete transaction
- `GET /summary/{userId}` - Financial summary

### 📊 **Budget Management** (`/api/budget`)

- `GET /user/{userId}` - User budgets
- `POST /` - Create budget
- `PUT /{id}` - Update budget
- `DELETE /{id}` - Delete budget

### 🏪 **Stores & Items** (`/api/store`)

- `GET /` - List all stores
- `GET /{id}` - Store details
- `GET /nearby` - Nearby stores
- `GET /{id}/items` - Store items

### 🎁 **Voucher System** (`/api/voucher`)

- `GET /` - Available vouchers
- `GET /user/{userId}` - User vouchers
- `POST /collect/{voucherId}/user/{userId}` - Collect voucher

### ⭐ **Loyalty Points** (`/api/loyalty`)

- `GET /user/{userId}` - User loyalty points
- `POST /` - Add loyalty points
- `GET /summary/{userId}` - Points summary

### 💸 **Cashback** (`/api/cashback`)

- `GET /user/{userId}` - User cashback history
- `POST /` - Process cashback
- `GET /summary/{userId}` - Cashback summary

### 🧾 **Receipt OCR** (`/api/receipt`)

- `POST /` - Upload receipt for OCR
- `GET /user/{userId}` - User receipts
- `PUT /{id}/process` - Process OCR results

### ⭐ **Reviews** (`/api/review`)

- `GET /store/{storeId}` - Store reviews
- `POST /` - Create review
- `GET /user/{userId}` - User reviews

### 📧 **Notifications** (`/api/notification`)

- `GET /user/{userId}` - User notifications
- `PUT /{id}/read` - Mark as read
- `GET /user/{userId}/unread` - Unread notifications

### ⚙️ **Settings** (`/api/settings`)

- `GET /user/{userId}` - User app settings
- `PUT /user/{userId}` - Update settings
- `PUT /user/{userId}/theme` - Update theme

### 🎤 **Voice Input** (`/api/speechtransaction`)

- `POST /` - Process voice transaction
- `GET /user/{userId}` - Voice transaction history

### 🔧 **System** (`/api/databasetest`)

- `GET /health` - System health check
- `GET /tables` - Database table counts

---

## 🔐 **Authentication System**

### **JWT Configuration**

```json
{
  "JwtSettings": {
    "SecretKey": "TechX_Super_Secret_Key_For_JWT_Authentication_Production_2024",
    "ExpirationMinutes": 1440,
    "RefreshTokenExpirationDays": 7
  }
}
```

### **Google OAuth Setup**

```json
{
  "GoogleAuth": {
    "ClientId": "707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com",
    "RedirectUri": "com.techx.android://oauth"
  }
}
```

### **Usage Example**

```bash
# Register
curl -X POST "https://api.techx.com/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123","firstName":"John","lastName":"Doe"}'

# Login
curl -X POST "https://api.techx.com/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123"}'

# Use Token
curl -X GET "https://api.techx.com/api/transaction/user/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 🗄️ **Database Schema (19 Tables)**

### **Core Tables**

- `users` - User accounts & profiles
- `categories` - Transaction categories
- `stores` - Partner stores
- `transactions` - Financial transactions
- `budgets` - Budget management

### **Feature Tables**

- `items` - Store products
- `receipts` & `receipt_items` - OCR receipts
- `loyalty_points` - Loyalty system
- `cashback_transactions` - Cashback tracking
- `vouchers`, `user_vouchers`, `voucher_usage` - Voucher system

### **System Tables**

- `reviews` - Store reviews
- `notifications` - Push notifications
- `settings` - User preferences
- `speech_transactions` - Voice input
- `refresh_tokens` - JWT refresh

---

## 🚀 **Production Deployment**

### **Railway + Supabase Setup**

1. **Create Supabase Project**

   - Go to [supabase.com](https://supabase.com)
   - Create new project
   - Run `create_database.sql` in SQL Editor

2. **Railway Deployment**

   ```bash
   # Install Railway CLI
   npm install -g @railway/cli

   # Login and deploy
   railway login
   railway init
   railway up
   ```

3. **Environment Variables**
   ```bash
   SUPABASE_CONNECTION_STRING=postgresql://postgres:password@db.xxx.supabase.co:5432/postgres
   ```

### **Docker Support**

```bash
# Build image
docker build -t techx-backend .

# Run container
docker run -p 8080:8080 -e SUPABASE_CONNECTION_STRING="your_connection_string" techx-backend
```

---

## 📱 **Mobile App Integration**

### **Android Configuration**

- ✅ CORS enabled for all origins
- ✅ JWT expiration: 24 hours (mobile-friendly)
- ✅ Google OAuth with Android deep links
- ✅ Error handling with consistent JSON responses
- ✅ Rate limiting: 100 requests/minute per user

### **Base URL**

```
Production: https://your-railway-app.railway.app
Development: https://localhost:7001
```

---

## 🔧 **Development**

### **Project Structure**

```
TechX-Backend/
├── Controllers/         # 15 API Controllers
├── Models/             # 19 Entity Models
│   └── DTOs/          # 33+ Data Transfer Objects
├── Services/          # Business Logic Layer
├── Data/              # Entity Framework Context
├── Helpers/           # JWT, Password, Validation
├── Middleware/        # Exception, JWT Middleware
└── Program.cs         # Application Configuration
```

### **Adding New Features**

1. Create Entity Model in `Models/`
2. Add DTO in `Models/DTOs/`
3. Create Service Interface & Implementation
4. Create Controller with endpoints
5. Update `ApplicationDbContext`
6. Test with Swagger UI

---

## 📊 **Features Overview**

| **Feature**        | **Status**  | **Endpoints** |
| ------------------ | ----------- | ------------- |
| 🔐 Authentication  | ✅ Complete | 5 endpoints   |
| 👤 User Management | ✅ Complete | 3 endpoints   |
| 💰 Transactions    | ✅ Complete | 8 endpoints   |
| 📊 Budgets         | ✅ Complete | 6 endpoints   |
| 🏪 Stores          | ✅ Complete | 7 endpoints   |
| 🎁 Vouchers        | ✅ Complete | 5 endpoints   |
| ⭐ Loyalty         | ✅ Complete | 4 endpoints   |
| 💸 Cashback        | ✅ Complete | 4 endpoints   |
| 🧾 Receipt OCR     | ✅ Complete | 6 endpoints   |
| ⭐ Reviews         | ✅ Complete | 4 endpoints   |
| 📧 Notifications   | ✅ Complete | 7 endpoints   |
| ⚙️ Settings        | ✅ Complete | 6 endpoints   |
| 🎤 Voice Input     | ✅ Complete | 4 endpoints   |

**Total: 70+ API Endpoints Ready for Production**

---

## 📞 **Support & Documentation**

- **Swagger UI**: `/swagger` endpoint for interactive API docs
- **Health Check**: `/health` endpoint for monitoring
- **Structured Logging**: All requests/responses logged
- **Error Handling**: Consistent JSON error responses

---

## 📄 **License**

MIT License - See LICENSE file for details.

**🎉 Backend hoàn thành 100% - Sẵn sàng cho Android app! 🚀**
