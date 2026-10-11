# SAM Grasshopper icon redesign — SAM_Revit PR record

Branch `feature/sam-gh-icon-redesign-q4`, based on `sow/2026-Q4` @ `01cd7717`. PR: SAM-BIM/SAM_Revit#22. Q4 migration of SAM-BIM/SAM_Revit#20 (`feature/sam-gh-icon-redesign` @ `fabe18a`, based on `sow/2026-Q3` @ `c82287a`, kept open for provenance): the same commits replayed onto `sow/2026-Q4`; Q3 history was not imported.
Propagates the SAM icon design system from SAM-BIM/SAM#166 (head `cf4d924a`, open, not merged) to this repository.

## Current status
All **56** Grasshopper objects in this repo (55 components + 1 params) use redesigned icons: **56 / 56**.
Built and validated; ready for review. **Not merged.**

## Work completed
- `design/grasshopper-icons/`: the shared SAM-BIM icon kit. `icons.py`, `render.py`, `sam_classify.py` and `ICON_DESIGN_SYSTEM.md` are vendored **verbatim** from SAM#166 (hash-checked). `icons_ext.py` and `ICON_DESIGN_SYSTEM_EXT.md` are the frozen SAM-BIM extension v1 (identical in every SAM-BIM repo). `tools/repo_rules.py` holds this repo's explicit decisions.
- **Inventory**: `tools/inventory.py` parses C# source (every non-abstract class declaring `ComponentGuid`).
- **Manifest** (source of truth): `manifest.json` / `manifest.csv` — per object: GUID, class, source, project, object glyph, operation, modifiers, icon id, resource, glyph/badge origin.
- **Generation**: 49 canonical SVGs → 24×24 PNGs; review sheet `review/contact_sheet.png` (native 24 px on GH normal / orange-warning / dark bodies + 3×) and `review/REVIEW.md`.
- **Integration**: each project's existing mechanism; only the icon token inside each `Icon` getter changes.

| Project | Objects | Icon resources | Mechanism |
|---|---|---|---|
| `SAM.Analytical.Grasshopper.Revit` | 32 | 29 | resx / Bitmap |
| `SAM.Architectural.Grasshopper.Revit` | 3 | 3 | resx / Bitmap |
| `SAM.Core.Grasshopper.Revit` | 21 | 20 | resx / Bitmap |

## Design reuse
- **Reused SAM object families (17)**: `aperture`, `case`, `cluster`, `construction`, `level`, `location`, `material`, `model`, `object`, `panel`, `result`, `settings`, `shell`, `space`, `type`, `value`, `zone`
- **New SAM-BIM ext v1 families used (2)**: `element`, `view`
- **Verbs**: `align`, `copy`, `create`, `display`, `export`, `extend`, `filter`, `get`, `import`, `inspect`, `intersect`, `modify`, `remove`, `set`, `snap`, `sort`, `update`, `validate`, `value` (all SAM)
- Distinct icons: **49** (40 on SAM glyphs, 9 on ext glyphs). Icon ids shared with SAM render pixel-identically to SAM's.

## Decisions and assumptions
- Grammar, palette, badge families and construction rules are unchanged (SAM#166). No text, no new colours.
- Qualifier variants (`…By<X>`) share an icon intentionally (see `review/REVIEW.md`).
- Interop direction: external → SAM = import ↓, SAM → external = export ↑.
- Revit → SAM conversions are imports ↓, SAM → Revit conversions are exports ↑; the object is the SAM object converted (model, panel, space, aperture, level, cluster, result, generic object).
- Revit walls use SAM `panel`; views and sheets use ext `view`; Revit elements and element ids use ext `element`; design options use SAM `case` (model variants).
- `Revit.RenumberSpaces` = space sort (distinct from `RenameSpaces` = space modify).
- Legacy icon resources are kept (still referenced by context menus / AssemblyInfo); no GUID, name, nickname, category, subcategory, parameter or behaviour change.

## Files changed
- New: `design/grasshopper-icons/**`, `<project>/Resources/Icons/SAM_GH_*.png`, `docs/GH-IconRedesign.md`.
- Modified: 56 component/param `.cs` files (one icon token each), 3× `Resources.resx`, 3× `Resources.Designer.cs`. No csproj change.

## Validation
| Check | Result |
|---|---|
| `tools/classify.py` | 56 classified, 0 unclassified |
| `tools/build.py` identical-pixel collision check | 0 groups (49 distinct icons; 5 intentionally shared icon(s) for qualifier variants, listed in `review/REVIEW.md`) |
| Icon ids shared with SAM#166 vs SAM's `png/24` | 16 shared, 16 byte-identical |
| `tools/integrate.py` re-parse | 56/56 objects reference their `SAM_GH_*` resource; every PNG exists |
| `tools/check_source.py` vs `origin/sow/2026-Q3` | vendored files OK; icon-token swaps: 56, non-icon changes: 0; base 59, now 59 -> UNCHANGED |
| `dotnet build SAM_Revit.sln -c Debug2026 -c Debug` | Build succeeded, 0 errors (Debug2026 / Revit 2026 configuration) |
| `tools/check_assemblies.py` | every assembly embeds every required 24×24 icon → OK |
| `tests/GhIconTest` (real Rhino 8 / Grasshopper, Rhino.Testing) | 56/56 verified offline from the built assemblies (`SAM_ICON_ASSEMBLY_DIR`): resource pixels = manifest PNG (max diff 0). Revit GH plugins load only in Rhino.Inside.Revit, so plain Rhino cannot emit them by GUID; GUID → resource is proven by the source re-parse |
| Repository test projects | none in this repository |
| Visual review (`review/contact_sheet.png`, 24 px on normal / warning / dark bodies) | all icons legible; no collisions |

## Unresolved issues / risks
- No live Rhino.Inside.Revit session was run; icons are verified at resource level (built assemblies) plus the source re-parse.
- Built against sibling repos as checked out locally (SAM on `feature/sam-gh-icon-redesign` = SAM#166); icon changes are API-neutral.

## Recommended next step
Review this PR (compare `review/contact_sheet.png`), then merge by the maintainer. After merge, add the `PROJECT_PROGRESS.md` closeout entry on `sow/2026-Q4` with the merge SHA. SAM#166 (the reference design system) remains open.

## SPDX header policy (CI `spdx-check`)
The repository SPDX check requires the LGPL-3.0-or-later SPDX line and the copyright line in every `.cs` file a PR changes. The icon-token swaps touched 37 older files that predated the policy (components, `Resources.Designer.cs`), and the kit test `IconTests.cs` had no header. The standard 2-line header was added to them; nothing else changed. `tools/check_source.py` accepts exactly this header as the only non-icon addition and compares against the merge base.

## Q4 migration validation
Re-validated on `sow/2026-Q4` @ `01cd7717`: `design/grasshopper-icons/tools/check_source.py origin/sow/2026-Q4` OK (icon-token swaps: 56, SPDX headers added: 34, non-icon changes: 0, ComponentGuid declarations unchanged); `msbuild SAM_Revit.sln` Debug2025, Debug2026 and Debug2027 (Visual Studio MSBuild) succeeded with 0 errors; `check_assemblies.py build` OK; tests: no test project in this repository. The Q4 feature diff (before this record commit) has the same patch-id, file set and blobs as the Q3 PR's feature diff.
