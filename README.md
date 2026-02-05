# RSTechTestApplication
This is a test task for the company RSTech.

## Description

A task management Avalonia application built on .NET 8 using PostgreSQL as a database (database deployed in a Docker container).

## Stack

- .NET 8
- Avalonia
- Entity Framework Core 8
- PostgreSQL 18.1
- Docker
  
## Screenshots

### Main view
<img width="1860" height="1052" alt="image" src="https://github.com/user-attachments/assets/49411345-241e-45fe-ae06-9f7bbbfcf3aa" />

### New task to add view
<img width="302" height="152" alt="image" src="https://github.com/user-attachments/assets/3acbf4c5-c7e5-4c3b-990e-06a39cf8c944" />

### Message to user
<img width="392" height="98" alt="image" src="https://github.com/user-attachments/assets/3127346b-2dc4-4ce9-8f70-a6be1ecddc58" />

## Features

- Keyboard control
- Notifying the user about the status of database requests
- CRUD operations with tasks (currently only CRU, because deletion of the task is soft)
- Automatic database initialization
- Docker containerization
- Entity Framework Core migrations

## Usage
To create task press "enter".

In the creation task window, you can use tab navigation (when you move the focus to a button and press the enter key, processing also occurs), and pressing the Escape key allows you to quickly exit the window. 

Use the arrow keys to navigate through the DataGrid.

When selecting a specific task, you can change its status (isCompleted) by pressing the space bar. 
You can also delete (soft delete) the selected task by pressing the delete key.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Quick start

### 1. Clone the repository

```bash
git clone https://github.com/your-username/RSTechTestApplication.git
cd RSTechTestApplication
```

### 2. Launch the docker container

```bash
docker-compose up -d
```

### 3. Launch the app
```bash
dotnet run --project RSTechTestApplication.Presentation
```

The application will automatically:
- Connect to the database
- Create the required schema
- Apply all migrations

## Project structure
```
RSTechTestApplication/
├── RSTechTestApplication.Presentation/    # Avalonia application, Program.cs
├── RSTechTestApplication.Infrastructure/  # DbContext, database initializer, migrations, repositories
├── RSTechTestApplication.Core/            # Entities, contracts
├── docker-compose.yml                     # Docker configuration
└── README.md
```

## Лицензия

MIT
