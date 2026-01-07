# LiftLugCalc2 - Engineering Lifting Lug Calculator
**Professional-grade lifting lug verification software following NORSOK R-002 standards.**

## Features

✅ **Forward Calculation** - Verify if selected lug passes for given load  
✅ **Reverse Calculation** - Suggest best lug from table for given load  
✅ **NORSOK R-002 load distribution** - 1, 2, 3, 4-point sling configurations  
✅ **Weight Correction Factors (WCF)** - Account for weight estimation uncertainty  
✅ **Complete capacity checks** - Tension, bearing, tear-out, shear, welds  
✅ **Project persistence** - Save/load projects with selections  
✅ **Professional console UI** - ASCII art, formatted tables, color coding  

## Architecture

```
LiftLugCalc2/
├── Core/           # Pure engineering logic
│   ├── Engine/     # Calculators (ForwardCalculator, ReverseCalculator)
│   ├── Models/     # ProjectInput, TableLug, Material, LugCheckResult
│   ├── FileOperations/ # Pure loaders (TableLugLoader, MaterialLoader)
│   ├── Resources/      # LugType.csv, Materials.csv
│   └── Utilities/  # PreliminaryCalculations, Constants
└── Console/        # UI layer only
    ├── Program.cs  # Thin orchestrator
    ├── UICommon.cs # All ASCII art, prompts, colors
    └── InputValidator.cs
```

**Engineering-First Principle**: Core contains only math/models. UI is 100% replaceable.

## Quick Start

1. **Clone & Build**
   ```bash
   git clone <repo>
   cd LiftLugCalc2
   dotnet build
   dotnet run
   ```

2. **CSV Files** (auto-loaded):
   - `Resources/LugType.csv` - Lug geometry table
   - `Resources/Materials.csv` - Material properties (S355, S355JR, etc.)

3. **Usage Flow**:
   ```
   Create Project → Enter WLL → Choose WCF → Lifting points (1-4) →
   Forward (check lug) OR Reverse (suggest lug) → Save results
   ```

## Standards Compliance

- **NORSOK R-002** (Edition 3, 2017+A1:2019)[2]
  - Load per point (PLP) calculation for 1-4 sling legs
  - Dynamic Amplification Factor (DAF = 1.5-2.0)
  - Weight Correction Factors (WCF = 1.03-1.3)

- **Capacity checks**:
  | Check | Formula |
  |-------|---------|
  | Net Section | `2 × (R - d/2) × t` |
  | Tension | `fy/γ_Rm × Anet / 1000` |
  | Bearing | `3.0 × fy × d × t / 1000` |
  | Tear-out | `1.2 × e × t × fy/√3 / 1000` |

## File Structure

```
Documents/LiftingLugCalc2/<ProjectName>/
├── Data/
│   └── project.txt          # ProjectInput + selections
├── Results/
│   └── ForwardReport_*.txt  # Calculation summaries
└── Logs/                    # Future logging
```

## CSV Format

**LugType.csv** (`;`-separated):
```
LugID;LugType;LugWLL;ThicknessPlate;DiameterHole;RadiusLug;...
1;0;15000;20;30;50;...
```

**Materials.csv** (`;`-separated):
```
MaterialID;Designation;YieldStrength;TensileStrength;YoungModulus;PoissonRatio
1;S355;355;510;210000;0.3
```

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `1-4` | Main menu |
| `Enter` | Accept defaults |
| `Q` | Quit |
| `Ctrl+C` | Emergency exit |

## Testing

Run verification tests:
```csharp
// In Core/Tests/LugVerificationTests.cs
LugVerificationTests.RunAll();
```

Tests verify:
- Net section area formulas
- Capacity calculations  
- Forward/Reverse calculator integration
- NORSOK load distribution

## Building & Deployment

```bash
dotnet build --configuration Release
dotnet publish -c Release -r win-x64 --self-contained
```

**Output**: `bin/Release/net10.0-windows7.0/win-x64/publish/LiftLugCalc2.exe`

## Planned Features

- [ ] PDF report generation
- [ ] Weld geometry validation
- [ ] Multiple load cases per project
- [ ] Export to Excel/CSV
- [ ] WPF GUI version
- [ ] Unit test coverage >95%

## License

MIT License

## Standards References

- **NORSOK R-002** - Lifting Equipment[2]
- **Eurocode 3** - Steel structures
- **DNV 2.7-1** - Offshore containers

