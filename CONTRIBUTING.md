# Contributing to PlcAssistant

Thank you for your interest in contributing to PlcAssistant! This document provides guidelines for contributing to the project.

## Code of Conduct

By participating in this project, you agree to maintain a respectful and inclusive environment for all contributors.

## How to Contribute

### Reporting Bugs

1. Check if the bug has already been reported in the Issues section
2. If not, create a new issue with:
   - Clear title and description
   - Steps to reproduce
   - Expected vs actual behavior
   - System information (OS, .NET version, etc.)
   - Screenshots if applicable

### Suggesting Features

1. Check if the feature has already been suggested
2. Create a new issue with:
   - Clear description of the feature
   - Use cases and benefits
   - Possible implementation approach

### Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes following our coding standards
4. Commit your changes (`git commit -m 'Add amazing feature'`)
5. Push to the branch (`git push origin feature/amazing-feature`)
6. Open a Pull Request

## Development Setup

### Prerequisites
- .NET 10 SDK
- For MAUI development: MAUI workload on macOS or Windows
- Git
- IDE: Visual Studio 2022, VS Code, or Rider

### Getting Started
```bash
# Clone the repository
git clone https://github.com/alemik/PlcAssitant.git
cd PlcAssitant

# Build the server components
cd src/PlcAssistant.Server
dotnet build

# Run with seed data
dotnet run -- --seed
```

See [BUILD.md](BUILD.md) for detailed build instructions.

## Project Structure

```
PlcAssistant/
├── src/
│   ├── PlcAssistant.Domain/         # Domain entities and interfaces
│   ├── PlcAssistant.Infrastructure/ # Data access and services
│   ├── PlcAssistant.Server/         # Background service
│   └── PlcAssistant.MauiApp/        # MAUI Hybrid UI
├── docs/                            # Documentation (if added)
└── tests/                           # Unit tests (if added)
```

## Coding Standards

### General Guidelines
- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and small
- Use async/await for I/O operations

### Domain Layer
- Keep domain logic pure and framework-independent
- No external dependencies in domain entities
- Use value objects for complex types
- Define clear interfaces

### Infrastructure Layer
- Implement domain interfaces
- Handle all external dependencies (database, logging, etc.)
- Use async patterns consistently
- Handle exceptions appropriately

### UI Layer
- Keep Blazor components focused
- Use proper component lifecycle methods
- Handle loading and error states
- Make UI responsive

### Example Code Style
```csharp
namespace PlcAssistant.Domain.Entities;

/// <summary>
/// Represents a PLC memory address
/// </summary>
public class PlcMemoryAddress
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Gets or sets the display name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    // ... rest of properties
}
```

## Testing Guidelines

### Unit Tests
- Write tests for domain logic
- Test edge cases and error conditions
- Use descriptive test names
- Follow Arrange-Act-Assert pattern

### Example Test Structure
```csharp
public class PlcMemoryAddressTests
{
    [Fact]
    public async Task AddAsync_ShouldAddAddress()
    {
        // Arrange
        var repository = new PlcMemoryAddressRepository(connectionString);
        var address = new PlcMemoryAddress { Name = "Test" };
        
        // Act
        var result = await repository.AddAsync(address);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }
}
```

## Commit Messages

Use clear and descriptive commit messages:

### Format
```
<type>: <subject>

<body>

<footer>
```

### Types
- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting, etc.)
- **refactor**: Code refactoring
- **test**: Adding or updating tests
- **chore**: Maintenance tasks

### Examples
```
feat: Add support for Real data type in PLC addresses

Implemented Real (floating-point) data type support in the
memory address entity and repository.

Closes #42
```

```
fix: Resolve database lock issue when multiple processes access LiteDB

Changed connection string to use shared mode to allow concurrent access.

Fixes #38
```

## Branch Naming

- `feature/description` - New features
- `fix/description` - Bug fixes
- `docs/description` - Documentation updates
- `refactor/description` - Code refactoring

## Pull Request Guidelines

### Before Submitting
- [ ] Code builds successfully
- [ ] Tests pass (if applicable)
- [ ] Documentation updated (if needed)
- [ ] Commit messages are clear
- [ ] Code follows project style guidelines

### PR Description Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Refactoring

## Testing
Describe testing performed

## Screenshots (if applicable)
Add screenshots for UI changes

## Related Issues
Fixes #(issue number)
```

## Architecture Decisions

When making significant architectural changes:
1. Discuss in an issue first
2. Consider impact on existing code
3. Document the decision
4. Update ARCHITECTURE.md if needed

## Questions?

Feel free to:
- Open an issue for questions
- Join discussions in existing issues
- Contact the maintainers

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Recognition

Contributors will be recognized in:
- GitHub contributors list
- Release notes (for significant contributions)
- Special thanks in README (for major features)

Thank you for contributing to PlcAssistant! 🚀
