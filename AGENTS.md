# Agent Instructions for pdf-combine

This document describes the architecture, design, repository layout, build tools, and other pertinent information for agents working on this repository.

---

## Application Overview

**pdf-combine** is a Windows-only desktop application built with WinForms (.NET 5.0) that allows users to:
1. **Combine** multiple PDF files into a single PDF file (in user-defined order).
2. **Split** a PDF file by extracting specific pages (or page ranges) into separate PDF files.

The application was originally created to combine scanned PDF pages and later extended with a split/extract feature.

- **Author**: acnicholls
- **Version**: 2.0.0
- **License**: See `LICENSE.md`

---

## Repository Layout

```
pdf-combine/                         # Repository root
├── .github/
│   ├── dependabot.yml               # Dependabot NuGet auto-update config
│   └── workflows/
│       ├── build-debug-binaries.yml     # CI: builds Debug MSI on PRs to develop
│       ├── build-release-binaries.yml   # CI: builds Release MSI on PRs to main
│       └── create-release-with-asset.yml # CI: creates GitHub release with MSI artifact
├── .gitignore                       # Standard VS .gitignore; also ignores /pdf-combine-testing/pdfs/*
├── LICENSE.md
├── README.md
├── AGENTS.md                        # This file
├── pdf-combine.sln                  # Visual Studio Solution (two projects)
├── pdf-combine/                     # Main application project
│   ├── pdf-combine.csproj           # .NET 5.0 WinForms project file
│   ├── Program.cs                   # Application entry point
│   ├── PdfFileList.xsd              # Typed DataSet schema (auto-generates Designer.cs)
│   ├── PdfFileList.Designer.cs      # Auto-generated (DO NOT EDIT manually)
│   ├── PdfFileList.cs               # Partial class extension (if any manual additions)
│   ├── PdfFileList.xsc              # DataSet configuration
│   ├── PdfFileList.xss              # DataSet styling
│   ├── Forms/
│   │   ├── frmMain.cs               # Main application shell form
│   │   ├── frmMain.Designer.cs      # Auto-generated designer code
│   │   ├── frmAbout.cs              # About dialog form
│   │   └── frmAbout.Designer.cs     # Auto-generated designer code
│   ├── UserControls/
│   │   ├── ctlCombine.cs            # Combine-feature UI control
│   │   ├── ctlCombine.Designer.cs   # Auto-generated designer code
│   │   ├── ctlSplit.cs              # Split-feature UI control
│   │   └── ctlSplit.Designer.cs     # Auto-generated designer code
│   └── Func/
│       ├── Combine.cs               # PDF combining business logic
│       └── Split.cs                 # PDF splitting business logic
└── pdf-combine-setup/
    └── pdf-combine-setup.vdproj     # Visual Studio Installer (MSI) project
```

> **Note**: `pdf-combine-testing/` is referenced in `.gitignore` and is used locally for manual testing; it is not committed.

---

## Architecture

