// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using Autodesk.Revit.DB;

namespace SAM.Core.Revit
{
    public static partial class Query
    {
        public static ForgeTypeId ForgeTypeId(this string text)
        {
            if(string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            switch(text)
            {
                case "HVACAirflow":
                    return SpecTypeId.AirFlow;

                case "URL":
                    return SpecTypeId.String.Url;

                case "Text":
                    return SpecTypeId.String.Text;

                case "YesNo":
                    return SpecTypeId.Boolean.YesNo;

                case "Integer":
                    return SpecTypeId.Int.Integer;

                case "Number":
                    return SpecTypeId.Number;

                case "HVACCoefficientOfHeatTransfer":
                    return SpecTypeId.HeatTransferCoefficient;

                case "Volume":
                    return SpecTypeId.Volume;

                case "HVACTemperature":
                    return SpecTypeId.HvacTemperature;

                case "HVACTemperatureDifference":
                    return SpecTypeId.HvacTemperatureDifference;

                case "HVACHeatGain":
                    return SpecTypeId.HeatGain;

                case "HVACCoolingLoad":
                    return SpecTypeId.CoolingLoad;

                case "HVACHeatingLoad":
                    return SpecTypeId.HeatingLoad;

                case "Area":
                    return SpecTypeId.Area;

                case "HVACCoolingLoadDividedByArea":
                    return SpecTypeId.CoolingLoadDividedByArea;

                case "HVACHeatingLoadDividedByArea":
                    return SpecTypeId.HeatingLoadDividedByArea;
            }

            throw new System.NotImplementedException();
        }


    }
}