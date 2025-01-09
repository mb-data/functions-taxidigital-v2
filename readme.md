# TaxiDigital

TaxiDigital is a .NET 8 application designed to manage taxi services efficiently. This project is structured into multiple layers to ensure separation of concerns and maintainability.

## Projects

### TaxiDigital.Application
This project contains the application logic and business rules. It includes:
- **FluentValidation**: For validating request models.
- **MediatR**: For implementing the mediator pattern.
- **Microsoft.Extensions.Logging.Abstractions**: For logging abstractions.
- **Serilog**: For structured logging.

#### Project References
- **TaxiDigital.SharedKernel**: Contains shared code and utilities used across multiple projects.

### TaxiDigital.Infrastructure
This project contains the infrastructure and data access logic. It includes:
- **Microsoft.Extensions.Caching.Abstractions**: For caching abstractions.

#### Project References
- **TaxiDigital.Application**: Depends on the application layer for business logic.

## Getting Started

### Prerequisites
- .NET 8 SDK

### Building the Project
To build the project, navigate to the solution directory and run:
### Running the Project
To run the project, navigate to the solution directory and run:
### Testing
To run the tests, navigate to the solution directory and run:
## Contributing
Contributions are welcome! Please open an issue or submit a pull request.

## License
This project is licensed under the MIT License.
