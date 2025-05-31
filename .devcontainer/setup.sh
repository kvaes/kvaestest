#!/bin/bash

# Codespace setup script - runs after container creation

echo "🚀 Setting up Events Manager development environment..."

# Install Azure Functions Core Tools
echo "📦 Installing Azure Functions Core Tools..."
npm install -g azure-functions-core-tools@4 --unsafe-perm true

# Backend setup
echo "🔧 Setting up backend..."
cd backend
dotnet restore
dotnet build
cd ..

# Frontend setup
echo "🎨 Setting up frontend..."
cd frontend
npm install
npm run type-check
cd ..

# Create environment file if it doesn't exist
if [ ! -f frontend/.env.local ]; then
    echo "🔧 Creating frontend environment file..."
    echo "VITE_API_BASE_URL=http://localhost:7071/api" > frontend/.env.local
fi

# Set up Git configuration
echo "🔧 Configuring Git..."
git config --global init.defaultBranch main
git config --global core.autocrlf input

echo "✅ Setup complete! Ready for development."
echo ""
echo "Next steps:"
echo "1. Start the backend: cd backend && func start"
echo "2. Start the frontend: cd frontend && npm run dev"
echo "3. Open the forwarded ports to access the application"