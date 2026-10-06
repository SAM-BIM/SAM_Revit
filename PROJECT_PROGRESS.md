# Project Progress - SAM_Revit (2026-Q4)

## Branch

`sow/2026-Q4` - bootstrapped 2026-10-06 from `master` `192efab4`. Frozen Q3 record: `sow/2026-Q3` @ `a3f15ce9` (not modified).

## Last updated

2026-10-06 (Q4 icon-redesign migration).

## Current status

Q4 branch cut from `master` `192efab4`, which is the exact commit pinned in SAM_Deploy's frozen Q3 baseline (`v20261006.1`). Bootstrap added only internal docs (this file, `AGENTS.md`) and a narrow CI branch-reference update (see Decisions). No product source changed. No Q4 product work has started.

## Q4 priorities

Not yet set by the owner. Record them here at the first Q4 planning pass. Known carry-over work is listed below.

## Known carry-over work

- **SAM Grasshopper icon redesign - PR #20** (`feature/sam-gh-icon-redesign` @ `fabe18af`, open, base `sow/2026-Q3`, not merged). Analysed 2026-10-06: the branch carries only its own 5 icon-only commits (`3820939`, `1d036db`, `b7a3568`, `9b4d3f1`, `fabe18a`) on top of Q3 commit `c82287af`. Those commits are not reachable from `sow/2026-Q4` (Q4 is built on the promoted `master` line), so a plain retarget would list 36 commits. Replaying exactly those commits onto `sow/2026-Q4` @ `88e7a6b9` is conflict-free (verified commit-by-commit with `git merge-tree`; identical to the net-diff merge). Planned action: rebase-onto Q4 as a new branch + PR, then close this one; owner-approved controlled task, not yet executed. **Update:** migrated; replacement Q4 PR SAM_Revit#22 (see the icon-redesign migration section); this old PR stays open for now.

## Repository-specific next steps

- Await Q4 planning. Open PRs for Q4 work against `sow/2026-Q4`.
- Follow the continuity convention in `AGENTS.md` for every PR and closeout.

## Decisions / assumptions

- Q4 base is `master` `192efab4`; the internal files were recovered from `sow/2026-Q3` into this branch only, never onto `master`.
- Q4 history intentionally does not contain the Q3 branch history (the maintained `master` is the promoted Q3 line, which is not a descendant of `sow/2026-Q3`); the frozen `sow/2026-Q3` branch is the permanent record.
- Historical Q2/Q3 content below is kept as evidence; its branch names, SHAs and next steps describe Q3 and are not current instructions.
- CI: the hard fallback list for dependency checkout now tries `sow/2026-Q4` first (then the previous Q3/Q2 entries).

## Validation

- Bootstrap verified 2026-10-06: `sow/2026-Q4` was created at exactly `192efab4` and the push was a normal (non-forced) branch creation.

## Issues / blockers

- None at bootstrap.

## Next step

- Owner to set Q4 priorities; then start the first Q4 task from this branch.

## Q4 operational cleanup (2026-10-06)

- Reviewed every active Q2/Q3 reference in this repository on `sow/2026-Q4` (workflow branch filters, dependency-branch resolution, `.gitmodules`/validation, docs). Historical Q2/Q3 mentions (feature documentation records, the frozen Q3 section below) are intentionally unchanged.
- Changed (`88e7a6b`): replaced the hard-coded fallback list `['sow/2026-Q4', 'sow/2026-Q3', 'sow/2026-Q2']` in `.github/workflows/build.yml` with a lookup of the newest `sow/YYYY-Qn` branch each dependency has (explicit head/base/canonical-quarter candidates unchanged), so a Q4 build can never fall back to the frozen Q3/Q2 lines and the next quarter needs no edit here. Resolves to `sow/2026-Q4` today.
- Checked, no action: the `github.repository_owner == 'SAM-BIM'` build guard (intentional; its comment names HoareLea only to explain why the guard exists), CODEOWNERS (SAM-BIM owners), and workflow secrets (no HoareLea-named secret). The local `upstream` (HoareLea) remote is preserved.
- Carry-over: **SAM Grasshopper icon redesign - PR #20** (`feature/sam-gh-icon-redesign` @ `fabe18af`, open, base `sow/2026-Q3`, not merged). Analysed 2026-10-06: the branch carries only its own 5 icon-only commits (`3820939`, `1d036db`, `b7a3568`, `9b4d3f1`, `fabe18a`) on top of Q3 commit `c82287af`. Those commits are not reachable from `sow/2026-Q4` (Q4 is built on the promoted `master` line), so a plain retarget would list 36 commits. Replaying exactly those commits onto `sow/2026-Q4` @ `88e7a6b9` is conflict-free (verified commit-by-commit with `git merge-tree`; identical to the net-diff merge). Planned action: rebase-onto Q4 as a new branch + PR, then close this one; owner-approved controlled task, not yet executed.
- Full cross-repository record, migration table and owner decisions: `SAM_Deploy:sow/2026-Q4` `PROJECT_PROGRESS.md`.

