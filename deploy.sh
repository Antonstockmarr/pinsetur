#!/bin/bash

set -e

# ============================
# CONFIG (EDIT THESE)
# ============================
RESOURCE_GROUP="stockmarr-rg"
APP_NAME="stockmarrdk"

LOG_DIR="logs"
mkdir -p "$LOG_DIR"

LOG_FILE="$LOG_DIR/deploy_$(date +%Y%m%d_%H%M%S).log"

# Log everything (stdout + stderr)
exec > >(tee -a "$LOG_FILE") 2>&1

echo "======================================"
echo "🚀 DEPLOY START: $(date)"
echo "📄 Log file: $LOG_FILE"
echo "======================================"

# ============================
# 1. BUILD VUE FRONTEND
# ============================
echo "📦 Building Vue app..."

cd vueapp

npm install
npm run build

cd ..

echo "✅ Vue build complete"

# Validate Vue output
if [ ! -d "vueapp/dist" ]; then
  echo "❌ ERROR: Vue dist folder missing"
  exit 1
fi

# ============================
# 2. PUBLISH .NET BACKEND
# ============================
echo "⚙️ Publishing .NET app..."

rm -rf webapi/publish

dotnet publish webapi -c Release -o webapi/publish

echo "✅ .NET publish complete"

# ============================
# 3. VALIDATE OUTPUT
# ============================
if [ ! -d "webapi/publish/wwwroot" ]; then
  echo "❌ ERROR: wwwroot missing - Vue not copied into publish"
  exit 1
fi

echo "✅ wwwroot exists"

# ============================
# 4. DEPLOY TO AZURE (NO ZIP TOOL)
# ============================
echo "☁️ Deploying to Azure App Service..."

echo "🗜️ Creating zip using PowerShell..."

powershell.exe -Command "
Compress-Archive -Path 'webapi/publish/*' -DestinationPath 'publish.zip' -Force
"

az webapp deploy \
  --resource-group "$RESOURCE_GROUP" \
  --name "$APP_NAME" \
  --src-path "publish.zip" \
  --type zip

echo "======================================"
echo "✅ DEPLOY COMPLETE: $(date)"
echo "🌐 App: https://$APP_NAME.azurewebsites.net"
echo "📄 Log saved at: $LOG_FILE"
echo "======================================"