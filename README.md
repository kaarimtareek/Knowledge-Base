# Personal Knowledge Base API

A RESTful API built with .NET 9 and designed to serve as a backend for a personal knowledge management application. This project is a practical exercise in building modern, containerized, and well-architected backend services.

---

## About This Project

The primary purpose of this repository is to document and showcase the development of a robust backend application, following modern software engineering principles. It serves as the foundation for a multi-phase learning plan covering everything from local containerization to cloud deployment and microservices architecture.

## Features

* **CRUD Operations:** Full Create, Read, Update, and Delete operations for core entities.
* **Knowledge Organization:** Manage knowledge `Entries`, categorize them with `Topics`, and label them with `Tags`.
* **Soft Deletion:** Entities are soft-deleted to allow for data recovery, enforced by a global query filter.
* **Modern Identifiers:** Uses ULIDs for primary keys, ensuring database-friendly, sortable, and unique identifiers.
* **API Documentation:** Automatically generated and interactive API documentation via Swagger / OpenAPI.

## Technology Stack

This project is built using a modern, cloud-native-ready technology stack.

| Category          | Technology / Library                                       |
| ----------------- | ---------------------------------------------------------- |
| **Backend** | .NET 9, ASP.NET Core, C#                                   |
| **Architecture** | Clean Architecture, Layered Monolith, MediatR (CQRS)       |
| **Data Access** | Entity Framework Core 8                                    |
| **Database** | PostgreSQL                                                 |
| **API & Schema** | REST, Swagger / OpenAPI                                    |
| **Validation** | FluentValidation                                           |
| **DevOps & Tools**| Docker, Docker Compose                                     |
| **Testing** | xUnit, Moq, FluentAssertions                               |

---

## Getting Started

### Prerequisites

To run this project locally, you will need the following installed on your machine:
* [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Running the Project Locally

This application is fully containerized and can be run with a single command.

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/kaarimtareek/Knowledge-Base
    ```

2.  **Navigate to the root directory:**
    ```bash
    cd Knowledge-Base
    ```

3.  **Launch the application and database:**
    ```bash
    docker-compose up --build
    ```
    This command will:
    * Pull the `postgres` image if you don't have it.
    * Build the .NET API Docker image, which includes running all unit tests.
    * Start both the API and database containers.
    * Apply any pending database migrations automatically on startup.

The API will be available at `http://localhost:6061`.

---

## API Documentation & Usage

Once the application is running, you can access the interactive Swagger UI documentation to explore and test all the available endpoints.

* **Swagger UI URL:** `http://localhost:6061/swagger`

## Running the Tests

To run the unit tests manually without Docker, you can use the .NET CLI from the root directory:

```bash
dotnet test
