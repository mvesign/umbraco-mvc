# Umbraco MVC

A training/demo project that showcases a layered Umbraco CMS setup using MVC controllers, custom routing, member-driven recipe submission, and in-memory upvoting.

## Overview

This repository demonstrates how to structure an Umbraco solution across multiple projects while keeping website concerns, business logic, and generated models separated.

It features:

- **Umbraco CMS 17.1.0**
- **uSync 17.0.2** for content/settings synchronization
- **.NET 10.0** with central package management
- **Layered architecture** split into Core, Models, Logic, and Website projects

## Features

### Website and Content

- Render controllers for recipe pages and recipe area pages
- Surface controller for member recipe submission flow
- API controller for authenticated member upvotes
- ModelsBuilder source code generation into a dedicated models project

### Custom Umbraco Extensions

- **Custom URL provider** (`HomeUrlProvider`) for home page URL behavior
- **Custom content finder** (`HomeContentFinder`) for resolving the site root to a home node
- **Notification handler** to move member-published recipes into the public recipe area

### Business Logic Services

- `IRecipeUploadService` / `RecipeUploadService` for programmatic content + media creation and publishing
- `IRecipeUpvoteService` / `RecipeUpvoteService` for upvote state management
- In-memory upvote tracking with duplicate upvote protection per member/recipe combination

## Solution Structure

- `UmbracoMvc.Core` - shared primitives and cross-cutting code
- `UmbracoMvc.Models` - generated Umbraco models, enums, exceptions, and submit/view models
- `UmbracoMvc.Logic` - composers, routing customizations, notification handlers, domain services
- `UmbracoMvc.Website` - ASP.NET Core host, Umbraco bootstrapping, controllers, views, static assets, and uSync files

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQL LocalDB
- A code editor such as Visual Studio, VS Code, or Rider

### Initial Setup

1. **Clone the repository**
	```bash
	git clone https://github.com/mvesign/umbraco-mvc.git
	cd umbraco-mvc
	```

2. **Restore dependencies**
	```bash
	dotnet restore
	```

3. **Configure local settings**
	- Open `UmbracoMvc.Website/appsettings.Local.json`.
	- Update the `ConnectionStrings:umbracoDbDSN` value for your local SQL Server/LocalDB instance.
	- Optionally update unattended install credentials.

4. **Prepare the database**
	- Create an empty database for this project (or point to an existing local development database).

5. **Run the website project**
	```bash
	cd UmbracoMvc.Website
	dotnet run
	```

6. **Open the site**
	- Navigate to the URL shown in the console (for example `https://localhost:xxxx`).
	- If needed, complete Umbraco setup/login at `/umbraco`.

## Development Configuration

- `appsettings.Local.json` is loaded in `DEBUG` mode from `Program.cs`.
- Umbraco ModelsBuilder is configured for `SourceCodeAuto` and writes generated models to `UmbracoMvc.Models/Content/Generated`.
- uSync is configured under `UmbracoMvc.Website/uSync`.

## License

See the [LICENSE](LICENSE) file for details.