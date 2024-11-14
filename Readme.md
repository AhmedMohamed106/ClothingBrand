# Sewing Atelier System - ITI Graduation Project

Sewing Atelier is an all-in-one web application for the fashion industry, merging e-commerce, custom order management, and educational courses. It allows users to purchase ready-made clothing, request custom-tailored garments, and enroll in courses.

---

## **Table of Contents**

- [Project Overview](#project-overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

---

## **Project Overview**

Sewing Atelier system offers a seamless experience for:
- Shopping ready-made clothing items.
- Placing custom clothing orders with specific measurements and designs.
- Enrolling in fashion and sewing courses.

Ideal for fashion retailers, custom designers, and students, CoutureConnect combines e-commerce, custom-tailoring, and educational resources in one solution.

---

## **Features**

1. **E-commerce Platform for Ready-made Clothing**
   - Product catalog with categories, filters, and search capabilities.
   - Product pages with detailed descriptions, pricing, and discounts.
   - Shopping cart and secure checkout process.
  
2. **Custom Order Management**
   - Custom orders with detailed measurement input.
   - Design reference image upload.
   - Track order status through completion.

3. **Educational Courses**
   - Fashion and sewing courses catalog.
   - Course enrollment and tracking progress.
   - Certification upon course completion.

4. **User Management and Authentication**
   - Role-based access for Users and Admins.
   - Secure login, registration, and profile management.

---

## **Project Structure**

```plaintext
CoutureConnect
├── ClothingBrand.Application         # Application Layer - Business logic, DTOs, Services, Interfaces
│   ├── Common
│   ├── Services                      # Service implementations (e.g., OrderService, ProductService)
│   ├── DTO                           # Data Transfer Objects (DTOs) for API communication
│   └── Interfaces                    # Interface definitions for dependency injection
│
├── ClothingBrand.Domain              # Domain Layer - Core entities, domain logic, shared interfaces
│   ├── Models                        # Entity models (e.g., Product, CustomOrder, Course)
│   └── Interfaces                    # Repository interfaces for data access abstraction
│
├── ClothingBrand.Infrastructure      # Infrastructure Layer - Data access, repository implementations
│   ├── Repositories                  # Concrete repository implementations (e.g., ProductRepository)
│   └── Data                          # Database context and migrations
│
├── ClothingBrand.WebApi              # API Layer - ASP.NET Core Web API for RESTful services
│   ├── Controllers                   # Controllers for API endpoints
│   └── Startup.cs                    # API configuration and dependency injection
│
├── ClothingBrand.Web                 # Angular Frontend - User interface for interacting with the application
│   ├── src
│       ├── app                       # Angular components, services, and routing
│       └── assets                    # Static assets (e.g., images, CSS)
│
└── Tests                             # Unit and integration tests
    ├── Application.Tests             # Tests for services and business logic
    └── WebApi.Tests                  # Tests for API endpoints


```

## **Technologies Used**

### Backend
- **ASP.NET Core** - API framework.
- **Entity Framework Core** - ORM.
- **SQL Server** - Database.
- **Stripe API** - Payment processing.

### Frontend
- **Angular 18** - SPA framework.
- **Bootstrap** - Responsive CSS framework.
- **TypeScript** - JavaScript with static typing for Angular.

### Additional Tools
- **AutoMapper** - DTO mapping.
- **JWT Authentication** - API security.
- **XUnit** - Testing framework.

---

## **Installation**

### Prerequisites

- .NET SDK 7.0+
- Node.js 16+ and npm
- SQL Server
- Angular CLI

### Backend Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/ahmedmohamed106//ClothingBrand.git
   cd ClothingBrand


### Frontend Setup    

#### check link of repository 
```bash
https://github.com/Enas20Hussein/ClothesBrand
   

