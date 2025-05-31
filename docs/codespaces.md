# GitHub Codespaces Development Guide

This guide explains how to set up and use GitHub Codespaces for developing the Events Manager application in a cloud-based development environment.

## What is GitHub Codespaces?

GitHub Codespaces provides a complete development environment in the cloud, accessible through your browser or Visual Studio Code. It comes pre-configured with all the tools you need to develop, build, and test the application.

## Quick Start

### Opening a Codespace

1. **From the GitHub Repository**

   - Navigate to https://github.com/kvaes/kvaestest
   - Click the green "Code" button
   - Select the "Codespaces" tab
   - Click "Create codespace on main"

2. **From Visual Studio Code**
   - Install the "GitHub Codespaces" extension
   - Open the Command Palette (Ctrl+Shift+P / Cmd+Shift+P)
   - Type "Codespaces: Create New Codespace"
   - Select the repository

### First-Time Setup

When your Codespace opens for the first time:

1. **Wait for automatic setup** (2-3 minutes)

   - Dependencies will be automatically installed
   - Development servers will be configured
   - Extensions will be loaded

2. **Verify the setup**
   - Open the integrated terminal (Ctrl+` / Cmd+`)
   - Check that both frontend and backend dependencies are installed

## Codespace Configuration

The repository includes a `.devcontainer` configuration that automatically:

- Installs .NET 8 SDK
- Installs Node.js 20
- Installs Azure Functions Core Tools
- Configures VS Code with recommended extensions
- Sets up port forwarding for the application
- Installs all project dependencies

### Pre-installed Extensions

Your Codespace comes with these VS Code extensions:

- **Azure Functions** - For backend development
- **C# Dev Kit** - C# language support
- **Vue Language Features** - Vue.js development
- **TypeScript and JavaScript** - Frontend language support
- **REST Client** - For API testing
- **Docker** - Container support
- **GitLens** - Enhanced Git capabilities

## Running the Application

### Automatic Startup (Recommended)

The Codespace is configured to automatically start both services:

1. **Check running services**

   ```bash
   # The following should show running processes:
   ps aux | grep -E "(func|npm)"
   ```

2. **Access the application**
   - VS Code will automatically show port forwarding notifications
   - Click on the ports to open the application
   - Frontend: Port 5173
   - Backend API: Port 7071

### Manual Startup

If services aren't running automatically:

1. **Start the backend**

   ```bash
   cd backend
   func start --port 7071
   ```

2. **Start the frontend** (new terminal)
   ```bash
   cd frontend
   npm run dev
   ```

## Development Workflow

### Making Changes

1. **Edit files** using the VS Code interface
2. **Save changes** - auto-save is enabled by default
3. **Test changes** - both services support hot reload
4. **View in browser** - click on forwarded port links

### Running Tests

```bash
# Backend tests
cd backend
dotnet test

# Frontend unit tests
cd frontend
npm run test:unit

# Frontend e2e tests (runs in headless mode)
npm run test:e2e:ci
```

### Building for Production

```bash
# Build backend
cd backend
dotnet build --configuration Release

# Build frontend
cd frontend
npm run build
```

## Port Forwarding

GitHub Codespaces automatically forwards these ports:

| Service  | Port | URL            | Description               |
| -------- | ---- | -------------- | ------------------------- |
| Frontend | 5173 | Auto-generated | Vue.js development server |
| Backend  | 7071 | Auto-generated | Azure Functions runtime   |

### Accessing Your Application

1. **View forwarded ports**

   - Go to the "Ports" tab in VS Code
   - Or use Ctrl+Shift+` / Cmd+Shift+`

2. **Open application**

   - Click the globe icon next to port 5173 for the frontend
   - Click the globe icon next to port 7071 for backend API

3. **Share with others** (if needed)
   - Right-click on a port
   - Select "Port Visibility" → "Public"
   - Share the generated URL

## Full End-to-End Testing

### Scenario 1: Create and Manage Events

1. **Open the frontend** (port 5173)
2. **Create a new event**
   - Click "Create Event"
   - Fill in event details
   - Submit the form
3. **Verify in API**
   - Open port 7071 in new tab
   - Navigate to `/api/events`
   - Verify the event appears in JSON response

### Scenario 2: Event Registration

1. **Create an event** (if not already done)
2. **View event details**
   - Click on an event from the list
   - Verify all event information displays correctly
3. **Register for the event**
   - Fill in the registration form
   - Submit registration
   - Verify success message

### Scenario 3: API Testing

Use the integrated terminal to test API endpoints:

