 Pet Queue System — Full Stack Monorepo

A full-stack appointment & queue management system for pet grooming / pet services businesses.
Built with a modern clean architecture backend and a React + Tailwind frontend.

This project demonstrates production-style layering, JWT authentication, repository & service patterns, and a modern SPA client — all inside a single monorepo.

 Architecture Overview

This repository is structured as a monorepo:

/Api → .NET 9 Minimal API backend
/Client → React + TypeScript + Tailwind frontend
/PetQueue.Tests → Unit tests
PetQueue.sln → Solution file

Backend and frontend are separated but versioned together for easier development and deployment.

 Tech Stack:
 
Backend

.NET 9 Minimal API

C#

Entity Framework Core

SQL Server

JWT Authentication

Repository Pattern

Service Layer (Business Logic)

DTO separation

Middleware exception handling

Frontend

React

TypeScript

Vite

TailwindCSS

Axios

Context-based Auth state

Protected routes

Modal-driven UI flows

Testing

xUnit

Moq

Service-level unit tests

🔐 Core Features
Authentication 

JWT login

Password hashing

Token validation

Protected endpoints

Protected frontend routes

Appointments

Create appointment

Edit appointment

View appointments

Appointment details modal

DTO-based API responses

Date + name filtering support (client side)

Users

Registration

Login

Role support ready (extensible)

 Backend Design

The backend follows layered separation:

Endpoints → API route definitions
Services → Business logic
Repositories → Data access
Models → Domain entities
Dtos → API contracts
Data → DbContext

Principles used

Separation of Concerns

Dependency Injection

DTO boundary protection

Async EF queries

Scoped services

Middleware error handling

 Running the Project
Requirements

.NET 9 SDK

Node.js 18+

SQL Server (local or remote)

▶️ Run Backend

cd Api
dotnet restore
dotnet ef database update
dotnet run

Swagger available at:

/swagger

▶️ Run Frontend

cd Client
npm install
npm run dev

Default:

http://localhost:5173

 Database

EF Core migrations included under:

Api/Migrations

Apply with:

dotnet ef database update

SQL schema file also included:

Api/Database/petqueue.full.sql

 Tests

cd PetQueue.Tests
dotnet test

Coverage includes:

Appointment service logic

Auth service logic

Repository mocking

 Basic Security Measures:

Password hashing

JWT signing

DTO output filtering

No secrets committed

bin / obj / node_modules excluded

Environment configs supported

 Future Enhancements

Role-based authorization

Refresh tokens

Redis caching

Event-driven notifications

Background workers

Email reminders

Docker compose setup

CI/CD pipeline

🎯 Purpose of This Project

This system was built as:

A clean architecture reference

A full-stack interview project

A scalable base for a real product

A microservice-ready foundation

It demonstrates real-world patterns — not demo shortcuts.

👨‍💻 Author

Moshe Mesilati
Full-Stack .NET + React Developer