The application uses a **layered WinForms architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│  Presentation Layer  (Forms/ and UserControls/)          │
│   frmMain  ──►  ctlCombine  ──►  Func/Combine.cs        │
│            └──►  ctlSplit   ──►  Func/Split.cs           │
├─────────────────────────────────────────────────────────┤
│  Data Layer  (PdfFileList DataSet)                       │
│   PdfFileList.xsd / PdfFileList.Designer.cs              │
├─────────────────────────────────────────────────────────┤
│  External Library  (PdfSharp)                            │
│   PdfReader, PdfDocument, PdfDocumentOpenMode            │
└─────────────────────────────────────────────────────────┘
```

### Key Design Decisions

- **Single-window shell with swappable UserControls**: `frmMain` hosts two user controls (`ctlCombineUi` and `ctlSplitUi`). The active feature is shown by toggling `.Show()` / `.Hide()` based on the menu item clicked. There are no tabs; only one control is visible at a time.
- **Static business logic methods**: `Combine.Start()` and `Split.Start()` are `public static bool` methods. They receive their input data as parameters and return `true` on success, `false` on failure. They do not depend on any instance state.
- **ADO.NET Typed DataSet for file list**: The Combine feature maintains a list of files using a strongly-typed `PdfFileList` DataSet (generated from `PdfFileList.xsd`). The `FileList` table has columns: `id` (int, auto-increment PK), `filename` (string), `filePath` (string), `order` (int).
- **Serilog for structured logging**: Logging is initialized in `Program.cs` (before the form launches) and used via `Log.Logger` throughout. Logs go to both the console and a daily rolling file at `logs/pdf-combine.txt`.
- **Encoding registration**: `System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)` is called at startup to support non-ASCII characters in PDF filenames/content (required by PdfSharp on .NET 5+).

---

## Design Patterns

| Pattern | Where Used | Notes |
|---|---|---|
| Static façade | `Func/Combine.cs`, `Func/Split.cs` | Business logic exposed as static methods with no side effects beyond file I/O |
| Observer (WinForms Events) | All `_Click` handlers | Standard WinForms event-driven pattern |
| Data Binding | `ctlCombine.cs` | `lbFileList` ListBox bound to `PdfFileList.FileList` DataTable rows via `DataSource` / `DisplayMember` / `ValueMember` |
| Swap Panel | `frmMain.cs` | Two UserControls placed in the same position; only one visible at a time, toggled by menu click |

---

## Forms and Controls

### `frmMain` (285 × 335 px)
- Non-maximizable, centered on screen.
- Contains a `MenuStrip` (`MainMenu`) with three items:
  - `tsmCombine` → shows `ctlCombineUi`, hides `ctlSplitUi`
  - `tsmSplit` → shows `ctlSplitUi`, hides `ctlCombineUi`
  - `tsmAbout` → opens `frmAbout` as a modal dialog
- On load, Combine UI is shown by default.

### `ctlCombine` (285 × 301 px)
- `btnAdd` → opens a multi-select `OpenFileDialog` (PDF filter) and adds files to `PdfFileList`.
- `lbFileList` → ListBox data-bound to `PdfFileList.FileList` ordered by `order`.
- `btnOrderUp` / `btnOrderDown` → swap the `order` values of adjacent rows, then rebind the list.
- `btnRemove` → removes selected row from DataSet.
- `btnCombine` → opens `SaveFileDialog`, gathers all rows, calls `Combine.Start()`, shows MessageBox on success.

### `ctlSplit` (285 × 301 px)
- `btnChoose` → opens `OpenFileDialog`, reads the PDF page count, populates `txtFileName` and `txtNumOfPages`.
- `txtPageRange` → user types page list (e.g., `1,3,5-8`).
- `btnSplit` → validates `txtPageRange`, expands ranges into a flat `IList<int>`, calls `Split.Start()`.
- Input validation is handled by `validateBtnSplitText()` which returns an `ErrorMessage` enum.

### `frmAbout` (478 × 139 px)
- Modal, non-resizable dialog.
- Shows version (2.0.0.0), author, copyright, and a disclaimer.

---

## Business Logic

### `Func/Combine.cs` — `Combine.Start(List<FileListRow>, string outputFilename)`
```
1. Create empty PdfDocument.
2. Iterate fileListRows ordered by row.order.
3. For each file: open with PdfReader in Import mode.
4. Copy every page to the new document.
5. Save to outputFilename.
6. Return true on success, false (+ log) on exception.
```

### `Func/Split.cs` — `Split.Start(IList<int> pageListRows, string inputFilename)`
```
1. Derive output directory: <same directory as input>/<filename-without-extension>/
2. Create the output directory if it does not exist.
3. Open inputFilename with PdfReader in Import mode.
4. For each 0-based page index in pageListRows:
   a. Create a new PdfDocument.
   b. Add the single page (oldFile.Pages[pageIndex]).
   c. Save as "Page<pageIndex+1>.pdf" in the output directory.
   d. Close the new document.
5. Return true on success, false on exception.
```

> **Important**: Page indices are 0-based internally. User input is 1-based. The conversion (`x - 1`) happens in `ctlSplit.btnSplit_Click` before calling `Split.Start()`.

### Split Page Range Validation (`ctlSplit.validateBtnSplitText`)
Accepts a comma-separated list like `1,3,5-8,10`. Validates:
1. Only digits, dashes, and commas are present (regex `[^1234567890\-,]`, equivalent to `[^\d\-,]`).
2. Each comma-delimited token with a dash must match `\d+\-\d+`.
3. In a range `a-b`, `a` must be less than `b`.
4. All page numbers must be ≥ 1 and ≤ total pages in the document.

---

## NuGet Dependencies

| Package | Version | Purpose |
|---|---|---|
| `PdfSharp` | 1.50.5147 | Core PDF read/write; `PdfReader`, `PdfDocument`, `PdfDocumentOpenMode` |
| `Serilog` | 2.12.0 | Structured logging framework |
| `Serilog.Sinks.Console` | 4.1.0 | Logging to stdout |
| `Serilog.Sinks.File` | 5.0.0 | Daily rolling file logging to `logs/pdf-combine.txt` |
| `System.Text.Encoding.CodePages` | 7.0.0 | Encoding support required by PdfSharp on .NET 5+ |

Dependencies are managed via NuGet and restored with `nuget restore` or `dotnet restore`.

---

## Required Build Tools

| Tool | Version / Notes | Purpose |
|---|---|---|
| .NET SDK | 5.0 (Windows) | Compile and run the WinForms application |
| MSBuild | (bundled with VS 2019/2022) | Build solution/project files |
| Visual Studio `devenv` | 2019 or 2022 | Required to build the `pdf-combine-setup` VDPROJ installer project |
| NuGet CLI | Latest stable | Restore NuGet packages (`nuget restore pdf-combine.sln`) |
| Windows OS | Windows 10/11 | WinForms targets `net5.0-windows`; cannot build on Linux/macOS |

> **Important**: The installer project (`pdf-combine-setup.vdproj`) is a legacy Visual Studio Setup Project. It **cannot** be built by `msbuild` alone — it requires `devenv` (the full Visual Studio IDE command-line). This is reflected in all three CI workflows.

### Building Locally

```powershell
# Restore NuGet packages
nuget restore .\pdf-combine.sln

