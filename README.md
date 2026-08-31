# SeleniumMStestProject

A C#/.NET Selenium + MSTest test-automation framework, covering both UI
(Selenium WebDriver) and API (`HttpClient`) testing, with a GitHub Actions
pipeline that runs the suite on every push and pull request.

- **UI under test:** https://automationexercise.com 
- **API under test:** https://automationexercise.com/api_list

## Tech stack

- .NET 8.0 (SDK-style project, cross-platform)
- MSTest (test framework + adapter)
- Selenium WebDriver 4.44, Chrome/Firefox/Edge (Selenium Manager resolves the
  matching driver for each browser automatically — no manual driver download
  needed)
- `System.Text.Json` for API response deserialization
- GitHub Actions for CI

## Project structure

```
SeleniumMStestProject/
├── Base/
│   ├── SeleniumTestBase.cs    # driver lifecycle (Chrome/Firefox/Edge), headless
│   │                          # in CI, ad-domain blocking + eager page load
│   │                          # (see "Live-site quirks" below), auto screenshot
│   │                          # capture on test failure
│   └── ApiTestBase.cs         # shared HttpClient setup/teardown
├── Controls/
│   ├── Control.cs             # reusable, wait-backed element wrapper
│   └── TextFieldControl.cs    # text-input-specific control
├── Pages/                     # one page object per automationexercise.com page
│   ├── HomePage.cs            # nav, subscribe footer, category/brand sidebar
│   ├── LoginPage.cs           # login form + signup (name/email) form
│   ├── SignupPage.cs          # account-information form + created/deleted confirmations
│   ├── AccountFixture.cs      # shared register/delete-account setup+teardown
│   ├── ProductsPage.cs        # grid, search, add-to-cart, category/brand pages
│   ├── ProductDetailsPage.cs  # quantity, add-to-cart, review form
│   ├── CartPage.cs            # cart rows, quantity, delete, checkout, subscribe
│   ├── CheckoutPage.cs        # delivery address, order comment, place order
│   └── PaymentPage.cs         # card fields, order confirmation, download invoice
├── Utilities/
│   ├── WaitHelper.cs           # WebDriverWait-based visible/clickable waits
│   ├── WebElementExtensions.cs # SafeClick (JS-click fallback) + JS-driven <select>
│   └── TestDataGenerator.cs    # unique email/name generation per test run
├── Toggles/
│   └── FeatureToggle.cs       # config-driven feature toggle example
├── Enums/
│   └── Enums.cs               # BrowserType (used to run E2E tests across
│                               # Chrome/Firefox/Edge); LanguageType and
│                               # EnvironmentType are still unused
├── Constants/
│   ├── Constants.cs           # timeout presets
│   └── TestCategories.cs      # Smoke/E2E/Regression/Api category names
├── Configuration.cs           # typed access to appsettings.json
├── appsettings.json           # BaseUrl, ApiBaseUrl, wait timeouts, toggle
└── Tests/
    ├── Smoke/                 # fast, critical-path checks (home page, search,
    │                           # subscribe, add-to-cart)
    ├── E2E/                   # full user journeys: register/login/logout,
    │                           # register-then-checkout, login-then-checkout
    ├── Regression/            # targeted checks against individual Test Cases
    │                           # from automationexercise.com/test_cases
    └── Api/                   # HTTP tests against automationexercise.com,
        └── Models/            # typed response models for deserialization
```

A few other folders (`Attributes/`, `Exceptions/`, `Objects/`, `Types/`,
`Queries/`, plus root-level `TestManagement.cs` / `TestPage.cs`) are
pre-existing scaffolding not yet wired into anything — known cleanup backlog, left alone for now.

## Getting started

Prerequisites:
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Google Chrome installed locally (for most UI tests); Firefox and Edge are
  only needed if you're running the cross-browser E2E test locally

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
which CI sets automatically) to force headless mode.

### Test coverage

The UI suites don't automate all 26 scenarios from
automationexercise.com/test_cases — each suite takes a representative slice
sized for what that suite is for, and every test method/name references its
official Test Case number where one applies:

- **Smoke** — home page loads, Test Cases page is reachable (TC7),
  subscribing on the home page (TC10), searching for a product (TC9),
  adding a product to cart from the grid (TC12).
- **Regression** — independent, targeted checks: incorrect login (TC3),
  duplicate-email signup (TC5), Contact Us (TC6), cart-page subscribe
  (TC11), cart quantity from the product page (TC13), removing a cart item
  (TC17), category browsing (TC18), brand browsing + add to cart (TC19),
  product review (TC21, gated by `FeatureToggle.EnableProductReviewTest`),
  and checkout address details matching registration (TC23).
