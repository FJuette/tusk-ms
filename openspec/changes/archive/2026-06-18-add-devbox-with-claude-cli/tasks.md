## 1. Create devbox configuration

- [x] 1.1 Create `devbox.json` at repo root with dotnet SDK (pinned to the version used in `template/global.json` or `Dockerfile`), nodejs LTS, and an `init_hook` that runs `npm install -g @anthropic-ai/claude-code`
- [x] 1.2 Run `devbox install` to generate `devbox.lock` and commit it alongside `devbox.json`
- [x] 1.3 Verify `devbox shell` activates without errors and `dotnet --version`, `node --version`, `claude --version` all succeed

## 2. Add Dev Container support

- [x] 2.1 Run `devbox generate devcontainer` inside the repo root to produce `.devcontainer/devcontainer.json`
- [x] 2.2 Open the repo in VS Code Dev Containers and confirm dotnet, node, and claude are available on the PATH

## 3. Documentation

- [x] 3.1 Add a "Development environment" section to the project README (or CLAUDE.md) documenting: prerequisite (`devbox` install link), `devbox shell` usage, and how Claude CLI is available automatically
