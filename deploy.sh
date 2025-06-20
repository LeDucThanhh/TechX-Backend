#!/bin/bash

# TechX Backend Deployment Script
RESOURCE_GROUP=${1:-"techx-rg"}
APP_NAME=${2:-"techx-backend-api"}
LOCATION=${3:-"Southeast Asia"}

echo "🚀 Starting TechX Backend Deployment..."

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    echo "❌ Azure CLI not found. Please install Azure CLI first."
    exit 1
fi

# Login to Azure
echo "🔐 Logging into Azure..."
az login

# Create Resource Group
echo "📁 Creating Resource Group..."
az group create --name $RESOURCE_GROUP --location "$LOCATION"

# Create App Service Plan (Free Tier)
echo "📋 Creating App Service Plan..."
az appservice plan create \
    --name "${APP_NAME}-plan" \
    --resource-group $RESOURCE_GROUP \
    --sku F1 \
    --is-linux

# Create Web App
echo "🌐 Creating Web App..."
az webapp create \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan "${APP_NAME}-plan" \
    --runtime "DOTNETCORE:8.0"

# Configure App Settings
echo "⚙️ Configuring App Settings..."
az webapp config appsettings set \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --settings \
        "JwtSettings__SecretKey=TechX_Super_Secret_Key_For_JWT_Authentication_MustBeLongEnough_2024" \
        "JwtSettings__Issuer=TechX" \
        "JwtSettings__Audience=TechXUsers" \
        "JwtSettings__ExpirationHours=24" \
        "GoogleAuth__ClientId=707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com" \
        "GoogleAuth__ProjectId=techx-463418" \
        "GoogleAuth__RedirectUri=com.techx.android://oauth" \
        "ASPNETCORE_ENVIRONMENT=Production"

# Deploy the application
echo "📦 Deploying Application..."
az webapp up \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --location "$LOCATION" \
    --runtime "DOTNETCORE:8.0"

# Display deployment information
echo "✅ Deployment Complete!"
echo "🔗 App URL: https://${APP_NAME}.azurewebsites.net"
echo "🏥 Health Check: https://${APP_NAME}.azurewebsites.net/health"
echo "📖 Swagger: https://${APP_NAME}.azurewebsites.net/swagger"
echo ""
echo "🗃️  Don't forget to setup your PostgreSQL database!"
echo "📱 Update your Android app base URL to: https://${APP_NAME}.azurewebsites.net" 