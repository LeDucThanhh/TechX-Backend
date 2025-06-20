# 🚀 Quick Deployment Guide

## Prerequisites

- Azure account (free tier)
- Azure CLI installed
- Android Studio with your APK project

## ⚡ 5-Step Deployment

### Step 1: Install Azure CLI

```bash
# Windows
winget install Microsoft.AzureCLI

# macOS
brew install azure-cli

# Ubuntu/Debian
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

### Step 2: Deploy Backend (Auto Script)

```bash
# Windows PowerShell
.\deploy.ps1

# Linux/Mac
chmod +x deploy.sh
./deploy.sh
```

### Step 3: Setup Database (FREE)

1. Go to [ElephantSQL](https://www.elephantsql.com/)
2. Create free account → "Tiny Turtle" instance
3. Copy connection string
4. Run command:

```bash
az webapp config connection-string set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --connection-string-type PostgreSQL \
  --settings DefaultConnection="YOUR_ELEPHANTSQL_CONNECTION_STRING"
```

### Step 4: Run Database Migration

1. Go to [Azure Portal](https://portal.azure.com)
2. Find your app: `techx-backend-api`
3. Go to "SSH" or "Console"
4. Run: `dotnet ef database update`

### Step 5: Update Android App

Change base URL in your Android project:

```kotlin
// Before
private const val BASE_URL = "http://10.0.2.2:5001/"

// After
private const val BASE_URL = "https://techx-backend-api.azurewebsites.net/"
```

## ✅ Test Your Deployment

### Backend Health Check

Visit: https://techx-backend-api.azurewebsites.net/health

Should return: `Healthy`

### API Documentation

Visit: https://techx-backend-api.azurewebsites.net/swagger

### Test Registration

```bash
curl -X POST https://techx-backend-api.azurewebsites.net/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"TestPass123@","firstName":"Test","lastName":"User","phoneNumber":"0901234567","dateOfBirth":"1990-01-01"}'
```

## 📱 Build & Test APK

1. **Update Android base URL** (Step 5 above)
2. **Build signed APK** in Android Studio
3. **Install on real device** (Pixel 9)
4. **Test authentication flows**

## 🔧 Google OAuth for Real Device

If Google Sign-In doesn't work:

1. Get production SHA-1 fingerprint:

```bash
keytool -list -v -keystore ~/.android/debug.keystore -alias androiddebugkey
```

2. Add SHA-1 to [Google Console](https://console.cloud.google.com/apis/credentials?project=techx-463418)

3. Update OAuth client with production fingerprint

## 🎯 Expected Results

✅ **Backend API**: Working on Azure  
✅ **Database**: PostgreSQL on ElephantSQL  
✅ **Authentication**: Registration/Login working  
✅ **Google OAuth**: Working with real device  
✅ **Android APK**: Can connect from real phone

## 🆘 Troubleshooting

### Backend not responding

- Check Azure Portal > App Service > Logs
- Verify app settings are configured
- Check database connection string

### Google Sign-In fails

- Verify SHA-1 fingerprint in Google Console
- Check package name matches exactly
- Ensure OAuth consent screen is configured

### Database connection fails

- Verify ElephantSQL connection string
- Check firewall/SSL settings
- Test connection from Azure console

## 💰 Costs

**FREE TIER LIMITS:**

- Azure App Service: 60 CPU minutes/day
- ElephantSQL: 20MB storage, 5 connections
- Total monthly cost: **$0** (within free limits)

**If you exceed free limits:**

- Azure App Service: ~$13/month (Basic plan)
- PostgreSQL: Consider Azure Database or paid ElephantSQL

Your backend is now production-ready! 🎉

## Prerequisites

- Azure account (free tier)
- Azure CLI installed
- Android Studio with your APK project

## ⚡ 5-Step Deployment

### Step 1: Install Azure CLI

```bash
# Windows
winget install Microsoft.AzureCLI

# macOS
brew install azure-cli

# Ubuntu/Debian
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

### Step 2: Deploy Backend (Auto Script)

```bash
# Windows PowerShell
.\deploy.ps1

# Linux/Mac
chmod +x deploy.sh
./deploy.sh
```

### Step 3: Setup Database (FREE)

1. Go to [ElephantSQL](https://www.elephantsql.com/)
2. Create free account → "Tiny Turtle" instance
3. Copy connection string
4. Run command:

```bash
az webapp config connection-string set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --connection-string-type PostgreSQL \
  --settings DefaultConnection="YOUR_ELEPHANTSQL_CONNECTION_STRING"
```

### Step 4: Run Database Migration

1. Go to [Azure Portal](https://portal.azure.com)
2. Find your app: `techx-backend-api`
3. Go to "SSH" or "Console"
4. Run: `dotnet ef database update`

### Step 5: Update Android App

Change base URL in your Android project:

```kotlin
// Before
private const val BASE_URL = "http://10.0.2.2:5001/"

// After
private const val BASE_URL = "https://techx-backend-api.azurewebsites.net/"
```

## ✅ Test Your Deployment

### Backend Health Check

Visit: https://techx-backend-api.azurewebsites.net/health

Should return: `Healthy`

### API Documentation

Visit: https://techx-backend-api.azurewebsites.net/swagger

### Test Registration

```bash
curl -X POST https://techx-backend-api.azurewebsites.net/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"TestPass123@","firstName":"Test","lastName":"User","phoneNumber":"0901234567","dateOfBirth":"1990-01-01"}'
```

## 📱 Build & Test APK

1. **Update Android base URL** (Step 5 above)
2. **Build signed APK** in Android Studio
3. **Install on real device** (Pixel 9)
4. **Test authentication flows**

## 🔧 Google OAuth for Real Device

If Google Sign-In doesn't work:

1. Get production SHA-1 fingerprint:

```bash
keytool -list -v -keystore ~/.android/debug.keystore -alias androiddebugkey
```

2. Add SHA-1 to [Google Console](https://console.cloud.google.com/apis/credentials?project=techx-463418)

3. Update OAuth client with production fingerprint

## 🎯 Expected Results

✅ **Backend API**: Working on Azure  
✅ **Database**: PostgreSQL on ElephantSQL  
✅ **Authentication**: Registration/Login working  
✅ **Google OAuth**: Working with real device  
✅ **Android APK**: Can connect from real phone

## 🆘 Troubleshooting

### Backend not responding

- Check Azure Portal > App Service > Logs
- Verify app settings are configured
- Check database connection string

### Google Sign-In fails

- Verify SHA-1 fingerprint in Google Console
- Check package name matches exactly
- Ensure OAuth consent screen is configured

### Database connection fails

- Verify ElephantSQL connection string
- Check firewall/SSL settings
- Test connection from Azure console

## 💰 Costs

**FREE TIER LIMITS:**

- Azure App Service: 60 CPU minutes/day
- ElephantSQL: 20MB storage, 5 connections
- Total monthly cost: **$0** (within free limits)

**If you exceed free limits:**

- Azure App Service: ~$13/month (Basic plan)
- PostgreSQL: Consider Azure Database or paid ElephantSQL

Your backend is now production-ready! 🎉
