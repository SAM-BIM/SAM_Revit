﻿using Autodesk.Revit.DB;
using SAM.Core.Revit;
using SAM.Geometry.Revit;
using System.Collections.Generic;

namespace SAM.Analytical.Revit
{
    public static partial class Convert
    {
        public static ApertureConstruction ToSAM_ApertureConstruction(this FamilyInstance familyInstance, Core.Revit.ConvertSettings convertSettings)
        {
            if (familyInstance == null)
                return null;

            FamilySymbol familySymbol = familyInstance.Document.GetElement(familyInstance.GetTypeId()) as FamilySymbol;
            if (familySymbol == null)
                return null;

            return ToSAM_ApertureConstruction(familySymbol, convertSettings);
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this FamilySymbol familySymbol, Core.Revit.ConvertSettings convertSettings)
        {
            if (familySymbol == null)
                return null;

            ApertureConstruction result = convertSettings?.GetObject<ApertureConstruction>(familySymbol.Id);
            if (result != null)
                return result;

            string name = familySymbol.FullName();

            ApertureType apertureType = familySymbol.ApertureType();

            List<ApertureConstruction> apertureConstructions = Analytical.Query.DefaultApertureConstructionLibrary().GetApertureConstructions(apertureType);
            if (apertureConstructions != null)
                result = apertureConstructions.Find(x => name.Equals(x.UniqueName()) || name.Equals(x.Name));

            if (result == null)
                result = new ApertureConstruction(name, apertureType);

            result.UpdateParameterSets(familySymbol, ActiveSetting.Setting.GetValue<Core.MapCluster>(Core.Revit.ActiveSetting.Name.ParameterMap));

            //Core.ParameterSet parameterSet = Core.Revit.Query.ParameterSet(familySymbol);
            //parameterSet.Add(Analytical.Query.ParameterName_Type(), Analytical.Query.Text(result.ApertureType));
            //result.Add(parameterSet);
            //result.SetValue(AnalyticalParameter.TypeName, Analytical.Query.Text(result.ApertureType));

            convertSettings?.Add(familySymbol.Id, result);

            return result;
        }
    }
}