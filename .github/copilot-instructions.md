This is a repository containing both a VueJS based front-end and a C# powered Azure Functions based back-end.
It is primarily responsible for ingesting metered usage for GitHub and recording that usage.

Please follow these guidelines when contributing:

## Code Standards

### Required Before Each Commit

### Development Flow


## Repository Structure
- `.github/workflows`: Containing all github actions workflows, amongst others our CI flows
- `backend/`: Contains all the back-end code based on Azure Functions in C#
- `frontend/`: Contains all the front-end code based on VueJS
- `datacontract/`: The specifications of the data contract. This directory will also include a script to generate sample data.
- `docs/`: Documentation
- `readme.md` : General overview of this repo that helps guide newjoiners around. 

## Key Guidelines
1. Follow Go best practices and idiomatic patterns
2. Maintain existing code structure and organization
3. Use dependency injection patterns where appropriate
4. Write unit tests for new functionality. Use table-driven unit tests when possible. Ensure the unit tests are part of the CI flow.
5. Document public APIs and complex logic. Suggest changes to the `docs/` folder when appropriate
6. Ensure that both the backend and frontend code have a seperate CI flow that is triggered when changes happen in their specific directory and that builds a container based artifact
7. This data contract will serve as a data contract between API (backend) & consumer (frontend). It will be used between the backend APIs and the frontend as a way to provide consistency. For example, but not limited to, the consistency toward the exact naming of objects.
