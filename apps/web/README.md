# AssetOps Web

## Architecture

Feature-based:

- `src/features/<feature>/` - components, hooks, and API calls that
  belong to one feature, grouped together.
- `src/routes/` - thin TanStack Router route definitions; compose
  feature components, don't hold feature logic themselves.
- `src/lib/` - cross-cutting: `lib/api` (generated client), `lib/query`
  (query client setup).
- `src/config/`, `src/testing/` - env config, test setup.

`lib/api` is generated via hey-api from a committed `openapi.json` -
reproducible builds, independent of the API's availability.

## Stack

- React + TypeScript (React Compiler enabled)
- Vite
- TanStack Router + TanStack Query
- Tailwind CSS
- Zod
- Biome
- Vitest + Testing Library
- pnpm

## Testing

- Component/unit - Vitest + Testing Library.

## CI/CD

Path-filtered to `apps/web` - only runs when this app changes.

- CI (PR) - Biome check, tests (coverage), SonarCloud quality gate
  (coverage, bugs, vulnerabilities).
- CD (merge to `main`) - same checks, then `vite build`, deployed to
  Azure Static Web Apps via a stored deployment token.
