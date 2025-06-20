# Google OAuth Production Setup

## Current Configuration

- **Client ID:** `707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com`
- **Project ID:** `techx-463418`

## Production Updates Required

### Step 1: Update OAuth Consent Screen

1. Go to [Google Cloud Console](https://console.cloud.google.com/apis/credentials?project=techx-463418)
2. Click "OAuth consent screen"
3. Add production domain if needed:
   - **Authorized domains:** `azurewebsites.net`
4. Update app information if needed

### Step 2: Update OAuth Client Settings

1. Go to "Credentials" tab
2. Click on your OAuth 2.0 Client ID
3. **For Android OAuth:**
   - Keep existing package name: `com.techx.android` (or your actual package)
   - Keep existing SHA-1 fingerprints
   - Add production SHA-1 if different from debug

### Step 3: Get Production SHA-1 (If Different)

For production APK, you may need different SHA-1:

#### From Keystore:

```bash
keytool -list -v -keystore your-release-key.keystore -alias your-key-alias
```

#### From Signed APK:

```bash
keytool -printcert -jarfile your-app.apk
```

#### From Google Play Console:

1. Go to Play Console > Your App > Setup > App Signing
2. Copy "SHA-1 certificate fingerprint"
3. Add this to Google Cloud Console

### Step 4: Backend Configuration (Already Done)

The backend is already configured with:

```json
{
  "GoogleAuth": {
    "ClientId": "707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com",
    "ProjectId": "techx-463418",
    "RedirectUri": "com.techx.android://oauth"
  }
}
```

### Step 5: Android App Updates

Update your Android app's base URL from:

```kotlin
// Before (localhost)
private const val BASE_URL = "http://10.0.2.2:5001/"

// After (production)
private const val BASE_URL = "https://techx-backend-api.azurewebsites.net/"
```

### Step 6: Test Google Sign-In

1. Build production APK
2. Install on real device
3. Test Google Sign-In flow
4. Verify backend receives and validates Google tokens

## Troubleshooting

### Common Issues:

#### "Sign-in temporarily disabled"

- Check OAuth consent screen configuration
- Verify app is not in testing mode for production users

#### "Invalid client" error

- Verify package name matches exactly
- Check SHA-1 fingerprint is correct
- Ensure Client ID is correctly configured

#### "Network error"

- Check if device has internet connection
- Verify backend URL is accessible: https://techx-backend-api.azurewebsites.net/health

#### Backend validation fails

- Check backend logs in Azure Portal
- Verify Google tokeninfo API is accessible from Azure
- Check Google OAuth configuration in Azure app settings

## Testing Checklist

- [ ] Google OAuth consent screen configured
- [ ] Android SHA-1 fingerprint added to Google Console
- [ ] Backend deployed to Azure
- [ ] Database configured and migrated
- [ ] Android app updated with production URL
- [ ] APK built and tested on real device
- [ ] Google Sign-In working end-to-end

## Current Configuration

- **Client ID:** `707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com`
- **Project ID:** `techx-463418`

## Production Updates Required

### Step 1: Update OAuth Consent Screen

1. Go to [Google Cloud Console](https://console.cloud.google.com/apis/credentials?project=techx-463418)
2. Click "OAuth consent screen"
3. Add production domain if needed:
   - **Authorized domains:** `azurewebsites.net`
4. Update app information if needed

### Step 2: Update OAuth Client Settings

1. Go to "Credentials" tab
2. Click on your OAuth 2.0 Client ID
3. **For Android OAuth:**
   - Keep existing package name: `com.techx.android` (or your actual package)
   - Keep existing SHA-1 fingerprints
   - Add production SHA-1 if different from debug

### Step 3: Get Production SHA-1 (If Different)

For production APK, you may need different SHA-1:

#### From Keystore:

```bash
keytool -list -v -keystore your-release-key.keystore -alias your-key-alias
```

#### From Signed APK:

```bash
keytool -printcert -jarfile your-app.apk
```

#### From Google Play Console:

1. Go to Play Console > Your App > Setup > App Signing
2. Copy "SHA-1 certificate fingerprint"
3. Add this to Google Cloud Console

### Step 4: Backend Configuration (Already Done)

The backend is already configured with:

```json
{
  "GoogleAuth": {
    "ClientId": "707259186410-7g8tp4dhu4qndso497a68qbr4ff9b3p3.apps.googleusercontent.com",
    "ProjectId": "techx-463418",
    "RedirectUri": "com.techx.android://oauth"
  }
}
```

### Step 5: Android App Updates

Update your Android app's base URL from:

```kotlin
// Before (localhost)
private const val BASE_URL = "http://10.0.2.2:5001/"

// After (production)
private const val BASE_URL = "https://techx-backend-api.azurewebsites.net/"
```

### Step 6: Test Google Sign-In

1. Build production APK
2. Install on real device
3. Test Google Sign-In flow
4. Verify backend receives and validates Google tokens

## Troubleshooting

### Common Issues:

#### "Sign-in temporarily disabled"

- Check OAuth consent screen configuration
- Verify app is not in testing mode for production users

#### "Invalid client" error

- Verify package name matches exactly
- Check SHA-1 fingerprint is correct
- Ensure Client ID is correctly configured

#### "Network error"

- Check if device has internet connection
- Verify backend URL is accessible: https://techx-backend-api.azurewebsites.net/health

#### Backend validation fails

- Check backend logs in Azure Portal
- Verify Google tokeninfo API is accessible from Azure
- Check Google OAuth configuration in Azure app settings

## Testing Checklist

- [ ] Google OAuth consent screen configured
- [ ] Android SHA-1 fingerprint added to Google Console
- [ ] Backend deployed to Azure
- [ ] Database configured and migrated
- [ ] Android app updated with production URL
- [ ] APK built and tested on real device
- [ ] Google Sign-In working end-to-end
