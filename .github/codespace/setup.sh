#!/usr/bin/env bash
set -euo pipefail

# Install .NET SDK 9
curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
chmod +x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 9.0

# Persist PATH for future shells
if ! grep -q '.dotnet' ~/.bashrc; then
  echo 'export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"' >> ~/.bashrc
fi
export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"

# Pin SDK (optional)
cat > /workspaces/${PWD##*/}/global.json << 'JSON'
{
  "sdk": { "version": "9.0.100", "rollForward": "latestFeature" }
}
JSON

# Build once (warm cache)
dotnet build || true

# EF Core CLI
dotnet tool install --global dotnet-ef || true
dotnet ef --version || true
