#!/bin/bash

# Codespace start services script - runs on container start

echo "🔄 Starting development services..."

# Function to start backend in background
start_backend() {
    echo "🚀 Starting Azure Functions backend..."
    cd backend
    nohup func start --port 7071 > /tmp/backend.log 2>&1 &
    echo $! > /tmp/backend.pid
    cd ..
    echo "✅ Backend started (PID: $(cat /tmp/backend.pid))"
}

# Function to start frontend in background
start_frontend() {
    echo "🎨 Starting Vue.js frontend..."
    cd frontend
    nohup npm run dev > /tmp/frontend.log 2>&1 &
    echo $! > /tmp/frontend.pid
    cd ..
    echo "✅ Frontend started (PID: $(cat /tmp/frontend.pid))"
}

# Wait for a service to be ready
wait_for_service() {
    local url=$1
    local name=$2
    local max_attempts=30
    local attempt=1
    
    echo "⏳ Waiting for $name to be ready..."
    
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

# Start services
start_backend
sleep 5  # Give backend time to initialize
start_frontend

# Wait for services to be ready
wait_for_service "http://localhost:7071/api/events" "Backend API"
wait_for_service "http://localhost:5173" "Frontend"

echo ""
echo "🎉 All services are running!"
echo ""
echo "📊 Service Status:"
echo "  Backend:  http://localhost:7071/api"
echo "  Frontend: http://localhost:5173"
echo ""
echo "📝 Logs:"
echo "  Backend:  tail -f /tmp/backend.log"
echo "  Frontend: tail -f /tmp/frontend.log"
echo ""
echo "🔧 Management:"
echo "  Stop backend:  kill \$(cat /tmp/backend.pid)"
echo "  Stop frontend: kill \$(cat /tmp/frontend.pid)"