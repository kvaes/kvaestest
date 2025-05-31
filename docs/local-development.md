# Local Development Setup

This guide will help you set up the Events Manager application for local development.

## Prerequisites

Before you begin, ensure you have the following tools installed:

### Required Tools

1. **Git**
   - Version 2.20 or later
   - [Download Git](https://git-scm.com/downloads)

2. **.NET 8 SDK**
   - Version 8.0 or later
   - [Download .NET](https://dotnet.microsoft.com/download)
   - Verify installation: `dotnet --version`

3. **Node.js**
   - Version 20.x or later
   - [Download Node.js](https://nodejs.org/)
   - Verify installation: `node --version` and `npm --version`

4. **Azure Functions Core Tools**
   - Version 4.x
   - Install via npm: `npm install -g azure-functions-core-tools@4 --unsafe-perm true`
   - Verify installation: `func --version`

### Optional Tools

5. **Docker Desktop** (for containerized development)
   - [Download Docker Desktop](https://docs.docker.com/get-docker/)
   - Verify installation: `docker --version`

6. **Visual Studio Code** (recommended IDE)
   - [Download VS Code](https://code.visualstudio.com/)
   - Recommended extensions:
     - Azure Functions
     - C# Dev Kit
     - Vue Language Features (Vetur)
     - TypeScript and JavaScript Language Features

## Initial Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/kvaes/kvaestest.git
   cd kvaestest
   ```

2. **Verify Project Structure**
   ```bash
   ls -la
   # You should see: backend/, frontend/, docs/, datacontract/, .github/
   ```

## Backend Setup

1. **Navigate to the backend directory**
   ```bash
   cd backend
   ```

2. **Restore .NET dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run tests** (optional)
   ```bash
   dotnet test
   ```

5. **Check local settings**
   ```bash
   cat local.settings.json
   ```
   
   The file should contain:
   ```json
   {
       "IsEncrypted": false,
       "Values": {
           "AzureWebJobsStorage": "UseDevelopmentStorage=true",
           "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
       }
   }
   ```

## Frontend Setup

1. **Navigate to the frontend directory**
   ```bash
   cd ../frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Check environment configuration**
   ```bash
   cat .env.local
   ```
   
   The file should contain:
   ```env
   VITE_API_BASE_URL=http://localhost:7071/api
   ```

4. **Run linting** (optional)
   ```bash
   npm run lint
   ```

5. **Run type checking** (optional)
   ```bash
   npm run type-check
   ```

## Running the Application

### Start the Backend

1. **Open a terminal in the backend directory**
   ```bash
   cd backend
   ```

2. **Start the Azure Functions runtime**
   ```bash
   func start --port 7071
   ```

   You should see output similar to:
   ```
   Azure Functions Core Tools
   Core Tools Version:       4.0.x
   Function Runtime Version: 4.x
   
   Functions:
   
   CreateEvent: [POST] http://localhost:7071/api/events
   GetEvent: [GET] http://localhost:7071/api/events/{id}
   GetEvents: [GET] http://localhost:7071/api/events
   UpdateEvent: [PUT] http://localhost:7071/api/events/{id}
   DeleteEvent: [DELETE] http://localhost:7071/api/events/{id}
   ```

3. **Test the backend** (in another terminal)
   ```bash
   curl http://localhost:7071/api/events
   # Should return: []
   ```

### Start the Frontend

1. **Open a new terminal in the frontend directory**
   ```bash
   cd frontend
   ```

2. **Start the development server**
   ```bash
   npm run dev
   ```

   You should see output similar to:
   ```
   VITE v6.x.x  ready in xxx ms
   
   ➜  Local:   http://localhost:5173/
   ➜  Network: use --host to expose
   ➜  press h to show help
   ```

3. **Access the application**
   - Open your browser to http://localhost:5173
   - You should see the Events Manager interface

## Testing the Setup

### Basic Functionality Test

1. **Create a test event**
   - Click "Create Event" in the frontend
   - Fill in the form:
     - Name: "Test Event"
     - Location: "Test Location"
     - Date: Tomorrow's date
     - Start Time: "14:00"
   - Click "Create Event"

2. **Verify the event appears**
   - You should be redirected to the events list
   - Your test event should be visible

3. **Test registration**
   - Click on your test event
   - Fill in the registration form
   - Submit the registration

### API Testing

You can also test the API directly:

```bash
# Create an event
curl -X POST http://localhost:7071/api/events \
  -H "Content-Type: application/json" \
  -d '{
    "name": "API Test Event",
    "location": "API Test Location", 
    "date": "2025-12-31",
    "startTime": "15:00"
  }'

# Get all events
curl http://localhost:7071/api/events

# Get events for a specific date
curl "http://localhost:7071/api/events?date=2025-12-31"
```

## Development Workflow

### Making Changes

1. **Backend Changes**
   - Edit files in the `backend/` directory
   - The Functions runtime will automatically reload
   - Check terminal for any compilation errors

2. **Frontend Changes**
   - Edit files in the `frontend/src/` directory
   - Vite will automatically hot-reload the browser
   - Check browser console for any errors

### Common Development Tasks

```bash
# Backend: Add new dependencies
cd backend
dotnet add package <PackageName>

# Frontend: Add new dependencies  
cd frontend
npm install <package-name>

# Run all tests
cd backend && dotnet test
cd ../frontend && npm run test:unit

# Build for production
cd backend && dotnet build --configuration Release
cd ../frontend && npm run build
```

## Troubleshooting

### Common Issues

1. **Backend won't start**
   - Check that .NET 8 SDK is installed: `dotnet --version`
   - Check that Azure Functions Core Tools are installed: `func --version`
   - Try cleaning and rebuilding: `dotnet clean && dotnet build`

2. **Frontend won't start**
   - Check that Node.js is installed: `node --version`
   - Try deleting node_modules and reinstalling: `rm -rf node_modules && npm install`
   - Check for port conflicts (default port 5173)

3. **CORS errors in browser**
   - Ensure backend is running on port 7071
   - Check that CORS headers are being added in the backend functions
   - Verify `.env.local` has the correct API URL

4. **API calls failing**
   - Check browser network tab for failed requests
   - Verify backend is running and accessible
   - Check for any JSON formatting issues

### Getting Help

If you encounter issues not covered here:

1. Check the [troubleshooting section](troubleshooting.md)
2. Search [existing issues](https://github.com/kvaes/kvaestest/issues)
3. Create a [new issue](https://github.com/kvaes/kvaestest/issues/new) with:
   - Your operating system
   - Version numbers of installed tools
   - Complete error messages
   - Steps to reproduce the issue

## Next Steps

Once you have the local environment working:

1. Read the [API Documentation](api.md)
2. Learn about [GitHub Codespaces setup](codespaces.md)
3. Review the [Contributing Guidelines](contributing.md)