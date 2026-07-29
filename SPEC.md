# Project Spec: DataWiz — Dataset Explorer & Cleaner

## 1. Overview
A WinUI 3 desktop app that lets a user load a CSV file, explore its structure and quality (summary stats, missing values, outliers), apply quick cleaning operations, and export a cleaned version — a lightweight, GUI-driven alternative to a pandas notebook workflow.

**Why this project:** It's a self-directed practice build to reinforce WinUI 3 fundamentals (data grids, file I/O, charts, MVVM) applied to a DSAI-relevant use case — the kind of data-cleaning step that precedes most analytics or ML work.

## 2. Target User
A student or analyst who wants to quickly inspect and clean a CSV without writing code — e.g., checking a dataset before importing it into Power BI or a model pipeline.

## 3. Core Features (MVP)

| # | Feature | Description |
|---|---------|-------------|
| 1 | **CSV Import** | File picker to load a `.csv` file into memory; parse headers and rows |
| 2 | **Data Grid View** | Display the dataset in a scrollable, sortable grid |
| 3 | **Summary Stats Panel** | Per-column: data type, count, missing count, min/max/mean (numeric), unique value count (categorical) |
| 4 | **Missing Value Detection** | Highlight cells/rows with nulls or blanks; show a missing-data % per column |
| 5 | **Outlier Detection** | Basic flagging for numeric columns using IQR or z-score threshold; highlight flagged rows |
| 6 | **Cleaning Operations** | Drop rows with missing values, fill missing values (mean/median/mode/custom), remove flagged outliers, trim whitespace, drop duplicate rows |
| 7 | **Export** | Save the cleaned dataset back to `.csv` |

## 4. Stretch Features (post-MVP)
- Column-level charts (histogram for numeric, bar chart for categorical value counts)
- Undo/redo for cleaning operations
- Save/load a "cleaning recipe" (sequence of operations) to reapply to another file
- Support `.xlsx` import in addition to `.csv`
- Dark/light theme toggle

## 5. Tech Stack
- **UI:** WinUI 3 (Windows App SDK 1.7+), C#
- **Data Grid:** CommunityToolkit `DataGrid` control (WinUI doesn't ship one natively)
- **Charts (stretch):** `CommunityToolkit.WinUI.Controls.DataVisualization` or Win2D
- **CSV Parsing:** `CsvHelper` NuGet package (or manual `System.IO` parsing if you want the practice)
- **Architecture:** MVVM (CommunityToolkit.Mvvm for `ObservableObject`, `RelayCommand`)

## 6. Data Model (rough sketch)
```
DatasetModel
 ├─ List<ColumnInfo>
 │    ├─ Name, DataType, MissingCount, UniqueCount
 │    └─ Min/Max/Mean/Median (if numeric)
 └─ List<DataRow> (raw + flags for missing/outlier)
```

## 7. UI Structure (rough sketch)
- **Left/top:** File load button + dataset summary (row count, column count, file name)
- **Center:** Data grid (main view)
- **Right panel:** Column inspector — click a column header, see its stats + a "clean this column" action list
- **Bottom bar:** Global actions (drop duplicates, export cleaned file)

## 8. Suggested Milestones
1. **Setup** — Create project, add NuGet packages, confirm it builds
2. **Import + Display** — Load CSV, render in data grid
3. **Stats Panel** — Compute and display per-column summary stats
4. **Missing/Outlier Detection** — Add detection logic + visual flagging
5. **Cleaning Operations** — Implement each operation, wire to UI
6. **Export** — Write cleaned data back to CSV
7. **Polish** — Stretch features, styling pass, error handling (bad CSV, empty file, etc.)

## 9. Success Criteria
- Can load a real-world messy CSV (some missing values, some outliers, a few duplicate rows) and clean it end-to-end through the UI
- No crashes on malformed input (empty file, extra commas, non-numeric in numeric column)
- Exported file is a valid, correctly cleaned CSV
