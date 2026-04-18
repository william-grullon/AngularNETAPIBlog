# Angular .NET API Blog

A full-stack blog application built with an Angular frontend and an ASP.NET Core Web API backend. Blog posts are stored as Markdown files on disk instead of in a relational database, which keeps the content easy to inspect and edit.

## Overview

- Frontend: Angular 20 + TypeScript
- Frontend build system: Angular application builder
- Backend: ASP.NET Core Web API on .NET 8
- Content storage: Markdown files under the API project
- API docs: Swagger/OpenAPI
- Dev setup: Angular CLI proxy forwards `/api` calls to the backend

## Repository Layout

```
AngularNETAPIBlog/
├── API/
│   └── AngularNETAPIBlog/
│       └── AngularNETAPIBlog/
│           ├── Controllers/
│           ├── Models/
│           ├── Repositories/
│           ├── Properties/
│           └── content/
│               ├── posts/
│               └── categories/
└── UI/
    └── angularNetBlog/
        └── src/
            ├── app/
            │   ├── core/
            │   └── features/
            ├── assets/
            └── environments/
```

## Prerequisites

- .NET 8 SDK
- Node.js and npm

## Run The API

From the API project directory:

```bash
cd API/AngularNETAPIBlog/AngularNETAPIBlog
dotnet restore
dotnet run
```

By default, the app uses:

- `http://localhost:5201`
- `https://localhost:7061`

Swagger is available at `/swagger` when the API is running.

## Run The UI

From the Angular project directory:

```bash
cd UI/angularNetBlog
npm install
npm start
```

The frontend runs at:

- `http://localhost:4200`

During development, the Angular proxy configuration forwards `/api` requests to the API at `http://localhost:5201`.
The UI now uses Angular's newer application builder, so `npm start` and `npm run build` both use the migrated build system.

## What It Does

- View blog posts and categories
- Create, edit, and delete posts
- Manage categories
- Store and render Markdown-based post content
- Expose a REST API for the Angular app

## Notes

- Blog content lives in the API project under `content/`, so changes there are part of the application data.
- The UI uses the Angular dev server proxy instead of hard-coding a backend URL in development.
- If you want to run both apps together, start the API first and then the UI.

## Related Project Readme

The Angular app also has its own project-level README in [`UI/angularNetBlog/README.md`](UI/angularNetBlog/README.md).
