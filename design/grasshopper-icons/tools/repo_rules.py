"""Repository-specific classification decisions for SAM_Revit (the only non-shared tool file).

OVERRIDES     : component display name -> (object glyph, op, extra)   extra: None | "plural" | "library" | note
PARAM_OBJECTS : param type key (Goo<X>Param class or typeof(X) name) -> glyph | (glyph, container, plural)
OBJECTS/VERBS : extra noun/verb rules tried before the shared ones (same shapes as SAM's OBJECTS/VERBS)
"""
# Revit -> SAM = import, SAM -> Revit = export. Revit walls = SAM `panel`; views/sheets = ext `view`; elements/ids = ext `element`.
OVERRIDES = {
    "Revit.AlignWalls": ("panel", "align", "plural"), "Revit.OverlapWalls": ("panel", "intersect", None),
    "SAMAnalytical.TrimOrExtendWall": ("panel", "extend", None), "SAMCore.GetWalls": ("panel", "get", "plural"),
    "SAMCore.WallKind": ("panel", "value", None),
    "Revit.MaterialLibrary": ("material", "get", "library"),
    "Revit.RenumberSpaces": ("space", "sort", "plural"),
    "Revit.SAMAnalyticalByElement": ("object", "import", None), "Revit.SAMAnalyticalByType": ("object", "import", None),
    "Revit.SAMAnalyticalByView": ("object", "import", None), "Revit.SAMAnalyticalModel": ("model", "import", None),
    "Revit.PanelsByCurtainWall": ("panel", "import", "plural"),
    "Revit.FloorsAndRoofsFromSpaces": ("panel", "get", "plural"), "Revit.PanelsFromSpaces": ("panel", "get", "plural"),
    "Revit.SetUpperLimit": ("level", "set", "upper limit of a space or wall"), "Revit.SpaceSnapUpperLimit": ("space", "snap", None),
    "SAMAdjacencyCluster.Revit": ("cluster", "export", None), "SAMAnalytical.ApertureRevit": ("aperture", "export", None),
    "SAMAnalytical.PanelRevit": ("panel", "export", None), "SAMAnalytical.SpaceRevit": ("space", "export", None),
    "SAMAnalytical.ResultsRevit": ("result", "export", None), "SAMAnalytical.Revit": ("object", "export", None),
    "SAMLevel.Revit": ("level", "export", None),
    "SAMAnalytical.PanelsByBoundaries": ("panel", "create", "plural"), "SAMAnalytical.RevitCheck": ("model", "validate", None),
    "SAMAnalytical.ShellsBySpaces": ("shell", "get", "plural"), "SAMAnalytical.Tool": ("settings", "value", "analytical tool"),
    "SAMArchitectural.LevelDispatchExisting": ("level", "filter", None), "SAMArchitectural.LevelInformation": ("level", "get", None),
    "Revit.SAMCoreDesignOption": ("case", "get", "design option = model variant"),
    "SAMCore.DuplicatedElementIds": ("element", "inspect", None), "SAMCore.DuplicatedUniqueIds": ("element", "inspect", None),
    "SAMCore.ElementsByScopeBox": ("element", "get", None), "SAMCore.IsPlaced": ("type", "validate", None),
}
PARAM_OBJECTS = {"ConvertSettings": "settings", "GooConvertSettings": "settings"}
OBJECTS = []
VERBS = []
