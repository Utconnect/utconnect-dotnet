# Utconnect-dotnet
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Utconnect_utconnect-dotnet&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Utconnect_utconnect-dotnet)

Utconnect-dotnet is a .NET project that includes a set of subprojects developed for the University of Transport and Communication. Built by many alumni, it serves various functionalities related to identity management, 
authentication, and the examination schedule management. The project is modular, encompassing domain logic, infrastructure, and presentation layers, and supports Docker for containerized deployment.

## Features

- **Identity Management**: Manage user identities and authentication.
- **Examination Schedule Management**: Handle examination schedules efficiently.
- **Modular Architecture**: Separate modules for domain, infrastructure, and presentation layers.
- **Docker Support**: Includes Docker configuration for containerized deployment.
- **Security**: Implements robust security measures and data protection.

## Project Structure
- `Auth`: Common authentication services and utilities.
- `Oidc`: OpenID Connect implementation.
- `Esm`: Examination schedule management
- `Home`: Contains the home and landing page components.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker (optional, for containerization)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Utconnect/utconnect-dotnet.git
    ```
2. Navigate to the project directory:
    ```sh
    cd utconnect-dotnet
    ```
3. Restore the dependencies:
    ```sh
    dotnet restore
    ```

### Running the Application

To run the application, use the following command:
```sh
dotnet run
```

If using Docker, build and run the containers:
```sh
docker-compose up --build
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request with your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

For more information, please visit the [repository](https://github.com/Utconnect/utconnect-dotnet) or contact the maintainers.

---

Feel free to make further adjustments to fit your specific needs.
