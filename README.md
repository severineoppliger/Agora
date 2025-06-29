# Introduction 
**Agora** is a community web platform designed for the university environment, enabling students, staff, and alumni 
to exchange services. Users offer their time and skills in return for a virtual currency called Kairos. 
Kairos is strictly based on the time invested by users, regardless of the type of service provided,
ensuring fairness across the platform. Upon registration, each user receives an initial allocation of Kairos 
to start trading.

The backend is built with **ASP.NET Core 9**, follows a clean and layered architecture, and provides a fully documented
**REST API** secured with cookie authentication. It communicates with a MariaDB database containerized in Docker.
It uses **Entity Framework Core** as ORM and **ASP.NET Core Identity** as account management system.

# Architecture
The backend of `Agora` solution is structured in three projects in a clean-like architecture as follows:
* `Agora.Core`: Microsoft.NET.Sdk project, a class library, which contains all the business logic: 
Entities, domain services, business validation, interfaces, enum, constants, etc.
* `Agora.Infrastructure`: Microsoft.NET.Sdk project, a class library,
responsible for the access to external ressources like the DB.
It contains the entity configurations, the DbContext and repositories.
  * It has`Agora.Core` as project reference.
* `Agora.API`: Microsoft.NET.Sdk.Web project, an executable which is a RESTful API.
It contains the controllers, input validation class, DTOs and their map profiles to entities (and vice-versa)
using AutoMapper, ApiQueryParameters, etc.
  * It has`Agora.Infrastructure` as project reference.

# Getting Started
## Prerequisites
Install:
- [Docker Desktop](https://docs.docker.com/desktop/)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download) to be able to run the application locally, without Docker
- An IDE to run C# code like [Rider](https://www.jetbrains.com/rider/download) or [Visual Studio Code](https://code.visualstudio.com/download) with an appropriate plugin.
- [Postman](https://www.postman.com/) to test the API.

No MariaDB installation is needed since the DB is automatically created and stored in the Docker container.

## Setup
1. Clone the repository: `git clone https://severineoppliger@dev.azure.com/severineoppliger/Agora/_git/Agora`
2. Navigate to the solution: `cd Agora`
3. 

# Frontend
Angular version 20.0.x
1. Install Node.js version 22.16.0 using NVM or from http://nodejs.org/
2. Install Angular CLI using npm (Node.js package manager): `npm install -g @angular/cli`
3. Run the client: From Agora solution
   * `cd client`
   * `ng serve`

# Test 
You can test the API REST using the Postman collection [Agora API.postman_collection.json](./Agora API.postman_collection.json).
Upload it in the Postman UI.

# License
Please refer to the file [LICENSE.txt](./LICENSE.txt).