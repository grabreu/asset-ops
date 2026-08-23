# AssetOps

## Repository

This is a monorepo containing:

- `apps/api` - ASP.NET Core API
- `apps/web` - React SPA

Read the relevant `README.md` before making changes to an application.

## General Rules

- Keep changes scoped to the requested application.
- Do not modify generated files manually.
- Prefer existing patterns over introducing new abstractions.
- Do not add dependencies unless they are necessary.
- Run the relevant formatting, linting, build, and tests after changes.
- Do not change CI/CD or infrastructure configuration unless explicitly required.
- Do not claim a validation command passed unless it was actually run.

## API

Before making changes to `apps/api`, read `apps/api/README.md`.

- Follow the existing Clean Architecture boundaries.
- Domain logic belongs in `Domain`.
- Use cases belong in `Application`.
- Persistence concerns belong in `Infrastructure`.
- HTTP concerns belong in `Api`.
- Preserve domain invariants.
- Integration tests use `WebApplicationFactory`.
- Unit tests should not depend on infrastructure.

## Web

Before making changes to `apps/web`, read `apps/web/README.md`.

- Follow the feature-based architecture.
- Keep TanStack Router route definitions thin.
- Feature logic belongs under `src/features`.
- Cross-cutting code belongs under `src/lib`.
- Do not manually modify generated API client files.
- The OpenAPI client is generated from the committed `openapi.json`.
- Use Biome for formatting and linting.
- Use Vitest and Testing Library for tests.

## Generated Files

The web API client is generated from the committed `openapi.json`.

When the API contract changes:

1. Update the API.
2. Update the committed `openapi.json`.
3. Regenerate the web API client.
4. Verify the generated changes.

## Validation

For API changes, run the relevant:

- `dotnet build`
- unit tests
- integration tests

For web changes, run the relevant:

- `pnpm check`
- tests
- `pnpm build`

## Git

Follow the conventions defined in `CONTRIBUTING.md` for branches, commits, and pull requests.

- Do not create or switch branches unless explicitly requested by the user.
- Do not create commits unless explicitly requested by the user.
- Do not push changes unless explicitly requested by the user.
- Keep commits focused on the requested change.

## Documentation

Follow the documentation conventions below when creating or updating project documentation.

### Audience

Write for a recruiter or another developer briefly evaluating the repository.

Keep documentation concise and skimmable. Do not write onboarding tutorials unless explicitly requested.

### README Structure

- Root `README.md` - product overview, use cases, apps, infrastructure, and one flagship end-to-end use case.
- `apps/api/README.md` - architecture, domain model, stack, testing, and CI/CD.
- `apps/web/README.md` - architecture, stack, testing, and CI/CD.
- `docs/adr/` - architectural decisions, when applicable.

Keep the existing README structure unless there is a clear reason to change it.

### Content Rules

- State facts concisely. Avoid unnecessary explanations or trailing rationale.
- Do not document information that is already obvious from the repository structure or configuration.
- Document decisions, constraints, and external facts that are not obvious from browsing the repository.
- Do not invent product requirements, architectural decisions, or future direction.
- Document engineering-quality claims only after they are implemented and verified.
- Use proper Markdown headings (`##`, `###`, etc.), not bold text as headings.
- Keep use cases as a flat list. Do not introduce prioritization tiers.
- Write ADRs when the decision is made, not speculatively beforehand.

### Use Cases

The use case list should remain a concise index.

Choose one flagship use case with a meaningful invariant and document it end to end with a sequence diagram. Keep other use cases as one-line entries.

### Diagrams

Use diagrams only when they communicate a meaningful architectural or domain rule.

- Use a state diagram for a real state machine with enough states or transitions that prose becomes difficult to follow.
- Use a sequence diagram when a flow crosses architectural boundaries and something meaningful happens, such as an invariant being checked or state being changed.
- Do not add diagrams for simple CRUD operations.
