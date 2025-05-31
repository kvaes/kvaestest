# Events Manager

A full-stack event management application built with Vue.js frontend and C# Azure Functions backend.

## 🏗️ Architecture

This application follows a microservices architecture with:

- **Frontend**: Vue.js 3 with TypeScript, Vue Router, and Pinia
- **Backend**: Azure Functions (.NET 8) with HTTP triggers
- **Data Contract**: Shared JSON schemas for consistency
- **Infrastructure**: Docker containers with CI/CD automation

## 📁 Project Structure

```
├── .github/              # GitHub workflows, dependabot, security policy
├── backend/              # C# Azure Functions backend
├── frontend/             # Vue.js frontend application
├── datacontract/         # Shared data models and schemas
├── docs/                 # Documentation
└── README.md            # This file
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- [Docker](https://docs.docker.com/get-docker/) (optional)

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/kvaes/kvaestest.git
   cd kvaestest
   ```

2. **Start the backend**
   ```bash
   cd backend
   func start --port 7071
   ```

3. **Start the frontend** (in a new terminal)
   ```bash
   cd frontend
   npm install
   npm run dev
   ```

4. **Access the application**
   - Frontend: http://localhost:5173
   - Backend API: http://localhost:7071/api

### Using Docker

```bash
# Build and run with Docker Compose
docker-compose up --build
```

## 🌟 Features

### Events Management
- Create, read, update, and delete events
- Filter events by date and location
- Event details with registration capability

### Event Registration
- User registration for events
- Collect name, email, pronouns, and communication preferences
- Registration confirmation

### API Features
- RESTful API design
- CORS support for web clients
- Comprehensive error handling
- Input validation
- JSON response format

## 📚 Documentation

Detailed documentation is available in the `/docs` folder:

- [Local Development Setup](docs/local-development.md)
- [GitHub Codespaces Guide](docs/codespaces.md)
- [API Documentation](docs/api.md)
- [Deployment Guide](docs/deployment.md)
- [Contributing Guidelines](docs/contributing.md)

## 🛠️ Technology Stack

### Backend
- **.NET 8**: Runtime platform
- **Azure Functions**: Serverless compute platform
- **C#**: Programming language
- **System.Text.Json**: JSON serialization
- **Data Annotations**: Input validation

### Frontend
- **Vue.js 3**: Progressive JavaScript framework
- **TypeScript**: Type-safe JavaScript
- **Vue Router**: Client-side routing
- **Pinia**: State management
- **Vite**: Build tool and dev server

### DevOps
- **GitHub Actions**: CI/CD automation
- **Docker**: Containerization
- **Dependabot**: Dependency management
- **GitHub Container Registry**: Container hosting

## 🔧 Development

### Backend Development
```bash
cd backend
dotnet build          # Build the project
dotnet test           # Run tests
func start           # Start local development server
```

### Frontend Development
```bash
cd frontend
npm install          # Install dependencies
npm run dev         # Start development server
npm run build       # Build for production
npm run test:unit   # Run unit tests
npm run lint        # Lint code
```

## 🧪 Testing

### Running Tests
```bash
# Backend tests
cd backend && dotnet test

# Frontend tests
cd frontend && npm run test:unit

# E2E tests
cd frontend && npm run test:e2e
```

## 🚢 Deployment

The application is designed for containerized deployment:

### Container Images
- Backend: `ghcr.io/kvaes/kvaestest/backend:latest`
- Frontend: `ghcr.io/kvaes/kvaestest/frontend:latest`

### Production Deployment
See [Deployment Guide](docs/deployment.md) for detailed instructions.

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](docs/contributing.md) for details.

### Code of Conduct
Please note that this project is released with a Contributor Code of Conduct. By participating in this project you agree to abide by its terms.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔒 Security

Please see our [Security Policy](.github/SECURITY.md) for information about reporting security vulnerabilities.

## 📊 Project Status

- ✅ Backend API implementation
- ✅ Frontend application 
- ✅ CI/CD pipelines
- ✅ Container builds
- ✅ Documentation
- 🚧 Authentication (planned)
- 🚧 Database persistence (planned)
- 🚧 Email notifications (planned)

## 📞 Support

If you have questions or need help:

1. Check the [documentation](docs/)
2. Search [existing issues](https://github.com/kvaes/kvaestest/issues)
3. Create a [new issue](https://github.com/kvaes/kvaestest/issues/new)

---

Made with ❤️ by the Events Manager team