# Build with devenv (Debug)
devenv .\pdf-combine.sln /Build Debug /Project pdf-combine-setup /NoSplash

# Build with devenv (Release)
devenv .\pdf-combine.sln /Build Release /Project pdf-combine-setup /NoSplash
```

For development builds without the installer:
```powershell
dotnet build .\pdf-combine\pdf-combine.csproj -c Debug
```

---

## CI/CD Workflows

All workflows run on `windows-latest` runners.

| Workflow | Trigger | Output |
|---|---|---|
| `build-debug-binaries.yml` | PRs to `develop` branch | Debug MSI artifact (`pdf-combine-debug-setup.msi`), build log |
| `build-release-binaries.yml` | PRs to `main` branch | Release MSI artifact, build log |
| `create-release-with-asset.yml` | Manual / release event | GitHub Release with MSI attached |

Each workflow:
1. Checks out code.
2. Sets up MSBuild (`microsoft/setup-msbuild@v1`).
3. Sets up VS Dev Environment (`seanmiddleditch/gha-setup-vsdevenv@v4`).
4. Runs `nuget restore`.
5. Runs `devenv` to build the installer project.
6. Uploads build log and MSI as GitHub Actions artifacts (5-day retention).

**Dependabot** is configured to automatically open PRs for NuGet package version updates.

---

## Testing

There is currently **no automated test project** in this repository. Testing is performed manually using PDFs placed in a local `pdf-combine-testing/pdfs/` directory (excluded from source control via `.gitignore`).

If adding tests, use a standard .NET test framework (xUnit or MSTest) in a new project added to the solution. Business logic in `Func/Combine.cs` and `Func/Split.cs` (static methods with file I/O) and validation logic in `ctlSplit.validateBtnSplitText` are the most suitable candidates for unit testing.

---

## Namespace and Coding Conventions

- **Root namespace**: `pdf_combine` (note: underscore, not hyphen — C# identifier constraint).
- **Forms**: in `pdf_combine.Forms` namespace, named `frm<Name>` (e.g., `frmMain`, `frmAbout`).
- **UserControls**: in `pdf_combine.UserControls` namespace, named `ctl<Name>` (e.g., `ctlCombine`, `ctlSplit`).
- **Business logic**: in `pdf_combine.Func` namespace, named by feature (`Combine`, `Split`).
- **Designer files** (`*.Designer.cs`): auto-generated by Visual Studio. Do not edit manually; use the designer surface instead.
- **PdfFileList.Designer.cs**: auto-generated from `PdfFileList.xsd` using the `MSDataSetGenerator` custom tool. Do not edit manually.
- Logging uses `Log.Logger.Information(...)`, `Log.Logger.Error(...)`, etc. (Serilog static accessor).
- XML doc comments (`/// <summary>`) are used on public/private methods throughout.

---

## Known Issues and Planned Work (from README)

- **v2.1 (planned)**: Bug fixes, improved input validation and parsing for the Split feature.
- The `validateBtnSplitText` method has a known code path where `MoreThanOneDash` is never returned (the code returns `InvalidCharacters` in that branch instead) — this is a minor logic inconsistency.
- Single-character page inputs (e.g., `"5"`) bypass the range-format checks and only validate against the invalid-character regex.

---

## Glossary

| Term | Meaning |
|---|---|
| Combine | Merge multiple PDF files into one, in user-specified order |
| Split / Extract | Copy selected pages from one PDF into individual output files |
| Page range | A string like `1,3,5-8,10` representing pages to extract |
| `PdfFileList` | The typed DataSet used to hold the list of files for combining |
| `FileListRow` | A single row in the `PdfFileList.FileList` DataTable |
| VDPROJ | Visual Studio Setup Project format (legacy installer technology) |
| Import mode | `PdfDocumentOpenMode.Import` — required when copying pages between PdfSharp documents |
