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

1. **Presentation Tier**: This is where the user interface components reside. It communicates with the application tier to process user input.
2. **Application Tier**: This is where the business logic is implemented. It processes data between the presentation tier and the data tier.
3. **Data Tier**: This is where the database management happens, storing all the chat data.