```bash
# Test events endpoint
curl http://localhost:7071/api/events

# Create an event via API
curl -X POST http://localhost:7071/api/events \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Codespace Test Event",
    "location": "Cloud Development Environment",
    "date": "2025-12-31",
    "startTime": "16:00"
  }'

# Register for an event (replace {eventId} with actual ID)
curl -X POST http://localhost:7071/api/registrations \
  -H "Content-Type: application/json" \
  -d '{
    "eventId": "{eventId}",
    "name": "Test User",
    "email": "test@example.com",
    "pronouns": "they/them",
    "optInCommunication": true
  }'
```

## Container Development

### Running with Docker

Your Codespace supports Docker development:

1. **Build containers**

   ```bash
   # Build backend container
   cd backend
   docker build -t events-backend .

   # Build frontend container
   cd ../frontend
   docker build -t events-frontend .
   ```

2. **Run with Docker Compose**

   ```bash
   # Create a simple docker-compose.yml
   cat > docker-compose.dev.yml << EOF
   version: '3.8'
   services:
     backend:
       build: ./backend
       ports:
         - "7071:80"
       environment:
         - AzureWebJobsStorage=UseDevelopmentStorage=true
         - FUNCTIONS_WORKER_RUNTIME=dotnet-isolated

     frontend:
       build: ./frontend
       ports:
         - "3000:80"
       depends_on:
         - backend
   EOF

   # Start services
   docker compose -f docker-compose.dev.yml up --build
   ```

## Troubleshooting

### Common Issues

1. **Codespace won't start**

   - Wait a few minutes for initial setup
   - Refresh the browser if the environment seems stuck
   - Check GitHub status page for service issues

2. **Ports not forwarding**

   - Check the "Ports" tab in VS Code
   - Manually forward ports: Ctrl+Shift+P → "Forward a Port"
   - Verify services are actually running

3. **Backend compilation errors**

   ```bash
   cd backend
   dotnet clean
   dotnet restore
   dotnet build
   ```

4. **Frontend build issues**

   ```bash
   cd frontend
   rm -rf node_modules package-lock.json
   npm install
   npm run build
   ```

5. **Can't access application**
   - Ensure ports are public if sharing with others
   - Check firewall/corporate network restrictions
   - Try opening port URLs in incognito mode

### Performance Tips

1. **Keep Codespace active**

   - Codespaces auto-suspend after 30 minutes of inactivity
   - Keep a terminal process running to prevent suspension

2. **Optimize resource usage**

   - Close unused browser tabs
   - Stop unnecessary services when not needed

3. **Use VS Code settings sync**
   - Enable Settings Sync to maintain your preferences across Codespaces

## Advanced Features

### Database Integration (Future)

When database functionality is added:

```bash
# Example: Running with a containerized database
docker run -d --name events-db \
  -e POSTGRES_PASSWORD=dev123 \
  -e POSTGRES_DB=events \
  -p 5432:5432 \
  postgres:15
```

### Secrets Management

For production-like testing:

1. **Add repository secrets**

   - Go to repository Settings → Secrets and variables → Codespaces
   - Add secrets like `DATABASE_CONNECTION_STRING`

2. **Access in Codespace**
   ```bash
   echo $DATABASE_CONNECTION_STRING
   ```

### Custom Development Container

To modify the Codespace configuration:

1. **Edit `.devcontainer/devcontainer.json`**
2. **Rebuild Codespace**
   - Ctrl+Shift+P → "Codespaces: Rebuild Container"

## Best Practices

### Development Workflow

1. **Create feature branches**

   ```bash
   git checkout -b feature/new-functionality
   ```

2. **Commit frequently**

   ```bash
   git add .
   git commit -m "Add new feature"
   ```

3. **Push changes**

   ```bash
   git push origin feature/new-functionality
   ```

4. **Create pull requests** from the GitHub web interface

### Resource Management

1. **Stop your Codespace** when not in use

   - Go to https://github.com/codespaces
   - Stop inactive Codespaces to save on billing

2. **Delete old Codespaces**
   - Remove Codespaces you no longer need
   - Keep your workspace clean

## Getting Help

If you encounter issues with Codespaces:

1. **Check GitHub Docs**

   - [GitHub Codespaces Documentation](https://docs.github.com/en/codespaces)

2. **Repository-specific help**

   - Check [local development guide](local-development.md)
   - Create an issue with "codespaces" label

3. **GitHub Support**
   - Contact GitHub support for platform-specific issues

## Benefits of Codespaces Development

- ✅ **Zero setup time** - Everything is pre-configured
- ✅ **Consistent environment** - Same setup for all developers
- ✅ **Cloud resources** - Powerful machines for development
- ✅ **Access anywhere** - Develop from any device with a browser
- ✅ **Collaboration** - Easy to share and pair program
- ✅ **Security** - Isolated environments for each workspace

---

Happy coding in the cloud! 🚀
