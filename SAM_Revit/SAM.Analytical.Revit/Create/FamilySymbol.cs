﻿using Autodesk.Revit.DB;
using SAM.Core.Revit;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.Revit
{
    public static partial class Create
    {
        public static FamilySymbol FamilySymbol(this ApertureConstruction apertureConstruction, Document document, PanelGroup panelGroup = PanelGroup.Undefined)
        {
            if(apertureConstruction == null || document == null)
            {
                return null;
            }

            List<BuiltInCategory> builtInCategories = new List<BuiltInCategory>();
            BuiltInCategory builtInCategory = apertureConstruction.BuiltInCategory();
            if (builtInCategory == BuiltInCategory.INVALID)
            {
                builtInCategories.Add(Query.BuiltInCategory(ApertureType.Door));
                builtInCategories.Add(Query.BuiltInCategory(ApertureType.Window));
            }
            else
            {
                builtInCategories.Add(builtInCategory);
            }

            List<Family> families = new FilteredElementCollector(document).OfClass(typeof(Family)).Cast<Family>().ToList();
            if(families == null || families.Count == 0)
            {
                return null;
            }

            HashSet<FamilyHostingBehavior> familyHostingBehaviors = Query.FamilyHostingBehaviors(panelGroup);

            List<Family> families_Filtered = new List<Family>();
            foreach (Family family in families)
            {
                if(family == null)
                {
                    continue;
                }

                if (!builtInCategories.Contains(family.FamilyCategory.BuiltInCategory))
                {
                    continue;
                }

                if(family.FamilyPlacementType != FamilyPlacementType.OneLevelBasedHosted)
                {
                    continue;
                }

                if(familyHostingBehaviors != null && familyHostingBehaviors.Count != 0)
                {
                    Parameter parameter = family.get_Parameter(BuiltInParameter.FAMILY_HOSTING_BEHAVIOR);
                    if (parameter != null && parameter.HasValue)
                    {
                        FamilyHostingBehavior familyHostingBehavior = (FamilyHostingBehavior)parameter.AsInteger();
                        if(!familyHostingBehaviors.Contains(familyHostingBehavior))
                        {
                            continue;
                        }
                    }
                }

                families_Filtered.Add(family);
            }

            Dictionary<Family, List<FamilySymbol>> dictionary = new Dictionary<Family, List<FamilySymbol>>();
            foreach(Family family in families_Filtered)
            {
                IEnumerable<ElementId> elementIds = family.GetFamilySymbolIds();
                if(elementIds == null || elementIds.Count() == 0)
                {
                    continue;
                }

                List<FamilySymbol> familySymbols = new List<FamilySymbol>();

                foreach(ElementId elementId in elementIds)
                {
                    FamilySymbol familySymbol = document.GetElement(elementId) as FamilySymbol;
                    if(familySymbol == null)
                    {
                        continue;
                    }

                    familySymbols.Add(familySymbol);
                }

                if(familySymbols == null || familySymbols.Count == 0)
                {
                    continue;
                }

                dictionary[family] = familySymbols;
            }

            if(dictionary.Count == 0)
            {
                return null;
            }

            string fullName = apertureConstruction.FullName();
            foreach(KeyValuePair<Family, List<FamilySymbol>> keyValuePair in dictionary)
            {
                foreach(FamilySymbol familySymbol in keyValuePair.Value)
                {
                    if(familySymbol.FullName() == fullName)
                    {
                        return familySymbol;
                    }
                }
            }

            string name = apertureConstruction.Name;
            foreach (KeyValuePair<Family, List<FamilySymbol>> keyValuePair in dictionary)
            {
                foreach (FamilySymbol familySymbol in keyValuePair.Value)
                {
                    if (familySymbol.Name == name)
                    {
                        return familySymbol;
                    }
                }
            }

            FamilySymbol result = null;
            foreach (KeyValuePair<Family, List<FamilySymbol>> keyValuePair in dictionary)
            {
                foreach (FamilySymbol familySymbol in keyValuePair.Value)
                {
                    if (familySymbol.Name.EndsWith(name))
                    {
                        result = familySymbol;
                        break;
                    }
                }

                if(result != null)
                {
                    break;
                }
            }

            foreach (KeyValuePair<Family, List<FamilySymbol>> keyValuePair in dictionary)
            {
                foreach (FamilySymbol familySymbol in keyValuePair.Value)
                {
                    if (familySymbol != null)
                    {
                        result = familySymbol;
                        break;
                    }
                }

                if (result != null)
                {
                    break;
                }
            }

            if (result == null)
            {
                return null;
            }

            result = result.Duplicate(apertureConstruction.Name) as FamilySymbol;
            if (!result.IsActive)
            {
                result.Activate();
            }

            return result;
        }
    }
}