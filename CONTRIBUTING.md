# Contributing

This is a personal project, but contributions are welcome.

## Commits

Format: `type(scope): summary`

- **type** — `feat`, `fix`, `docs`, `chore`, `ci`, `refactor`, `test`, `build`, `perf`
- **scope** — `api`, `web`, or omitted for changes that cut across both (repo config, docs, CI)
- **body** — optional. Add one only when the commit carries a reason, trade-off, or constraint that isn't obvious from the title or diff. Skip it otherwise.

Keep commits focused — avoid mixing unrelated changes.

## Branches

`type/scope-short-description`, or `type/short-description` when the change is cross-cutting.

Examples: `feat/api-product-crud`, `docs/contributing-guide`

## Pull requests

- All changes land on `main` through a PR — no direct pushes.
- Squash merge only; the PR title becomes the commit title.
- No required approvals (single maintainer), but CI checks must pass before merging.
