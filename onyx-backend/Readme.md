# Onyx Backend
### Functions and backend for Onyx

#### Repository for all Onyx backend services, exisitng services:
- Budget

#### Currently working on:
- Migration from API to Azure Functions
- Writing Tests
- Documenting the functions
- Small domain refactor
## Environment Variables

To run this project, you must have these environment variables

`CosmosDb__PrimaryKey` to connect with database


## Run Locally
Temporary solution, won't be supported after migration to Azure Functions

### Using git

#### Clone the project

```bash
    git clone https://github.com/dbrdak/onyx-app
```

#### Go to the project directory

```bash
    cd <ProjectPath>/onyx-backend
```

#### Start the server with dotnet CLI

```bash
    dotnet run --project .\budget\src\Budget.API\Budget.API.csproj
```

### Using docker

#### Download file for running API with docker
[run-scipt.ps1](https://github.com/DBrdak/onyx-app/blob/penny-migration/onyx-backend/run-script.ps1)

#### Run file
You'll need to login to docker.io and pass the Azure CosmosDB instance key which have 'budget' database created
## Authors

- [@dbrdak](https://www.github.com/dbrdak)

