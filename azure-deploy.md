# Azure Deployment Guide - TechX Backend

## Prerequisites

- Azure account (free tier available)
- Azure CLI installed
- .NET SDK 8.0
- Git

## Step 1: Install Azure CLI

```bash
# Windows
winget install Microsoft.AzureCLI

# Login to Azure
az login
```

## Step 2: Create Resource Group

```bash
az group create --name techx-rg --location "Southeast Asia"
```

## Step 3: Create App Service Plan (Free Tier)

```bash
az appservice plan create \
  --name techx-plan \
  --resource-group techx-rg \
  --sku F1 \
  --is-linux
```

## Step 4: Create Web App

```bash
az webapp create \
  --name techx-backend-api \
  --resource-group techx-rg \
  --plan techx-plan \
  --runtime "DOTNETCORE:8.0"
```

## Step 5: Setup PostgreSQL Database

### Option A: ElephantSQL (Free)

1. Go to https://www.elephantsql.com/
2. Create free account
3. Create new instance (Tiny Turtle - Free)
4. Copy connection string

### Option B: Azure Database for PostgreSQL

```bash
az postgres flexible-server create \
  --name techx-db-server \
  --resource-group techx-rg \
  --location "Southeast Asia" \
  --admin-user techxadmin \
  --admin-password "YourSecurePassword123!" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32
```

## Step 6: Configure App Settings

```bash
# Set connection string
az webapp config connection-string set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --connection-string-type PostgreSQL \
  --settings DefaultConnection="Host=YOUR_DB_HOST;Port=5432;Database=YOUR_DB_NAME;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;SSL Mode=Require;"

# Set app settings
az webapp config appsettings set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --settings \
    "JwtSettings__SecretKey=TechX_Super_Secret_Key_For_JWT_Authentication_MustBeLongEnough_2024" \
    "JwtSettings__Issuer=TechX" \
    "JwtSettings__Audience=TechXUsers" \
    "JwtSettings__ExpirationHours=24" \
    "GoogleAuth__ClientId=707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com" \
    "GoogleAuth__ProjectId=techx-463418" \
    "GoogleAuth__RedirectUri=com.techx.android://oauth"
```

## Step 7: Deploy Code

```bash
# From project root directory
az webapp up \
  --name techx-backend-api \
  --resource-group techx-rg \
  --plan techx-plan \
  --location "Southeast Asia" \
  --runtime "DOTNETCORE:8.0"
```

## Step 8: Run Database Migrations

```bash
# SSH into Azure App Service
az webapp ssh --name techx-backend-api --resource-group techx-rg

# Or use Kudu console: https://techx-backend-api.scm.azurewebsites.net/DebugConsole
```

## Your App URL

```
https://techx-backend-api.azurewebsites.net
```

## Update Android App

Change base URL in Android app from:

```
http://10.0.2.2:5001
```

To:

```
https://techx-backend-api.azurewebsites.net
```

## Test Endpoints

- Health: https://techx-backend-api.azurewebsites.net/health
- Swagger: https://techx-backend-api.azurewebsites.net/swagger
- API: https://techx-backend-api.azurewebsites.net/api/

## Prerequisites

- Azure account (free tier available)
- Azure CLI installed
- .NET SDK 8.0
- Git

## Step 1: Install Azure CLI

```bash
# Windows
winget install Microsoft.AzureCLI

# Login to Azure
az login
```

## Step 2: Create Resource Group

```bash
az group create --name techx-rg --location "Southeast Asia"
```

## Step 3: Create App Service Plan (Free Tier)

```bash
az appservice plan create \
  --name techx-plan \
  --resource-group techx-rg \
  --sku F1 \
  --is-linux
```

## Step 4: Create Web App

```bash
az webapp create \
  --name techx-backend-api \
  --resource-group techx-rg \
  --plan techx-plan \
  --runtime "DOTNETCORE:8.0"
```

## Step 5: Setup PostgreSQL Database

### Option A: ElephantSQL (Free)

1. Go to https://www.elephantsql.com/
2. Create free account
3. Create new instance (Tiny Turtle - Free)
4. Copy connection string

### Option B: Azure Database for PostgreSQL

```bash
az postgres flexible-server create \
  --name techx-db-server \
  --resource-group techx-rg \
  --location "Southeast Asia" \
  --admin-user techxadmin \
  --admin-password "YourSecurePassword123!" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32
```

## Step 6: Configure App Settings

```bash
# Set connection string
az webapp config connection-string set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --connection-string-type PostgreSQL \
  --settings DefaultConnection="Host=YOUR_DB_HOST;Port=5432;Database=YOUR_DB_NAME;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;SSL Mode=Require;"

# Set app settings
az webapp config appsettings set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --settings \
    "JwtSettings__SecretKey=TechX_Super_Secret_Key_For_JWT_Authentication_MustBeLongEnough_2024" \
    "JwtSettings__Issuer=TechX" \
    "JwtSettings__Audience=TechXUsers" \
    "JwtSettings__ExpirationHours=24" \
    "GoogleAuth__ClientId=707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com" \
    "GoogleAuth__ProjectId=techx-463418" \
    "GoogleAuth__RedirectUri=com.techx.android://oauth"
```

## Step 7: Deploy Code

```bash
# From project root directory
az webapp up \
  --name techx-backend-api \
  --resource-group techx-rg \
  --plan techx-plan \
  --location "Southeast Asia" \
  --runtime "DOTNETCORE:8.0"
```

## Step 8: Run Database Migrations

```bash
# SSH into Azure App Service
az webapp ssh --name techx-backend-api --resource-group techx-rg

# Or use Kudu console: https://techx-backend-api.scm.azurewebsites.net/DebugConsole
```

## Your App URL

```
https://techx-backend-api.azurewebsites.net
```

## Update Android App

Change base URL in Android app from:

```
http://10.0.2.2:5001
```

To:

```
https://techx-backend-api.azurewebsites.net
```

## Test Endpoints

- Health: https://techx-backend-api.azurewebsites.net/health
- Swagger: https://techx-backend-api.azurewebsites.net/swagger
- API: https://techx-backend-api.azurewebsites.net/api/
