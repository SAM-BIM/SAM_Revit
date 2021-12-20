﻿using Autodesk.Revit.DB;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Analytical.Grasshopper.Revit.Properties;
using SAM.Core.Grasshopper;
using SAM.Core.Revit;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper.Revit
{
    public class RevitSAMAnalyticalByType : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("983b8384-71c3-4243-b93b-e63400311864");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.2";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Revit;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public RevitSAMAnalyticalByType()
          : base("Revit.SAMAnalyticalByType", "Revit.SAMAnalyticalByType",
              "Convert Revit Link Instance To SAM Analytical Object ie. Panel, Construction, Aperture, ApertureConstruction, Space",
              "SAM", "Revit")
        {
        }

        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "_type_", NickName = "_type_", Description = "Type Name ie. Panel, Construction, Aperture, ApertureConstruction, Space", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "revitLinkInstance_", NickName = "revitLinkInstance_", Description = "Revit Link Instance", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));

                global::Grasshopper.Kernel.Parameters.Param_Boolean boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_run", NickName = "_run", Description = "Run", Access = GH_ParamAccess.item };
                boolean.SetPersistentData(false);
                result.Add(new GH_SAMParam(boolean, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "analyticalObjects", NickName = "analyticalObjects", Description = "SAM Analytical Objects", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">
        /// The DA object is used to retrieve from inputs and store in outputs.
        /// </param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index = -1;

            index = Params.IndexOfInputParam("_run");
            
            bool run = false;
            if (index == -1 || !dataAccess.GetData(index, ref run) || !run)
                return;

            string typeName = null;
            index = Params.IndexOfInputParam("_type_");
            if (index == -1 || !dataAccess.GetData(index, ref typeName) || string.IsNullOrWhiteSpace(typeName))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            typeName = typeName.Trim();
            
            Type type = Type.GetType(string.Format("{0},{1}", "SAM.Analytical." + typeName, "SAM.Analytical"));
            if (type == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            Document document = RhinoInside.Revit.Revit.ActiveDBDocument;
            if (document == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            Transform transform = Transform.Identity;

            GH_ObjectWrapper objectWrapper = null;

            index = Params.IndexOfInputParam("revitLinkInstance_");
            if(index != -1)
            {
                dataAccess.GetData(index, ref objectWrapper);
                if (objectWrapper != null)
                {
                    dynamic obj = objectWrapper.Value;

                    ElementId aId = obj.Id as ElementId;

                    Element element = (obj.Document as Document).GetElement(aId);
                    if (element == null || !(element is RevitLinkInstance))
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Element");
                        return;
                    }

                    RevitLinkInstance revitLinkInstance = element as RevitLinkInstance;

                    document = revitLinkInstance.GetLinkDocument();
                    transform = revitLinkInstance.GetTotalTransform();
                }
            }


            ConvertSettings convertSettings = new ConvertSettings(true, true, true);

            IEnumerable<Core.SAMObject> result = Analytical.Revit.Convert.ToSAM(document, type, convertSettings, transform);


            index = Params.IndexOfOutputParam("revitLinkInstance_");
            if(index != -1)
            {
                dataAccess.SetDataList(index, result);
            }
        }
    }
}