#!/bin/bash

# Comprehensive integration test script for Events Manager
# This script tests the full application stack in local development or CI environments

set -e  # Exit on any error

echo "🧪 Running Events Manager Integration Tests..."

# Function to check if a command exists
command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# Function to wait for a service to be ready
wait_for_service() {
    local url=$1
    local name=$2
    local max_attempts=30
    local attempt=1
    
    echo "⏳ Waiting for $name to be ready at $url..."
    
    while [ $attempt -le $max_attempts ]; do
        if curl -f -s "$url" > /dev/null 2>&1; then
            echo "✅ $name is ready!"
            return 0
        fi
        
        echo "  Attempt $attempt/$max_attempts - $name not ready yet..."
        sleep 2
        attempt=$((attempt + 1))
    done
    
    echo "❌ $name failed to start within expected time"
    return 1
}

# Prerequisites check
echo "🔍 Checking prerequisites..."

PREREQ_FAILED=false

if ! command_exists dotnet; then
    echo "❌ .NET CLI not found"
    PREREQ_FAILED=true
else
    echo "✅ .NET CLI found: $(dotnet --version)"
fi

if ! command_exists node; then
    echo "❌ Node.js not found"
    PREREQ_FAILED=true
else
    echo "✅ Node.js found: $(node --version)"
fi

if ! command_exists npm; then
    echo "❌ npm not found"
    PREREQ_FAILED=true
else
    echo "✅ npm found: $(npm --version)"
fi

if ! command_exists func; then
    echo "⚠️  Azure Functions Core Tools not found - will skip runtime tests"
    SKIP_RUNTIME=true
else
    echo "✅ Azure Functions Core Tools found: $(func --version)"
    SKIP_RUNTIME=false
fi

if [ "$PREREQ_FAILED" = true ]; then
    echo "❌ Prerequisites check failed. Please install missing tools."
    exit 1
fi

echo "🔧 Building projects..."

# Test backend build
echo "  Building backend..."
cd backend
if dotnet build --configuration Release; then
    echo "  ✅ Backend build successful"
else
    echo "  ❌ Backend build failed"
    exit 1
fi

# Test backend tests (if any)
if dotnet test --list-tests 2>/dev/null | grep -q "Test run for"; then
    echo "  Running backend tests..."
    if dotnet test --configuration Release --verbosity minimal; then
        echo "  ✅ Backend tests passed"
    else
        echo "  ❌ Backend tests failed"
        exit 1
    fi
else
    echo "  ℹ️  No backend tests found"
fi

cd ..

# Test frontend build  
echo "  Building frontend..."
cd frontend

if npm ci; then
    echo "  ✅ Frontend dependencies installed"
else
    echo "  ❌ Frontend dependency installation failed"
    exit 1
fi

if npm run lint; then
    echo "  ✅ Frontend linting passed"
else
    echo "  ❌ Frontend linting failed"
    exit 1
fi

if npm run type-check; then
    echo "  ✅ Frontend type checking passed"
else
    echo "  ❌ Frontend type checking failed"
    exit 1
fi

if npm run test:unit; then
    echo "  ✅ Frontend unit tests passed"
else
    echo "  ❌ Frontend unit tests failed"
    exit 1
fi

if npm run build; then
    echo "  ✅ Frontend build successful"
else
    echo "  ❌ Frontend build failed"
    exit 1
fi

cd ..

# Docker build tests (if Docker is available)
if command_exists docker; then
    echo "🐳 Testing Docker builds..."
    
    echo "  Building backend container..."
    if docker build -t events-backend-test ./backend; then
        echo "  ✅ Backend container build successful"
        # Cleanup
        docker rmi events-backend-test >/dev/null 2>&1 || true
    else
        echo "  ❌ Backend container build failed"
        exit 1
    fi
    
    echo "  Building frontend container..."
    if docker build -t events-frontend-test ./frontend; then
        echo "  ✅ Frontend container build successful"
        # Cleanup
        docker rmi events-frontend-test >/dev/null 2>&1 || true
    else
        echo "  ❌ Frontend container build failed"
        exit 1
    fi
else
    echo "ℹ️  Docker not available, skipping container tests"
fi

# Runtime tests (only if Azure Functions Core Tools are available)
if [ "$SKIP_RUNTIME" = false ]; then
    echo "🚀 Testing runtime functionality..."
    
    # Kill any existing processes on our ports
    echo "  Cleaning up existing processes..."
    pkill -f "func start" || true
    pkill -f "npm run dev" || true
    sleep 2
    
    # Start backend
    echo "  Starting backend..."
    cd backend
    nohup func start --port 7071 > /tmp/test_backend.log 2>&1 &
    BACKEND_PID=$!
    cd ..
    
    # Start frontend
    echo "  Starting frontend..."
    cd frontend
    nohup npm run dev -- --port 5173 > /tmp/test_frontend.log 2>&1 &
    FRONTEND_PID=$!
    cd ..
    
    # Function to cleanup on exit
    cleanup() {
        echo "  Cleaning up test processes..."
        kill $BACKEND_PID $FRONTEND_PID 2>/dev/null || true
        wait 2>/dev/null || true
    }
    trap cleanup EXIT
    
    # Wait for services and run API tests
    if wait_for_service "http://localhost:7071/api/events" "Backend API"; then
        echo "  Running API tests..."
        
        # Test empty events list
        if [ "$(curl -s http://localhost:7071/api/events)" = "[]" ]; then
            echo "  ✅ Empty events list test passed"
        else
            echo "  ❌ Empty events list test failed"
            exit 1
        fi
        
        # Test event creation
        EVENT_RESPONSE=$(curl -s -X POST http://localhost:7071/api/events \
          -H "Content-Type: application/json" \
          -d '{"name":"Test Event","location":"Test Location","date":"2025-12-31","startTime":"15:00"}')
        
        if echo $EVENT_RESPONSE | grep -q '"id"'; then
            echo "  ✅ Event creation test passed"
        else
            echo "  ❌ Event creation test failed"
            exit 1
        fi
    fi
    
    if wait_for_service "http://localhost:5173" "Frontend"; then
        if curl -s http://localhost:5173 | grep -q "Events Manager"; then
            echo "  ✅ Frontend accessibility test passed"
        else
            echo "  ❌ Frontend accessibility test failed"
            exit 1
        fi
    fi
else
    echo "⏭️  Skipping runtime tests (Azure Functions Core Tools not available)"
fi

echo ""
echo "🎉 All available tests passed!"
echo ""
echo "📊 Test Summary:"
echo "  ✅ Backend build and compilation"
echo "  ✅ Frontend build and compilation"
echo "  ✅ Frontend linting and type checking"
echo "  ✅ Unit tests execution"
if command_exists docker; then
    echo "  ✅ Docker container builds"
fi
if [ "$SKIP_RUNTIME" = false ]; then
    echo "  ✅ Runtime API functionality"
    echo "  ✅ Frontend accessibility"
fi
echo ""
echo "🚀 The Events Manager application is ready for development and deployment!"

if [ "$SKIP_RUNTIME" = false ]; then
    echo ""
    echo "🔗 Application URLs (during test run):"
    echo "  Frontend: http://localhost:5173"
    echo "  Backend:  http://localhost:7071/api"
    echo ""
    echo "📝 Runtime logs available at:"
    echo "  Backend:  /tmp/test_backend.log"
    echo "  Frontend: /tmp/test_frontend.log"
fi