# PawLogger

> A self-hosted, open-source pet health and care tracking application.

**PawLogger** is a pet-health-focused adaptation of **[LubeLogger](https://github.com/hargata/lubelog)**.  
It builds on the upstream project’s foundation and redirects the user-facing experience toward **pet profiles, health history, medications, vaccinations, vet visits, reminders, expenses, and exportable summaries**.

> **Attribution:** PawLogger is based on LubeLogger, and that upstream work should remain clearly credited and respected. This project is an adaptation/refactor of that codebase, not a greenfield rewrite.

---

## Overview

PawLogger is designed to keep a pet’s health and care history in one place.

It supports tracking and managing:

- pet profiles and lifecycle status
- health timeline/history
- vaccinations, medications, vet visits, and licensing
- reminders and follow-ups
- pet-related expenses
- weight history and trends
- allergy and quick health notes
- attachments and exportable summaries

The app now presents as a **pet-health-first** experience, while some upstream-era internal naming and compatibility paths may still remain during migration.

---

## Features

### Pet Profiles
Track one or more pets and store profile details such as:

- name
- species
- breed
- sex
- date of birth / estimated age
- color / markings
- microchip
- spayed / neutered status
- photo
- notes

Supported pet lifecycle statuses:

- **Active**
- **Archived**
- **Rehomed**
- **Deceased**

---

### Health Timeline
PawLogger uses a centralized **HealthRecord** timeline/history model for pet care tracking.

Supported categories include:

- Vet Visit
- Vaccination
- Medication
- Weight Check
- Allergy / Reaction
- Preventive Care
- Behavioral Note
- Miscellaneous Care

Health records can include notes, tags, dates, costs, and attachments where applicable.

---

### Specialized Health Records
Structured record flows exist for:

- **Vaccinations**
- **Medications**
- **Vet Visits**
- **Licensing**

These can link/project into the main health timeline so the pet record remains useful as a single longitudinal history.

---

### Reminders
- centralized reminder flow
- pet-care-oriented reminder handling
- follow-up and due-date support where applicable

---

### Pet Expenses
Track pet-related expenses such as:

- vet care
- medications
- vaccinations
- grooming
- food
- supplies
- licensing
- insurance
- boarding / daycare
- training
- preventive care
- other costs

---

### Weight Tracking
- structured **Weight Check** entries
- weight history support
- weight trend support
- user-configurable preferred weight unit

---

### Allergy Tracking
Track allergy and reaction history through structured health records.

---

### Quick Health Notes
Fast-entry health and care notes for day-to-day observations.

---

### Pet Health Summary / PDF Export
- consolidated pet summary view
- exportable PDF summary
- intended for sharing, saving, or printing as a cleaner pet record summary

---

### Multi-Pet Support
Track multiple pets in the same app instance.

---

### Attachments
Store file/document attachments in relevant record flows.

---

### Compatibility Support
- user-facing pet routes prefer `/animals`
- legacy `/Vehicle` routes may still remain functional for compatibility
- some upstream-era internal identifiers may still remain in code and storage

---

## Main Workflows

### 1. My Pets
View and manage pets, including active and inactive statuses.

### 2. Pet Profile
Edit pet details, status, weight, and general information.

### 3. Health Records
Use the health timeline as the main source of care history.

### 4. Specialized Records
Create and manage:

- vaccinations
- medications
- vet visits
- licensing

### 5. Reminders
Track upcoming care, follow-ups, and recurring needs.

### 6. Expenses
Track pet-related costs and review totals/history.

### 7. Pet Health Summary
Generate a readable summary of a pet’s record and export it as a PDF.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10 / ASP.NET Core MVC |
| Default database | LiteDB |
| Optional database | PostgreSQL |
| UI framework | Bootstrap |
| Charts | Chart.js |
| Date picker | Bootstrap Datepicker |
| Alerts / modals | SweetAlert2 |
| CSV processing | CsvHelper |
| Markdown rendering | Drawdown |
| Email | MailKit |

---

## Getting Started

### Prerequisites

You can run PawLogger using either Docker or the .NET SDK.

- [Docker](https://www.docker.com/)
- or [.NET 10 SDK](https://dotnet.microsoft.com/)

---

## Running with Docker

### Default (LiteDB)

```bash
docker compose up -d
```

This uses the default `docker-compose.yml` and runs the app with LiteDB.

### PostgreSQL

```bash
docker compose -f docker-compose.postgresql.yml up -d
```

This uses PostgreSQL instead of LiteDB.

### Other compose variants

The repository may also include additional compose variants such as:

- `docker-compose.dev.yml`
- `docker-compose.traefik.yml`

Use the one that matches your environment and deployment style.

---

## Running Locally

Restore packages and run the application:

```bash
dotnet restore
dotnet run
```

Build explicitly:

```bash
dotnet build -c Release
```

Clean build:

```bash
dotnet clean
dotnet restore
dotnet build -c Release
```

---

## Configuration

Configuration is handled through:

- `appsettings.json`
- environment variables
- persisted user/server settings

If PostgreSQL is enabled, configure the appropriate connection string for your deployment.

If your current deployment uses a `POSTGRES_CONNECTION` environment variable, continue using that convention unless your local configuration differs.

---

## Routing

PawLogger now prefers pet-friendly user-facing routing such as:

- `/animals/...`

Legacy upstream-style routes may still remain available for compatibility, including older `/Vehicle/...` paths.

---

## Compatibility Notes

PawLogger is a pet-health-focused adaptation of LubeLogger, but some compatibility layers remain.

Examples include:

- some internal names from the original project may still remain in code or storage
- some legacy tabs or compatibility paths may still exist in the UI or backend
- the internal project/assembly naming may still reflect the upstream codebase in some places
- some older report or compatibility paths may still exist even if the primary pet-facing workflow now uses PawLogger terminology

These compatibility layers exist to reduce migration risk while the application continues to move away from vehicle-era internals.

---

## Known Limitations

- some legacy LubeLogger-era internal naming may still remain
- some compatibility/legacy tabs may still exist
- certain old vehicle-era functionality may still be present under compatibility paths
- the app is pet-focused in user-facing workflows, but not every internal identifier has necessarily been fully renamed
- some legacy report/export paths may still exist alongside newer pet-summary flows

---

## Development Notes

When making future changes, the safest approach is:

1. preserve user-facing pet terminology
2. keep upstream attribution intact
3. avoid reckless renaming of compatibility-critical fields and routes
4. prefer additive migration over destructive rewrites

Examples of compatibility-critical items that may still exist internally include upstream-era naming like `Vehicle` / `VehicleId`.

---

## Upstream Attribution

PawLogger is based on the work done in **[LubeLogger](https://github.com/hargata/lubelog)** by **hargata**.

This repository should continue to acknowledge that upstream foundation clearly and respectfully.

PawLogger does **not** claim to be built from scratch. It is an adaptation/refactor of the upstream project for a different domain: pet health and care tracking.

---

## License

This repository is licensed under the **MIT License**.

See [LICENSE](LICENSE) for details.

Because PawLogger is based on LubeLogger, upstream attribution and license obligations should remain preserved.

---

## Credits

### Upstream Project
- [LubeLogger](https://github.com/hargata/lubelog)

### Core Dependencies
- [Bootstrap](https://github.com/twbs/bootstrap)
- [LiteDB](https://github.com/litedb-org/LiteDB)
- [Npgsql](https://github.com/npgsql/npgsql)
- [Bootstrap Datepicker](https://github.com/uxsolutions/bootstrap-datepicker)
- [SweetAlert2](https://github.com/sweetalert2/sweetalert2)
- [CsvHelper](https://github.com/JoshClose/CsvHelper)
- [Chart.js](https://github.com/chartjs/Chart.js)
- [Drawdown](https://github.com/adamvleggett/drawdown)
- [MailKit](https://github.com/jstedfast/MailKit)

---

## Roadmap

Potential future improvements may include:

- further reduction of legacy internal naming
- additional cleanup of compatibility-era tabs and report paths
- continued UX polish for pet-health-first workflows
- improved export/report options
- additional documentation and deployment guidance

This section reflects possible future work only. It does not imply those items are complete.

---

## Summary

PawLogger is a pet-health-focused adaptation of LubeLogger that now supports:

- pet profiles
- health timeline/history
- vaccinations
- medications
- vet visits
- licensing
- reminders
- pet expenses
- weight tracking
- allergy tracking
- quick health notes
- Pet Health Summary / PDF export
- multi-pet support
- attachments

It is intended to feel like a pet care application in normal user-facing workflows while continuing to respect and preserve the upstream foundation it was built from.
