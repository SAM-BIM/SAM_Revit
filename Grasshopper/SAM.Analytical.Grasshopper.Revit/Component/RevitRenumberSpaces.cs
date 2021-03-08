﻿using Autodesk.Revit.DB;
using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Revit.Properties;
using SAM.Core;
using SAM.Core.Grasshopper.Revit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.Grasshopper.Revit
{
    public class RevitRenumberSpaces : SAMTransactionComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("d7c1384b-486d-45b2-ace0-b09deca6d14e");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Revit;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public RevitRenumberSpaces()
          : base("Revit.RenumberSpaces", "Revit.RenumberSpaces",
              "Renumber Spaces",
              "SAM", "Revit")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddParameter(new RhinoInside.Revit.GH.Parameters.SpatialElement(), "_spaces_", "_spaces_", "Revit Space", GH_ParamAccess.list);
            inputParamManager.AddBooleanParameter("_run", "_run", "Run", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddParameter(new RhinoInside.Revit.GH.Parameters.SpatialElement(), "Spaces", "Spaces", "Revit Spaces", GH_ParamAccess.list);
            outputParamManager.AddTextParameter("Numbers", "Numbers", "Numbers", GH_ParamAccess.list);
            outputParamManager.AddBooleanParameter("Successful", "Successful", "Parameters Updated", GH_ParamAccess.item);
        }

        protected override void TrySolveInstance(IGH_DataAccess dataAccess)
        {
            dataAccess.SetData(2, false);

            bool run = false;
            if (!dataAccess.GetData(1, ref run) || !run)
                return;

            List<Autodesk.Revit.DB.Mechanical.Space> spaces = new List<Autodesk.Revit.DB.Mechanical.Space>();
            if (!dataAccess.GetDataList(0, spaces))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            if(spaces == null || spaces.Count == 0)
            {
                Document document = RhinoInside.Revit.Revit.ActiveDBDocument;
                spaces = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_MEPSpaces).Cast<Autodesk.Revit.DB.Mechanical.Space>().ToList();
            }

            bool successful = false;
            List<Autodesk.Revit.DB.Mechanical.Space> result = new List<Autodesk.Revit.DB.Mechanical.Space>();
            List<string> numbers = new List<string>();

            if (spaces == null || spaces.Count == 0)
            {
                Dictionary<string, List<Autodesk.Revit.DB.Mechanical.Space>> dictionary = new Dictionary<string, List<Autodesk.Revit.DB.Mechanical.Space>>();
                foreach(Autodesk.Revit.DB.Mechanical.Space space in spaces)
                {
                    string name = space?.Level?.Name;
                    if (string.IsNullOrEmpty(name))
                        name = string.Empty;

                    if(!dictionary.TryGetValue(name, out List<Autodesk.Revit.DB.Mechanical.Space> spaces_Level))
                    {
                        spaces_Level = new List<Autodesk.Revit.DB.Mechanical.Space>();
                        dictionary[name] = spaces_Level;
                    }

                    spaces_Level.Add(space);
                }

                foreach (KeyValuePair<string, List<Autodesk.Revit.DB.Mechanical.Space>> keyValuePair in dictionary)
                {
                    XYZ xyz_Min = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);
                    foreach(Autodesk.Revit.DB.Mechanical.Space space_Temp in keyValuePair.Value)
                    {
                        XYZ xyz = (space_Temp.Location as LocationPoint).Point;
                        if (xyz == null)
                            continue;

                        xyz_Min = new XYZ(Math.Min(xyz.X, xyz_Min.X), Math.Min(xyz.Y, xyz_Min.Y), Math.Min(xyz.Z, xyz_Min.Z));
                    }

                    List<Tuple<double, Autodesk.Revit.DB.Mechanical.Space>> tuples = new List<Tuple<double, Autodesk.Revit.DB.Mechanical.Space>>();
                    foreach (Autodesk.Revit.DB.Mechanical.Space space_Temp in keyValuePair.Value)
                    {
                        XYZ xyz = (space_Temp.Location as LocationPoint).Point;
                        if (xyz == null)
                            continue;

                        tuples.Add(new Tuple<double, Autodesk.Revit.DB.Mechanical.Space>(xyz.DistanceTo(xyz_Min), space_Temp));
                    }

                    tuples.Sort((x, y) => x.Item1.CompareTo(x.Item2));

                    int count = 1;
                    string levelName = keyValuePair.Key.Replace("Level", string.Empty).Trim();
                    foreach(Autodesk.Revit.DB.Mechanical.Space space in tuples.ConvertAll(x => x.Item2))
                    {
                        string number = count.ToString();
                        if (!string.IsNullOrWhiteSpace(levelName))
                            number = string.Format("{0}_{1}", levelName, number);

                        result.Add(space);
                        numbers.Add(number);

                        space.get_Parameter(BuiltInParameter.ROOM_NUMBER).Set(number);

                        count++;
                    }
                }
            }

            dataAccess.SetDataList(0, result);
            dataAccess.SetDataList(1, numbers);
            dataAccess.SetData(2, successful);

        }
    }
}