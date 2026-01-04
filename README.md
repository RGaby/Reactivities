# Reactivities – Modular Monolith (.NET + React)

## 📌 Overview
Reactivities is a full-stack application built as a **modular monolith**, using a .NET backend and a React + TypeScript frontend.

The project demonstrates **enterprise-level architecture patterns**, while keeping the system easy to understand, test, and extend.

It is suitable both as an advanced learning project and as a **portfolio project for .NET backend / full-stack roles**.

---

## 🏗 Architecture

### Backend (.NET)
The backend follows Clean Architecture principles:
```
API/ → Controllers, Middleware, Startup
Application/ → CQRS (Commands & Queries), DTOs, Validators
Domain/ → Entities, business rules
Persistence/ → EF Core, DbContext, migrations
Infrastructure/ → External services (Identity, Email, etc.)
```

**Key principles:**
- Thin controllers
- Business logic isolated in the Application layer
- Domain layer free of framework dependencies
- Data access separated from business logic

---

### Frontend (React + TypeScript)

The frontend lives in:
client-app/


Features:
- React + TypeScript
- Communicates with the backend via HTTP REST APIs
- Fully decoupled from backend implementation

> The frontend can be moved to a separate repository without major refactoring.

---

## 🔁 Request Flow
```
React Client
↓ HTTP (REST)
API Controllers
↓
MediatR (Commands / Queries)
↓
Application → Domain → Persistence
```

---

## ▶️ Running the project

### Requirements
- .NET SDK
- Node.js + npm
- Database configured via EF Core

### Backend
bash
dotnet restore
cd API
dotnet run

### Frontend

cd client-app
npm install
npm start

If you wish for the photo upload to work create a file called appsettings.json in the Reactivities/API folder and copy/paste the following configuration.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "CloudinarySettings": {
    "CloudName": "REPLACEME",
    "ApiKey": "REPLACEME",
    "ApiSecret": "REPLACEME"
  },
  "AllowedHosts": "*"
}
```
Create an account (free of charge, no credit card required) at https://cloudinary.com and then replace the Cloudinary keys in the appsettings.json file with your own cloudinary keys.

You can then browse to the app on https://localhost:3000 and login with either of the test users:

email: bob@test.com or tom@test.com or jane@test.com

password: Password1!

## 🎯 Why this project?

Demonstrates a well-structured enterprise monolith

Clear separation of concerns

Easy to evolve toward microservices

Ideal for technical interviews and code reviews

## 🚀 Possible Improvements

Extended unit and integration tests

Architecture diagrams

## 🧠 Final Notes

This repository is a monorepo, but not a tightly coupled monolith.
The backend and frontend are logically separated and communicate strictly through APIs, following modern best practices.
