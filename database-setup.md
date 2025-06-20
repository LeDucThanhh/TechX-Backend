# Production Database Setup

## Option 1: ElephantSQL (FREE & RECOMMENDED)

### Step 1: Create Account

1. Go to https://www.elephantsql.com/
2. Click "Get a managed database today"
3. Sign up with GitHub/Google/Email

### Step 2: Create Database Instance

1. Click "Create New Instance"
2. Select "Tiny Turtle" (FREE)
3. Name: `techx-production-db`
4. Select nearest datacenter (Singapore for Vietnam)
5. Click "Create instance"

### Step 3: Get Connection Details

1. Click on your instance
2. Copy the connection string (format: postgres://username:password@host:port/dbname)
3. Example: `postgres://abcdefgh:xyz123...@rain.db.elephantsql.com:5432/abcdefgh`

### Step 4: Configure Azure App Service

```bash
# Set connection string in Azure
az webapp config connection-string set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --connection-string-type PostgreSQL \
  --settings DefaultConnection="YOUR_ELEPHANTSQL_CONNECTION_STRING"
```

### Step 5: Run Migrations

1. Go to Azure Portal > App Service > SSH/Console
2. Or use: https://techx-backend-api.scm.azurewebsites.net/DebugConsole
3. Run: `dotnet ef database update`

## Option 2: Azure Database for PostgreSQL

### Step 1: Create PostgreSQL Server

```bash
az postgres flexible-server create \
  --name techx-db-server \
  --resource-group techx-rg \
  --location "Southeast Asia" \
  --admin-user techxadmin \
  --admin-password "YourSecurePassword123!" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32 \
  --public-access 0.0.0.0
```

### Step 2: Create Database

```bash
az postgres flexible-server db create \
  --resource-group techx-rg \
  --server-name techx-db-server \
  --database-name techx_production
```

### Step 3: Configure Firewall

```bash
# Allow Azure services
az postgres flexible-server firewall-rule create \
  --resource-group techx-rg \
  --name techx-db-server \
  --rule-name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### Step 4: Get Connection String

```bash
# Connection string format:
Host=techx-db-server.postgres.database.azure.com;Database=techx_production;Username=techxadmin;Password=YourSecurePassword123!;SSL Mode=Require;
```

## After Database Setup

### 1. Update Azure App Service

Set the connection string environment variable

### 2. Run Migrations

Use Kudu console or SSH to run:

```bash
dotnet ef database update
```

### 3. Seed Data

The app will automatically seed categories and default data on first run.

### 4. Test Connection

Visit: https://techx-backend-api.azurewebsites.net/health

## Troubleshooting

### Connection Issues

- Check firewall rules
- Verify SSL settings
- Ensure connection string format is correct

### Migration Issues

- Check logs in Azure Portal > App Service > Log Stream
- Verify Entity Framework tools are available
- May need to run migrations manually via SQL

### Performance

- ElephantSQL free tier: 20MB storage, 5 concurrent connections
- Azure free tier: More resources but costs money after trial

## Option 1: ElephantSQL (FREE & RECOMMENDED)

### Step 1: Create Account

1. Go to https://www.elephantsql.com/
2. Click "Get a managed database today"
3. Sign up with GitHub/Google/Email

### Step 2: Create Database Instance

1. Click "Create New Instance"
2. Select "Tiny Turtle" (FREE)
3. Name: `techx-production-db`
4. Select nearest datacenter (Singapore for Vietnam)
5. Click "Create instance"

### Step 3: Get Connection Details

1. Click on your instance
2. Copy the connection string (format: postgres://username:password@host:port/dbname)
3. Example: `postgres://abcdefgh:xyz123...@rain.db.elephantsql.com:5432/abcdefgh`

### Step 4: Configure Azure App Service

```bash
# Set connection string in Azure
az webapp config connection-string set \
  --name techx-backend-api \
  --resource-group techx-rg \
  --connection-string-type PostgreSQL \
  --settings DefaultConnection="YOUR_ELEPHANTSQL_CONNECTION_STRING"
```

### Step 5: Run Migrations

1. Go to Azure Portal > App Service > SSH/Console
2. Or use: https://techx-backend-api.scm.azurewebsites.net/DebugConsole
3. Run: `dotnet ef database update`

## Option 2: Azure Database for PostgreSQL

### Step 1: Create PostgreSQL Server

```bash
az postgres flexible-server create \
  --name techx-db-server \
  --resource-group techx-rg \
  --location "Southeast Asia" \
  --admin-user techxadmin \
  --admin-password "YourSecurePassword123!" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32 \
  --public-access 0.0.0.0
```

### Step 2: Create Database

```bash
az postgres flexible-server db create \
  --resource-group techx-rg \
  --server-name techx-db-server \
  --database-name techx_production
```

### Step 3: Configure Firewall

```bash
# Allow Azure services
az postgres flexible-server firewall-rule create \
  --resource-group techx-rg \
  --name techx-db-server \
  --rule-name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### Step 4: Get Connection String

```bash
# Connection string format:
Host=techx-db-server.postgres.database.azure.com;Database=techx_production;Username=techxadmin;Password=YourSecurePassword123!;SSL Mode=Require;
```

## After Database Setup

### 1. Update Azure App Service

Set the connection string environment variable

### 2. Run Migrations

Use Kudu console or SSH to run:

```bash
dotnet ef database update
```

### 3. Seed Data

The app will automatically seed categories and default data on first run.

### 4. Test Connection

Visit: https://techx-backend-api.azurewebsites.net/health

## Troubleshooting

### Connection Issues

- Check firewall rules
- Verify SSL settings
- Ensure connection string format is correct

### Migration Issues

- Check logs in Azure Portal > App Service > Log Stream
- Verify Entity Framework tools are available
- May need to run migrations manually via SQL

### Performance

- ElephantSQL free tier: 20MB storage, 5 concurrent connections
- Azure free tier: More resources but costs money after trial
