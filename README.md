This is a graduation project developed using **ASP.NET Core** with **Clean Architecture** for managing a clothing brand. The system includes features for ready-made garment sales, custom clothing design requests, and paid sewing courses. This repository contains the backend and frontend code (with Angular) along with project documentation.

## Features

1. **Product Management**: 
   - Showcase ready-made garments with prices and discounts.
   - Orders sent through WhatsApp for confirmation or processed online.

2. **Custom Clothing Requests**: 
   - Customers can provide specific measurements and designs with image uploads.
   - Option for customers to supply materials or let the company handle everything.
   - A deposit system to confirm serious orders.

3. **Sewing Courses**: 
   - Paid sewing courses with restricted access until payment confirmation.

4. **Payments**:
   - Integrated online payments for both product purchases and course enrollment.

5. **User Roles**: 
   - Role-based access for customers, admins, and staff.

## Project Architecture: Clean Architecture

The project follows the **Clean Architecture** pattern to ensure a scalable, maintainable, and testable system.

- **Core**:
  - Contains the business logic and domain entities.
  - Application layer with services and interfaces for business operations.
  
- **Infrastructure**:
  - Database access using **Entity Framework Core**.
  - Payment and email services integration.
  
- **Presentation**:
  - **Angular frontend** for a responsive user interface.
  - API Controllers that expose endpoints to the frontend.

- **Tests**:
  - Unit and integration tests for core functionalities.

### Folder Structure

.
├── src
│   ├── Core
│   │   ├── Entities
│   │   ├── Interfaces
│   │   ├── Services
│   ├── Infrastructure
│   │   ├── Data
│   │   ├── PaymentService
│   │   ├── Repositories
│   ├── WebAPI
│   │   ├── Controllers
│   │   ├── ViewModels
│   ├── Frontend (Angular)
│   │   ├── src
│   │   ├── app
│   │   ├── services
├── tests
│   ├── Core.Tests
│   ├── IntegrationTests
└── README.md
## Technology Stack

- **Backend**: 
  - ASP.NET Core 7.0
  - Entity Framework Core
  - SQL Server
  - JWT for Authentication
  - Stripe for Payments

- **Frontend**:
  - Angular 18
  - Bootstrap 5

- **Testing**:
  - xUnit
  - Moq for unit testing

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/en/) (for Angular)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Stripe Account](https://stripe.com) for payment integration.

### Installation

1. **Clone the repository**:
bash
   git clone https://github.com/username/repository-name.git
   
2. **Backend Setup**:
   - Navigate to the backend project folder:
bash
     cd src/WebAPI
     
- Restore NuGet packages:
bash
     dotnet restore
     
- Update the database:
bash
     dotnet ef database update
     
- Run the application:
bash
     dotnet run
     
3. **Frontend Setup**:
   - Navigate to the Angular project folder:
bash
     cd src/Frontend
     
- Install Node.js packages:
bash
     npm install
     
- Run the Angular app:
bash
     ng serve
     
4. **Run Tests**:
bash
   dotnet test
   
### Git Workflow

1. **Clone the repository**:
bash
   git clone https://github.com/username/repository-name.git
   
2. **Create a feature branch**:
bash
   git checkout -b feature-branch-name
   
3. **Commit your changes**:
bash
   git add .
   git commit -m "Add some feature"
   
4. **Push to the branch**:
bash
   git push origin feature-branch-name
   
5. **Open a Pull Request** on GitHub to merge your changes.

## API Endpoints

### Products
- **GET /api/products**: Get all products
- **GET /api/products/{id}**: Get a product by ID
- **POST /api/products**: Add a new product
- **PUT /api/products/{id}**: Update a product
- **DELETE /api/products/{id}**: Delete a product

### Custom Orders
- **POST /api/orders/custom**: Place a custom order

### Courses
- **GET /api/courses**: Get all courses
- **POST /api/courses/enroll**: Enroll in a course

### Payments
- **POST /api/payments/checkout**: Checkout and process payment

## Contributing

Please follow these guidelines for contributing to the project:

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push your changes to the feature branch.
5. Submit a pull request.

## License

This project is licensed under the MIT License.