- **E2E** — full multi-page journeys with real account creation/cleanup:
  register + delete account (TC1), login then logout (TC2 + TC4), place an
  order registering at checkout (TC15), and place an order logging in
  before checkout (TC16).

### Cross-browser E2E testing

Only `UserCanRegisterAndDeleteAccount` runs cross-browser, via
`[DataRow(BrowserType.Chrome/Firefox/Edge)]` — it's the cheapest full
journey (no cart/checkout/payment), so it's the one used to prove
forms/navigation work on all three browsers. The other E2E tests involve a
full cart → checkout → payment flow and run Chrome-only, to avoid tripling
load on the live site for marginal extra signal. Smoke and Regression tests
also always use Chrome. Under the hood, `SeleniumTestBase` lazily creates a
Chrome driver on first use by default; tests that need a specific browser
call `InitializeDriver(BrowserType.X)` first. `BrowserType.Safari` is
declared but not implemented — no practical cross-platform/CI story for it
here.

### Live-site quirks (and how the tests cope with them)

automationexercise.com is a real, ad-monetized site, not a static fixture,
which surfaced a few real issues while rebuilding these suites against it:

- **Ad iframes intercept clicks.** Google ad iframes can grow to cover much
  of the viewport and sit on top of real page elements (the footer
  subscribe button, a product's review-submit button, etc.).
  `SeleniumTestBase` null-routes the known ad domains via
  `--host-resolver-rules` for Chrome/Edge, and `WebElementExtensions.SafeClick`
  falls back to a JS-dispatched click if a normal click still gets
  intercepted (e.g. on Firefox, which has no equivalent host-resolver flag).
- **A slow ad/video resource can hang page-load-complete**, which made
  GeckoDriver block for a full 60 seconds on an otherwise-successful click
  in Firefox. All three browsers use `PageLoadStrategy.Eager` so WebDriver
  returns once the DOM is interactive rather than waiting on every
  sub-resource; `WaitHelper`'s explicit waits still guarantee element
  readiness regardless of page-load strategy.
- **Native `<select>` clicks are also click-interception targets.** The
  account-information form's day/month/year/country dropdowns are set via
  `WebElementExtensions.SelectByValueViaJs` / `SelectByTextViaJs` (set the
  value, dispatch a `change` event) instead of Selenium's
  `SelectElement`, which opens/selects via two more real clicks.
- **Heading text renders uppercase via CSS**, so `Element.Text` on
  category/brand page headings doesn't match the page's HTML source
  case-for-case — assertions compare case-insensitively
  (`ProductsPage.NormalizedGridHeading` also collapses stray whitespace the
  site's template leaves in category/brand names).
- **The site occasionally rate-limits with "under heavy load (queue full)"**
  under sustained parallel traffic. `[assembly: Parallelize(Workers = 4, ...)]`
  (`Properties/AssemblyInfo.cs`) caps concurrent browser sessions rather
  than defaulting to processor count, which is both kinder to the live site
  and avoids the resource contention/timeouts that many-at-once real
  browser sessions cause locally.

## Configuration

Settings live in `SeleniumMStestProject/appsettings.json` and are exposed
via the `Config` class:

| Key | Purpose | Default |
|---|---|---|
| `BaseUrl` | UI target for Selenium tests | `https://automationexercise.com` |
| `ApiBaseUrl` | API target for HTTP tests | `https://automationexercise.com` |
| `ImplicitWaitSeconds` / `ExplicitWaitSeconds` | Wait timeouts used by `WaitHelper` | `10` / `30` |
| `FeatureToggle.EnableProductReviewTest` | Gates the product-review submission step in the Regression suite (`Assert.Inconclusive` when off), so CI can stop posting reviews to the public form without a code change | `true` |

## CI/CD

`.github/workflows/pr-tests.yml` runs on every push/PR to `master`:
1. **Build** — restore + build the solution.
2. **Smoke** and **Api** then run in parallel (both only depend on Build).
3. **E2E & Regression** runs after Smoke passes, as a gate against spending
   time on the slower suite if the fast smoke checks already fail. This job
   sets up Chrome and Firefox explicitly (`browser-actions/setup-*`); Edge
   relies on the version preinstalled on the `ubuntu-latest` runner image, so
   unlike Chrome/Firefox it isn't pinned by the workflow.

Each job publishes its `.trx` results as a build artifact.
