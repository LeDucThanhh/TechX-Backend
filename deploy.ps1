# TechX Backend Deployment Script
param(
    [string]$ResourceGroup = "techx-rg",
    [string]$AppName = "techx-backend-api",
    [string]$Location = "Southeast Asia"
)

Write-Host "🚀 Starting TechX Backend Deployment..." -ForegroundColor Green

# Check if Azure CLI is installed
if (!(Get-Command "az" -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Azure CLI not found. Please install Azure CLI first." -ForegroundColor Red
    exit 1
}

# Login to Azure
Write-Host "🔐 Logging into Azure..." -ForegroundColor Yellow
az login

# Create Resource Group
Write-Host "📁 Creating Resource Group..." -ForegroundColor Yellow
az group create --name $ResourceGroup --location $Location

# Create App Service Plan (Free Tier)
Write-Host "📋 Creating App Service Plan..." -ForegroundColor Yellow
az appservice plan create `
    --name "${AppName}-plan" `
    --resource-group $ResourceGroup `
    --sku F1 `
    --is-linux

# Create Web App
Write-Host "🌐 Creating Web App..." -ForegroundColor Yellow
az webapp create `
    --name $AppName `
    --resource-group $ResourceGroup `
    --plan "${AppName}-plan" `
    --runtime "DOTNETCORE:8.0"

# Configure App Settings
Write-Host "⚙️ Configuring App Settings..." -ForegroundColor Yellow
az webapp config appsettings set `
    --name $AppName `
    --resource-group $ResourceGroup `
    --settings `
        "JwtSettings__SecretKey=TechX_Super_Secret_Key_For_JWT_Authentication_MustBeLongEnough_2024" `
        "JwtSettings__Issuer=TechX" `
        "JwtSettings__Audience=TechXUsers" `
        "JwtSettings__ExpirationHours=24" `
        "GoogleAuth__ClientId=707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com" `
        "GoogleAuth__ProjectId=techx-463418" `
        "GoogleAuth__RedirectUri=com.techx.android://oauth" `
        "ASPNETCORE_ENVIRONMENT=Production"

# Deploy the application
Write-Host "📦 Deploying Application..." -ForegroundColor Yellow
az webapp up `
    --name $AppName `
    --resource-group $ResourceGroup `
    --location $Location `
    --runtime "DOTNETCORE:8.0"

# Display deployment information
Write-Host "✅ Deployment Complete!" -ForegroundColor Green
Write-Host "🔗 App URL: https://$AppName.azurewebsites.net" -ForegroundColor Cyan
Write-Host "🏥 Health Check: https://$AppName.azurewebsites.net/health" -ForegroundColor Cyan
Write-Host "📖 Swagger: https://$AppName.azurewebsites.net/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "🗃️  Don't forget to setup your PostgreSQL database!" -ForegroundColor Yellow
Write-Host "📱 Update your Android app base URL to: https://$AppName.azurewebsites.net" -ForegroundColor Yellow 