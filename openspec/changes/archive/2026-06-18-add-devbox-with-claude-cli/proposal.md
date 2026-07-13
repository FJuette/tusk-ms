## Why

Contributors and AI-assisted workflows need a reproducible, hermetic development environment. Adding a devbox configuration ensures every developer gets the same toolchain (dotnet, Node.js, Claude CLI) without manual setup.

## What Changes

- Add `devbox.json` at the repo root declaring all required packages (dotnet SDK, Node.js, Claude CLI via npm)
- Add `devbox.lock` (generated automatically on first `devbox install`)
- Add a `.devcontainer/devcontainer.json` that uses the devbox environment for VS Code / GitHub Codespaces compatibility
- Document devbox usage in a brief section of the existing README (or CLAUDE.md)

## Capabilities

### New Capabilities

- `devbox-environment`: Hermetic dev environment config — declares dotnet, Node.js, and Claude CLI packages; enables `devbox shell` to drop into a fully wired workspace

### Modified Capabilities

<!-- none — no existing spec-level requirements change -->

## Impact

- **New files**: `devbox.json`, `devbox.lock` (generated), `.devcontainer/devcontainer.json`
- **No breaking changes** to the template output or nuget packaging
- **Dependency**: devbox CLI must be installed on the host machine (one-time bootstrap); Claude CLI is installed inside the devbox shell via npm (`@anthropic-ai/claude-code`)
- **CI**: no changes required; devbox is opt-in for local development
