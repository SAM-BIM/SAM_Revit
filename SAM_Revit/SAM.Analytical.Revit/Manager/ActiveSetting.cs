﻿using Autodesk.Revit.DB;
using SAM.Core;
using System.Reflection;

namespace SAM.Analytical.Revit
{
    public static partial class ActiveSetting
    {
        public static class Name
        {
            public const string FileName_DefaultAirConstructionLibrary = "FileName_DefaultAirConstructionLibrary";
            public const string Library_DefaultAirConstructionLibrary = "Library_DefaultAirConstructionLibrary";
        }

        private static Setting setting = Load();

        private static Setting Load()
        {
            Setting setting = ActiveManager.GetSetting(Assembly.GetExecutingAssembly());
            if (setting == null)
                setting = GetDefault();

            return setting;
        }

        public static Setting Setting
        {
            get
            {
                return setting;
            }
        }

        public static Setting GetDefault()
        {
            Setting result = new Setting(Assembly.GetExecutingAssembly());

            TypeMap typeMap = new TypeMap();

            //AnalyticalModel
            typeMap.Add(AnalyticalModelParameter.NorthAngle, typeof(ProjectInfo), "SAM_NorthAngle");

            //Aperture
            typeMap.Add(typeof(Aperture), typeof(FamilyInstance), "GetWidth", "SAM_BuildingElementWidth");
            typeMap.Add(typeof(Aperture), typeof(FamilyInstance), "GetHeight", "SAM_BuildingElementHeight");
            typeMap.Add(typeof(Aperture), typeof(FamilyInstance), "Guid", "SAM_GUID");

            //ApertureConstruction
            typeMap.Add(ApertureConstructionParameter.Color, typeof(FamilySymbol), "SAM_BuildingElementColor");
            typeMap.Add(ApertureConstructionParameter.DefaultPanelType, typeof(FamilySymbol), "SAM_BuildingElementType");
            typeMap.Add(ApertureConstructionParameter.Transparent, typeof(FamilySymbol), "SAM_BuildingElementTransparent");
            typeMap.Add(ApertureConstructionParameter.Description, typeof(FamilySymbol), "SAM_BuildingElementDescription");
            typeMap.Add(ApertureConstructionParameter.DefaultFrameWidth, typeof(FamilySymbol), "SAM_BuildingElementFrameWidth");
            typeMap.Add(ApertureConstructionParameter.IsInternalShadow, typeof(FamilySymbol), "SAM_BuildingElementInternalShadows");

            //Construction
            typeMap.Add(ConstructionParameter.Color, typeof(HostObjAttributes), "SAM_BuildingElementColor");
            typeMap.Add(ConstructionParameter.DefaultPanelType, typeof(HostObjAttributes), "SAM_BuildingElementType");
            typeMap.Add(ConstructionParameter.Description, typeof(HostObjAttributes), "SAM_BuildingElementDescription");
            typeMap.Add(ConstructionParameter.IsAir, typeof(HostObjAttributes), "SAM_BuildingElementAir");
            typeMap.Add(ConstructionParameter.IsInternalShadow, typeof(HostObjAttributes), "SAM_BuildingElementInternalShadows");
            typeMap.Add(ConstructionParameter.IsGround, typeof(HostObjAttributes), "SAM_BuildingElementGround");
            typeMap.Add(ConstructionParameter.Transparent, typeof(HostObjAttributes), "SAM_BuildingElementTransparent");
            typeMap.Add(ConstructionParameter.DefaultThickness, typeof(HostObjAttributes), "SAM_BuildingElementThickness");

            //InternalCondition
            typeMap.Add(InternalConditionParameter.OccupancyProfileName, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_OccupacyProfile");
            typeMap.Add(InternalConditionParameter.OccupancySensibleGain, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_OccupantSensGain");
            typeMap.Add(InternalConditionParameter.OccupancyLatentGain, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_OccupantLatGain");
            typeMap.Add(InternalConditionParameter.EquipmentSensibleProfileName, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_SmallPowerSensProfile");
            typeMap.Add(InternalConditionParameter.EquipmentLatentProfileName, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_SmallPowerLatProfile");
            typeMap.Add(InternalConditionParameter.LightingGain, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_GenLightingGain");
            typeMap.Add(InternalConditionParameter.LightingLevel, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_DesignLuxLevel");
            typeMap.Add(typeof(InternalCondition), typeof(Autodesk.Revit.DB.Mechanical.Space), "Name", "SAM_IC_ThermalTemplate");

            //Space
            typeMap.Add(SpaceParameter.Area, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_Area");
            typeMap.Add(SpaceParameter.Occupancy, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_NoPeople");
            typeMap.Add(SpaceParameter.Volume, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_Volume");
            typeMap.Add(SpaceParameter.FacingExternal, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_FacingExternal");
            typeMap.Add(SpaceParameter.FacingExternalGlazing, typeof(Autodesk.Revit.DB.Mechanical.Space), "SAM_FacingExternalGlazing");
            typeMap.Add(typeof(Space), typeof(Autodesk.Revit.DB.Mechanical.Space), "Name", "SAM_SpaceName");

            //Panel
            typeMap.Add(PanelParameter.Transparent, typeof(HostObject), "SAM_BuildingElementTransparent");
            //mapCluster.Add(typeof(Panel), typeof(HostObject), null, "SAM_NorthAngle"); //double
            //mapCluster.Add(typeof(Panel), typeof(HostObject), null, "SAM_FacingExternal"); //double
            //mapCluster.Add(typeof(Panel), typeof(HostObject), null, "SAM_FacingExternalGlazing"); //double

            //Material
            typeMap.Add(typeof(Core.Material), typeof(FamilyInstance), "ThermalConductivity", "SAM_Material_Conductivity");
            typeMap.Add(typeof(Core.Material), typeof(FamilyInstance), "SpecificHeatCapacity", "SAM_Material_SpecificHeat");
            typeMap.Add(typeof(Core.Material), typeof(FamilyInstance), "Density", "SAM_Material_Density");
            typeMap.Add(typeof(Core.Material), typeof(FamilyInstance), "Description", "SAM_Material_Description");
            typeMap.Add(typeof(Core.Material), typeof(FamilyInstance), "Name", "SAM_Material_Name");
            typeMap.Add(MaterialParameter.TypeName, typeof(FamilyInstance), "SAM_Material_Type");
            typeMap.Add(MaterialParameter.DefaultThickness, typeof(FamilyInstance), "SAM_Material_Width");
            typeMap.Add(MaterialParameter.VapourDiffusionFactor, typeof(FamilyInstance), "SAM_Material_VapourDiffusionFactor");

            //GasMaterial
            typeMap.Add(GasMaterialParameter.HeatTransferCoefficient, typeof(FamilyInstance), "SAM_Material_ConvectionCoefficient");

            //TransparentMaterial
            typeMap.Add(TransparentMaterialParameter.ExternalEmissivity, typeof(FamilyInstance), "SAM_Material_ExternalEmissivity");
            typeMap.Add(TransparentMaterialParameter.ExternalLightReflectance, typeof(FamilyInstance), "SAM_Material_ExternalLightReflectance");
            typeMap.Add(TransparentMaterialParameter.ExternalSolarReflectance, typeof(FamilyInstance), "SAM_Material_ExternalSolarReflectance");
            typeMap.Add(TransparentMaterialParameter.InternalEmissivity, typeof(FamilyInstance), "SAM_Material_InternalEmissivity");
            typeMap.Add(TransparentMaterialParameter.InternalLightReflectance, typeof(FamilyInstance), "SAM_Material_InternalLightReflectance");
            typeMap.Add(TransparentMaterialParameter.InternalSolarReflectance, typeof(FamilyInstance), "SAM_Material_InternalSolarReflectance");
            typeMap.Add(TransparentMaterialParameter.IsBlind, typeof(FamilyInstance), "SAM_Material_IsBlind");
            typeMap.Add(TransparentMaterialParameter.LightTransmittance, typeof(FamilyInstance), "SAM_Material_LightTransmittance");
            typeMap.Add(TransparentMaterialParameter.SolarTransmittance, typeof(FamilyInstance), "SAM_Material_SolarTransmittance");

            //OpaqueMaterial
            typeMap.Add(OpaqueMaterialParameter.ExternalEmissivity, typeof(FamilyInstance), "SAM_Material_ExternalEmissivity");
            typeMap.Add(OpaqueMaterialParameter.ExternalLightReflectance, typeof(FamilyInstance), "SAM_Material_ExternalLightReflectance");
            typeMap.Add(OpaqueMaterialParameter.ExternalSolarReflectance, typeof(FamilyInstance), "SAM_Material_ExternalSolarReflectance");
            typeMap.Add(OpaqueMaterialParameter.IgnoreThermalTransmittanceCalculations, typeof(FamilyInstance), "SAM_Material_IngnoreInUvalue");
            typeMap.Add(OpaqueMaterialParameter.InternalEmissivity, typeof(FamilyInstance), "SAM_Material_InternalEmissivity");
            typeMap.Add(OpaqueMaterialParameter.InternalLightReflectance, typeof(FamilyInstance), "SAM_Material_InternalLightReflectance");
            typeMap.Add(OpaqueMaterialParameter.InternalSolarReflectance, typeof(FamilyInstance), "SAM_Material_InternalSolarReflectance");


            result.Add(Core.Revit.ActiveSetting.Name.ParameterMap, typeMap);

            //File Names
            result.Add(Name.FileName_DefaultAirConstructionLibrary, "SAM_AirConstructionLibrary.JSON");

            string path;

            path = Query.DefaultAirConstructionLibraryPath(result);
            if (System.IO.File.Exists(path))
                result.Add(Name.Library_DefaultAirConstructionLibrary, Core.Create.IJSAMObject<ConstructionLibrary>(System.IO.File.ReadAllText(path)));

            return result;
        }
    }
}