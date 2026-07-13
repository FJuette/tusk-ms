## ADDED Requirements

### Requirement: Devbox configuration file exists
The repository SHALL contain a `devbox.json` at the root that declares all required development packages including the correct .NET SDK version and Node.js LTS.

#### Scenario: Developer initialises the shell
- **WHEN** a developer runs `devbox shell` in the repository root
- **THEN** a hermetic shell is activated with dotnet and node available at the pinned versions

#### Scenario: Package versions are pinned
- **WHEN** `devbox.json` and `devbox.lock` are committed
- **THEN** any developer who runs `devbox shell` on any machine gets identical tool versions

### Requirement: Claude CLI available inside devbox shell
The devbox environment SHALL install the Claude CLI (`@anthropic-ai/claude-code`) globally via npm so that `claude` is on the PATH within `devbox shell`.

#### Scenario: Claude CLI is accessible after shell activation
- **WHEN** a developer runs `devbox shell`
- **THEN** running `claude --version` succeeds and prints the installed version

#### Scenario: Claude CLI installation is automatic
- **WHEN** a developer enters `devbox shell` for the first time on a clean machine
- **THEN** the `init_hook` installs Claude CLI without any manual npm install step

### Requirement: Dev Container support
The repository SHALL contain a `.devcontainer/devcontainer.json` that uses the devbox-generated container image.

#### Scenario: VS Code Dev Container opens correctly
- **WHEN** a developer opens the repository in VS Code with the Remote-Containers extension and selects "Reopen in Container"
- **THEN** the container starts with the same tools available as in `devbox shell` (dotnet, node, claude)

#### Scenario: GitHub Codespaces compatibility
- **WHEN** the repository is opened in GitHub Codespaces
- **THEN** the devcontainer configuration is detected and the environment is provisioned with dotnet, node, and claude available
