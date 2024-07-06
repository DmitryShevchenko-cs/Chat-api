# Simple Chat Application

## Prerequisites

- .NET 8 SDK
- MS SQL Server
- Visual Studio or any other C# IDE


## Description

This is a simple chat application where users can:
- Create chats.
- Search for existing chats.
- Connect to existing chats.
- Delete chats that they have created.

Once connected to a chat, users can send messages that will be visible to other users in the same chat.


## Tech Stack

- **ASP.NET Core (Web API)**: The backend framework for creating the API.
- **Entity Framework Core (Code First)**: The ORM (Object-Relational Mapper) for interacting with the database using a code-first approach.
- **SignalR**: A library for adding real-time web functionality, allowing for live messaging within chats.
- **.NET 8**: The framework version used for this application.


## Architecture

The application follows a 3-tier architecture:

1. **WEB**: Handles HTTP requests, routing, and interacts directly with clients.
2. **Data access layer**: anages database interactions using Entity Framework Core or another ORM.
3. **Bussiness logic layer**: Implements business rules, validation, and workflows.

**Test**: Includes unit tests, integration tests, and mock data for testing purposes.



