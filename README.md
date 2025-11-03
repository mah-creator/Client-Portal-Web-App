# Client Portal Web App

This repository contains a full stack Client Portal application built with:

- **Backend:** ASP.NET Core (API)
- **Frontend:** React Web App
- **Payments:** Stripe (Checkout + Webhooks)

The system supports multiple roles (Admin, Freelancer, Customer), Project management, Task sharing, and Paid features via Stripe billing.

---

## Technical Overview

This platform provides an online portal where customers and freelancers interact around projects.

**Core Features Include:**

| Area | Details |
|------|---------|
| Authentication | JWT based authentication & role-based authorization |
| Roles | Customer, Freelancer, Admin |
| Projects / Tasks | Create project, assign tasks, update status, exchange files |
| Payments | Stripe Checkout integration |
| Stripe Webhooks | Payment confirmation is processed server-side |

---

## Running the Projects

### Backend (Server API)

```sh
cd api
dotnet restore
dotnet ef database update   # apply migrations (if any)
dotnet run
```


#### About dotnet Launch Profiles

The backend uses `.NET Launch Profiles` (inside `Properties/launchSettings.json`) to define which URL the frontend application runs on during development.

You can switch profiles in VS / VS Code:

- Two profiles: **LiveDev**, **LocalDev**
- Each profile defines:
  - `ASPNETCORE_REACT_APP_URL` environment variable (the URL of the react app)

The backend enables CORS for the domain specified by the variable.

### Frontend (Client Web App)

```sh
cd app
npm i
npm run dev
```

Make sure the `API_BASE_URL` variable (in `src/lib/api-client.ts`) to the URL of the backend.

---

## Stripe Setup

Stripe secrets are stored using **.NET Secret Manager**, not in config files.

### 1) Set Stripe Secrets

Run these **inside the backend project folder** (where `.csproj` exists):

1) Enable secret storage

    ```.NET CLI
    dotnet user-secrets init
    ```

2) Set the Stripe `SecretKey` secret

    ``` .NET CLI
    dotnet user-secrets set "Stripe:SecretKey" "your-stripe-secret-key"
    ```

**That's it, you're ready to go!**

### 2) Run Stripe CLI to forward webhooks

```
stripe listen --forward-to <<API base URL>>/api/stripe/webhook
```

This command will show something like:

```
✔ Ready! Your webhook signing secret is whsec_ABC123
```

Copy that “whsec_…” and set it using user-secrets:

```sh
dotnet user-secrets set "Stripe:WebhookSecret" "whsec_ABC123"
```

### 3) To verify everything is set correctly:

```sh
dotnet user-secrets list
```

---

## Test Accounts

Use these demo accounts for testing:

| Role | Email | Password |
|------|-------|----------|
| Customer | customer1@customer.com | 123 |
| Customer | a.agha@gmail.com | 123456 |
| Customer | n.lubbad@gmail.com | 123456 |
| Freelancer | freelancer@local.com | 123 |
| Freelancer | mah.tah@gmail.com | 123456 |
| Admin | admin@local.com | 123 |

> These are local development accounts only.
