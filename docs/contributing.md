# Contributing to Events Manager

Thank you for your interest in contributing to Events Manager! This document provides guidelines and information for contributors.

## 🤝 How to Contribute

### Reporting Issues

Before creating an issue, please:

1. **Search existing issues** to avoid duplicates
2. **Use a clear and descriptive title**
3. **Provide detailed information** including:
   - Steps to reproduce
   - Expected behavior
   - Actual behavior
   - Environment details (OS, browser, versions)
   - Screenshots if applicable

### Suggesting Enhancements

Enhancement suggestions are welcome! Please:

1. **Check existing feature requests** first
2. **Explain the use case** and benefits
3. **Provide examples** of how it would work
4. **Consider the scope** - smaller, focused features are easier to implement

### Pull Requests

We actively welcome pull requests! Here's how to get started:

1. **Fork the repository**
2. **Create a feature branch** from `main`
3. **Make your changes** following our guidelines
4. **Test your changes** thoroughly
5. **Submit a pull request** with a clear description

## 🛠️ Development Setup

### Prerequisites

- .NET 8 SDK
- Node.js 20+
- Azure Functions Core Tools
- Git

### Local Setup

```bash
# Clone your fork
git clone https://github.com/YOUR_USERNAME/kvaestest.git
cd kvaestest

# Set up backend
cd backend
dotnet restore
dotnet build

# Set up frontend  
cd ../frontend
npm install
npm run build

# Start development servers
cd ../backend && func start --port 7071 &
cd ../frontend && npm run dev &
```

For detailed setup instructions, see [Local Development Guide](docs/local-development.md).

## 📝 Code Guidelines

### General Principles

- **Write clean, readable code** with meaningful names
- **Follow existing patterns** and conventions
- **Keep functions and methods small** and focused
- **Add comments** for complex logic only
- **Write tests** for new functionality

### Backend Guidelines (.NET/C#)

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use **PascalCase** for public members
- Use **camelCase** for private fields
- **Validate inputs** at API boundaries
- **Handle exceptions** appropriately
- **Add XML documentation** for public APIs

Example:
```csharp
public class EventService : IEventService
{
    private readonly List<Event> _events = new();
    
    /// <summary>
    /// Creates a new event with the specified details.
    /// </summary>
    /// <param name="request">The event creation request</param>
    /// <returns>The created event</returns>
    public async Task<Event> CreateEventAsync(EventCreateRequest request)
    {
        // Implementation...
    }
}
```

### Frontend Guidelines (Vue.js/TypeScript)

- Follow [Vue.js Style Guide](https://vuejs.org/style-guide/)
- Use **TypeScript** for type safety
- Use **composition API** for new components
- **Extract reusable logic** into composables
- **Props should be typed** with interfaces
- **Use meaningful component names**

Example:
```vue
<template>
  <div class="event-card">
    <h3>{{ event.name }}</h3>
    <p>{{ formatDate(event.date) }}</p>
  </div>
</template>

<script setup lang="ts">
interface Props {
  event: Event
}

const props = defineProps<Props>()

const formatDate = (date: string): string => {
  return new Date(date).toLocaleDateString()
}
</script>
```

### Data Contract Guidelines

- **Keep models consistent** between frontend and backend
- **Use clear, descriptive property names**
- **Include validation attributes** in backend models
- **Document all fields** in schemas

## 🧪 Testing

### Backend Testing

```bash
cd backend
dotnet test
```

### Frontend Testing

```bash
cd frontend
npm run test:unit        # Unit tests
npm run test:e2e:ci     # E2E tests (CI mode)
npm run lint           # Linting
npm run type-check     # TypeScript checking
```

### Test Guidelines

- **Write tests for new features**
- **Update tests when changing existing code**
- **Use descriptive test names**
- **Keep tests focused and isolated**
- **Mock external dependencies**

## 📚 Documentation

When contributing, please:

- **Update relevant documentation** in the `/docs` folder
- **Add API documentation** for new endpoints
- **Update README** if adding new features
- **Include code comments** for complex logic

## 🔄 Development Workflow

### Branch Naming

Use descriptive branch names:
- `feature/add-event-filtering`
- `bugfix/fix-registration-validation`
- `docs/update-api-documentation`
- `refactor/improve-error-handling`

### Commit Messages

Use clear, descriptive commit messages:
- Start with a verb (Add, Fix, Update, Remove)
- Keep the first line under 50 characters
- Add details in the body if needed

Examples:
```
Add event filtering by location and date

- Implement query parameter parsing
- Add location contains search
- Add date exact match filtering
- Update API documentation
```

### Pull Request Process

1. **Create a pull request** with:
   - Clear title and description
   - Reference to related issues
   - Screenshots for UI changes
   - Test results

2. **Code review process**:
   - Address reviewer feedback promptly
   - Make requested changes in new commits
   - Keep discussions respectful and constructive

3. **Merge requirements**:
   - All CI checks must pass
   - At least one reviewer approval
   - Up-to-date with main branch

## 🚀 Release Process

- We use semantic versioning (MAJOR.MINOR.PATCH)
- Features go into the next minor version
- Bug fixes may go into patch releases
- Breaking changes require a major version bump

## 📦 Project Structure

```
├── .github/              # GitHub workflows and templates
├── backend/              # C# Azure Functions backend
│   ├── Functions/        # HTTP trigger functions
│   ├── Models/           # Data models
│   ├── Services/         # Business logic
│   └── Dockerfile        # Container configuration
├── frontend/             # Vue.js frontend application
│   ├── src/
│   │   ├── components/   # Reusable Vue components
│   │   ├── views/        # Page components
│   │   ├── services/     # API and utility services
│   │   └── types/        # TypeScript type definitions
│   └── Dockerfile        # Container configuration
├── datacontract/         # Shared data models and schemas
├── docs/                 # Project documentation
└── README.md            # Project overview
```

## 🎯 Areas for Contribution

We welcome contributions in these areas:

### High Priority
- 🐛 **Bug fixes** and stability improvements
- 📝 **Documentation** improvements
- 🧪 **Test coverage** expansion
- ♿ **Accessibility** enhancements

### Medium Priority
- ✨ **New features** (see issues labeled `enhancement`)
- 🔧 **Developer experience** improvements
- 🎨 **UI/UX** enhancements
- 📊 **Performance** optimizations

### Future Considerations
- 🔐 **Authentication** implementation
- 💾 **Database** persistence
- 📧 **Email** notifications
- 🌐 **Internationalization**

## ❓ Getting Help

If you need help with contributing:

1. **Check the documentation** in the `/docs` folder
2. **Search existing issues** and discussions
3. **Ask questions** in issue comments
4. **Join discussions** in pull requests
5. **Create a new issue** with the `question` label

## 🏆 Recognition

Contributors will be:
- **Listed in release notes** for significant contributions
- **Mentioned in the README** for ongoing contributions
- **Invited as collaborators** for sustained contributions

## 📄 License

By contributing to this project, you agree that your contributions will be licensed under the MIT License.

## 🤗 Code of Conduct

This project follows a Code of Conduct to ensure a welcoming environment:

- **Be respectful** and inclusive
- **Focus on what is best** for the community
- **Show empathy** towards other community members
- **Accept constructive criticism** gracefully
- **Use welcoming and inclusive language**

---

Thank you for contributing to Events Manager! 🎉