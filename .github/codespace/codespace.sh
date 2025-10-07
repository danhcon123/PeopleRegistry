# See what SDKs you have
dotnet --list-sdks

# Install the .NET install script
curl -fsSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh

# Install .NET 9 SDK for current user
./dotnet-install.sh --channel 9.0

# Make sure the new dotnet is on PATH for future shells
echo 'export PATH="$HOME/.dotnet:$PATH"' >> ~/.bashrc
echo 'export PATH="$HOME/.dotnet/tools:$PATH"' >> ~/.bashrc   # 👈 add tools path as well
source ~/.bashrc

# Verify .NET 9 is visible
dotnet --list-sdks

# (Optional but recommended) pin SDK via global.json in your repo root
cat > global.json << 'JSON'
{
  "sdk": { "version": "9.0.100", "rollForward": "latestFeature" }
}
JSON

# Build again
dotnet build

# ✅ Install EF Core CLI tool globally (so 'dotnet ef' works)
dotnet tool install --global dotnet-ef

# Verify dotnet-ef
dotnet ef --version
