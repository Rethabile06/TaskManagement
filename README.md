# TaskManagement

Lightweight task management system implemented in .NET.

## Overview

This repository contains a .NET solution for task management. It includes the application projects, unit tests, and supporting code to build, run, and test the system.

## Tech stack

- .NET 10
- C#
- `dotnet` CLI for build/run/test

## Requirements

- .NET 10 SDK (install from https://dotnet.microsoft.com/)

## Getting started

1. Clone the repository:

   `git clone https://github.com/Rethabile06/TaskManagement.git`

2. Open a terminal in the repository root and restore dependencies:

   `dotnet restore`

3. Build the solution:

   `dotnet build`

4. Run a specific project (replace `<project-path>` with the project file or folder):

   `dotnet run --project <project-path>`

### API documentation

Once the application is running, you can explore and test the endpoints using the Scalar API Reference:

    URL: https://localhost:7033/scalar/v1

## Tests

Run unit tests from the solution root:

`dotnet test`

The repository includes unit tests (see `Tests.Unit` project).

## Project structure (common layout)

- `src/` — application projects
- `tests/` or `Tests.Unit/` — unit tests
- `README.md` — this file

Your repository may use a different folder layout; use `dotnet sln list` to see projects included in the solution.

## Contributing

1. Fork the repository and create a feature branch.
2. Implement changes and add tests where applicable.
3. Submit a pull request describing the change.

## License

See the `LICENSE` file in the repository root for license details.