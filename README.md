# SeleniumMStestProject

A C#/.NET Selenium + MSTest test-automation framework, covering both UI
(Selenium WebDriver) and API (`HttpClient`) testing, with a GitHub Actions
pipeline that runs the suite on every push and pull request.

- **UI under test:** https://seleniumbase.io/demo_page
- **API under test:** https://automationexercise.com/api_list

## Tech stack

- .NET 8.0 (SDK-style project, cross-platform)
- MSTest (test framework + adapter)
- Selenium WebDriver 4.44 (Selenium Manager resolves the matching ChromeDriver
  automatically — no manual driver download needed)
- `System.Text.Json` for API response deserialization
- GitHub Actions for CI

## Project structure

```
SeleniumMStestProject/
├── Base/
│   ├── SeleniumTestBase.cs    # ChromeDriver setup/teardown, headless in CI,
│   │                          # auto screenshot capture on test failure
│   └── ApiTestBase.cs         # shared HttpClient setup/teardown
├── Controls/
│   ├── Control.cs             # reusable, wait-backed element wrapper
│   └── TextFieldControl.cs    # text-input-specific control
├── Pages/
│   └── TestPageLanding.cs     # Page Object for the demo page
├── Utilities/
│   └── WaitHelper.cs          # WebDriverWait-based visible/clickable waits
├── Toggles/
│   └── FeatureToggle.cs       # config-driven feature toggle example
├── Constants/
│   ├── Constants.cs           # timeout presets
│   └── TestCategories.cs      # Smoke/E2E/Regression/Api category names
├── Configuration.cs           # typed access to App.config settings
├── App.config                 # BaseUrl, ApiBaseUrl, wait timeouts, toggle
└── Tests/
    ├── Smoke/                 # fast, critical-path UI checks
    ├── E2E/                   # full user-journey UI tests
    ├── Regression/            # targeted UI regression checks
    └── Api/                   # HTTP tests against automationexercise.com,
        └── Models/            # typed response models for deserialization
```

A few other folders (`Attributes/`, `Exceptions/`, `Enums/`, `Objects/`,
`Types/`, `Queries/`, plus root-level `TestManagement.cs` / `TestPage.cs`)
are pre-existing scaffolding not yet wired into anything — known cleanup
backlog, left alone intentionally rather than touched blindly.

## Getting started

Prerequisites:
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Google Chrome installed locally (for UI tests)

```bash
dotnet restore
dotnet build
```

## Running tests

Run everything:
```bash
dotnet test
```

Run a specific category (Smoke, E2E, Regression, or Api):
```bash
dotnet test --filter "TestCategory=Smoke"
```

UI tests run headed by default locally. Set `HEADLESS=true` (or `CI=true`,
which CI sets automatically) to force headless Chrome.

## Configuration

Settings live in `SeleniumMStestProject/App.config` and are exposed via the
`Config` class:

| Key | Purpose | Default |
|---|---|---|
| `BaseUrl` | UI target for Selenium tests | `https://seleniumbase.io/demo_page` |
| `ApiBaseUrl` | API target for HTTP tests | `https://automationexercise.com` |
| `ImplicitWaitSeconds` / `ExplicitWaitSeconds` | Wait timeouts used by `WaitHelper` | `10` / `30` |
| `FeatureToggle.EnableNavigationDropdownTest` | Gates the nav-dropdown step in the E2E suite (`Assert.Inconclusive` when off) | `true` |

## CI/CD

`.github/workflows/pr-tests.yml` runs on every push/PR to `master`:
1. **Build** — restore + build the solution.
2. **Smoke** and **Api** then run in parallel (both only depend on Build).
3. **E2E & Regression** (headless Chrome) runs after Smoke passes, as a gate against
   spending time on the slower suite if the fast smoke checks already fail.

Each job publishes its `.trx` results as a build artifact.
