# LiftLugCalc2 - Engineering Lifting Lug Calculator v2
**LiftLugCalc2** is a C# engineering application for lifting-lug calculations. This version is the graphical implementation of the original console-based application, using **Silk.NET** and **ImGui.NET** for the user interface.
The application follows an **Engineering-First Architecture**: engineering calculations remain separate from the graphical interface, while the GUI provides a straightforward workflow for defining, calculating, reviewing, saving, loading, and reporting lifting-lug projects.

## Purpose

The application performs lifting-lug calculations and checks based on the engineering requirements implemented in the original LiftLugCalc calculation engine, including checks associated with:
- NORSOK R-002 Annex F
- NORSOK R-002 Annex J
- EN 13001

## Engineering Philosophy

The application follows a few simple principles:
- **Engineering first** — engineering calculations are kept independent of the GUI.
- **Procedural calculation flow** — the calculation engine performs the engineering work; GUI classes do not contain engineering calculations.
- **KISS** — keep the implementation simple and understandable.
- **DRY** — avoid unnecessary duplication.
- **Clear engineering intent** — code should remain understandable to an engineer who is not primarily a software developer.
- **No unnecessary UI complexity** — the application is a focused engineering tool rather than a general-purpose desktop framework.

## Calculation Workflow

The normal workflow is linear:
```text
Project Setup
      ↓
Weight Definition
      ↓
Lift Geometry
      ↓
Calculation Mode
      ↓
Material Selection
      ↓
 ┌───────────────┐
 │               │
Forward        Reverse
 │               │
Lug Selection    │
 │               │
 └───────┬───────┘
         ↓
       Run
         ↓
      Results
```

### Forward Calculation

The user defines the project, loading and lifting geometry, selects the calculation mode, material and lug, and then runs the calculation.

### Reverse Calculation

The user defines the project, loading and lifting geometry, selects the calculation mode and material, and the application evaluates the available lug types to determine a suitable lug.

## GUI

The graphical interface is built with:

- **C#**
- **Silk.NET**
- **ImGui.NET**
- **OpenGL**

The application uses a single main window with four principal areas:

```text
┌──────────────────────────────────────────────────────┐
│ Navigation │                Content                  │
│            │                                          │
│ Project    │                                          │
│ Setup      │                                          │
│ Weight     │                                          │
│ Geometry   │                                          │
│ ...        │                                          │
├────────────┴──────────────────────────────────────────┤
│ Status                                                 │
├────────────────────────────────────────────────────────┤
│ NEW   OPEN   SAVE   REPORT                            │
└────────────────────────────────────────────────────────┘
```

The navigation panel indicates the current workflow step and provides visual status information for completed or problematic steps. The command bar changes according to the current workflow screen and provides commands such as:

- BACK
- NEXT
- RUN
- MODIFY / RE-RUN
- NEW
- OPEN
- SAVE
- REPORT

Buttons that cannot currently be used are disabled.

## Project Management

Projects are stored locally under the user's Documents directory using the configured application directory. A project is stored in its own directory:

```text
Documents/
└── <application directory>/
    └── <Project Name>/
        ├── project.txt
        └── Report_<date>.txt
```

The current persistent project information includes:

- Project identification
- Project name
- Created by
- Revision
- Date
- Weight basis
- Input weight
- WCF selection
- WLL
- Number of lifting points
- A1
- A2
- B1
- B2
- Selected lug ID
- Selected material ID

The GUI supports:
- Creating a new project
- Opening an existing project
- Saving a project
- Generating a calculation report

Opening a project restores the project information and returns the user to **Project Setup**, allowing the loaded project to be reviewed or modified before proceeding through the calculation workflow again.

## Results

The Results window presents the engineering checks in a table, including:
- Tension
- Shear
- Bearing
- Tear-out
- Weld

For each check, the application displays the calculated capacity and safety factor. The minimum safety factor and weld geometry result are also shown, followed by the overall calculation result. 
Safety-factor values are additionally presented with visual indicators to provide information about the margin relative to configured review criteria. The application does **not** treat those visual safety-factor thresholds as NORSOK Pass/Fail requirements. The engineering **acceptance** of safety factors remains as the **responsibility of the engineer**.
The overall calculation result represents the suitability according to the calculation criteria implemented by the engineering calculation engine.

## Reporting

Reports are saved in the current project's directory using the current naming convention:
```text
Report_dd-MM-yyyy.txt
```
The report naming convention may be changed in a later development stage.

## Application Architecture

The GUI is intentionally separated from the engineering calculation layer.
A simplified structure is:
```text
Program
   │
   └── GuiController
          │
          ├── Session
          ├── GUI Windows
          ├── Navigation
          └── Command Bar
                 │
                 ▼
          Engineering Calculators
                 │
                 ├── ForwardCalculator
                 └── ReverseCalculator
```

Important GUI components include:
- `GuiController`
- `Session`
- `MainWindow`
- `MenuWindow`
- `ProjectSetupWindow`
- `WeightDefinitionWindow`
- `LiftGeometryWindow`
- `CalculationModeWindow`
- `MaterialSelectionWindow`
- `LugSelectionWindow`
- `ForwardCalculationWindow`
- `ReverseCalculationWindow`
- `ResultsWindow`
- `OpenProjectWindow`
- `NavigationPanel`
- `CommandBar`
- `StatusBar`
- `GuiCommon`

Shared GUI dimensions and layout values are maintained in `GuiConstants`.
Project persistence and file handling are handled through:
- `FilingSystem`
- `Formatter.ProjectFormatter`

Engineering reporting is handled through:
- `ReportGenerator`

The GUI does not perform the engineering calculations itself.

## Data Sources

The application loads lifting-lug and material data from the application's configured data files. These are loaded during application startup and made available to the GUI and calculation engine.

## Current Development Status

The ImGui implementation currently provides a complete basic project workflow:
- [x] Graphical application startup
- [x] Main menu
- [x] Project setup
- [x] Weight definition
- [x] Lift geometry
- [x] Calculation mode selection
- [x] Material selection
- [x] Lug selection
- [x] Forward calculation
- [x] Reverse calculation
- [x] Results presentation
- [x] Navigation/status indicators
- [x] Project creation
- [x] Project saving
- [x] Project opening
- [x] Project data restoration
- [x] Report generation
- [x] Modify / re-run workflow
- [x] New-project confirmation
- [x] Input/progression validation
- [x] Lift-geometry asymmetry warning
- [x] Safety-factor visual indicators

## Future Development

Potential future improvements include:
- Further refinement of project lifecycle handling
- Improved presentation of reverse-calculation candidates
- Engineering diagrams and lift-geometry visualisation
- Further refinement of the Results window
- More detailed calculation transparency
- Additional engineering-oriented GUI improvements

Future features should preserve the application's Engineering-First philosophy and avoid introducing unnecessary UI complexity.

## Technology
- **Language:** C#
- **GUI:** ImGui.NET
- **Graphics/windowing:** Silk.NET
- **Graphics API:** OpenGL
- **Development environment:** Visual Studio
- **Data:** CSV / text files
- **Project persistence:** Text files

## Disclaimer
This application is an engineering calculation tool. Results should be reviewed by a suitably qualified engineer and used in accordance with the applicable engineering standards, project requirements, design basis, and engineering judgement.
The software's calculated result and visual indicators do not replace engineering review or approval.