## Q4 icon-redesign migration (2026-10-06)

- Old PR: SAM-BIM/SAM_Revit#20 (`feature/sam-gh-icon-redesign` @ `fabe18af`, base `sow/2026-Q3`) - **preserved, open, untouched**.
- New branch `feature/sam-gh-icon-redesign-q4` cut from `sow/2026-Q4` @ `01cd7717`; new PR **SAM-BIM/SAM_Revit#22** (base `sow/2026-Q4`), feature head `40fd69ed`. **Not merged.**
- Replayed (old -> new, `cherry-pick -x`; commit set taken from the GitHub PR metadata): `3820939`->`aa7c083`, `1d036db`->`de018db`, `b7a3568`->`b163c25`, `9b4d3f1`->`914511c`, `fabe18a`->`bf56f13`; replay-only tip `bf56f13f`; plus one new docs commit `40fd69e` pointing the PR record at the new PR. No Q3 history imported.
- Verified at the replay-only tip, before the record commit: result tree identical to the net-diff merge of the old feature onto Q4 (`db9de0cf9d`); same aggregate and per-commit `git patch-id`, file set (237 files), numstat and blobs as the old PR; no workflow/`.gitmodules`/`AGENTS.md`/`PROJECT_PROGRESS.md`/solution changes. The final PR head is not tree-identical to the old feature by design (extra documentation-only commit).
- Validation: `check_source.py origin/sow/2026-Q4` OK, `check_assemblies.py` OK, local build 0 errors, relevant tests green (see the PR body); PR CI `build` success, `spdx` success; mergeable: mergeable.
- Next: owner decides whether/when to close the old PR; merge remains the maintainer's call.

---

# Historical record - 2026-Q3 (frozen)

Source: last revision of the file on `sow/2026-Q3`, commit `d6e0919` (the file was removed from the Q3 tip by `a3f15ce`; `sow/2026-Q3` tip is `a3f15ce9`). Preserved verbatim except that heading levels are shifted down one. Everything below describes Q3 and is not a current instruction.

## Project Progress

### Branch
`sow/2026-Q3`

### Last updated
2026-09-22 - app.config cleanup merged

### Current status
Part of the repo-family .NET Framework `app.config` cleanup: base [SAM#126](https://github.com/SAM-BIM/SAM/pull/126) plus 17 sibling PRs, all merged into `sow/2026-Q3` on 2026-09-22 (SAM first), with their branches deleted.

### Completed
- [SAM_Revit#18](https://github.com/SAM-BIM/SAM_Revit/pull/18) merged as `088ae869`: removed dead .NET Framework `app.config` files.

### Decisions / assumptions
- Every project here targets `netstandard2.0` or `net8.0(-windows)` and is an `OutputType Library`. Library `.dll.config` files are never read at runtime (only the host `Rhino.exe`/`Revit.exe` config is), so the net472-era binding redirects, `<supportedRuntime>` and `loadFromRemoteSources` were inert. They only emitted stale `.dll.config` files into `build/` and `%APPDATA%\SAM`.
- No `ConfigurationManager`/`AppSettings` use in the repo; deleted files held binding/runtime config only.

### Files changed
- `Grasshopper/SAM.Analytical.Grasshopper.Revit/app.config` (deleted)
- `Grasshopper/SAM.Architectural.Grasshopper.Revit/app.config` (deleted)
- `Grasshopper/SAM.Core.Grasshopper.Revit/SAM.Core.Grasshopper.Revit.csproj` (edited: dropped bare `System.IO.Compression` reference)
- `Grasshopper/SAM.Core.Grasshopper.Revit/app.config` (deleted)
- `Grasshopper/SAM.Geometry.Grasshopper.Revit/app.config` (deleted)
- `SAM_Revit/SAM.Analytical.Revit/app.config` (deleted)
- `SAM_Revit/SAM.Architectural.Revit/app.config` (deleted)
- `SAM_Revit/SAM.Core.Revit/app.config` (deleted)

### Validation
- Before merge, full `BuildAlls_v4.bat` (Debug Restore;Clean;Rebuild of every repo, starting from an emptied `%APPDATA%\SAM`): exit 0, 0 errors. The redeployed `%APPDATA%\SAM` has no `SAM.*.dll.config`.
- CI on the PR: build + spdx pass.

### Issues / blockers
- Pre-existing, unrelated: the 4 `Grasshopper/*.Revit` projects define no `Debug2027`/`Release2027` configuration.

### Next step
- None for this cleanup. Continue with the next planned task on `sow/2026-Q3`.
