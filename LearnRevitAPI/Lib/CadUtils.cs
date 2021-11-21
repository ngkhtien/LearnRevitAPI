#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
#endregion

namespace LearnRevitAPI.Lib
{
    public static class CadUtils
    {
        /// <summary>
        /// Lấy về tất cả Solid có Faces.Count > 0 của file CAD.
        /// </summary>
        /// <param name="cadInstance"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static List<Solid> GetSolids(ImportInstance cadInstance)
        {
            List<Solid> allSolids = new List<Solid>();

            // Defaults to medium detail, no references and no view.
            Options option = new Options();
            GeometryElement geoElement = cadInstance.get_Geometry(option);

            foreach (GeometryObject geoObject in geoElement)
            {
                if (geoObject is GeometryInstance)
                {
                    GeometryInstance geoInstance = geoObject as GeometryInstance;
                    GeometryElement geoElement2 = geoInstance.GetInstanceGeometry();
                    foreach (GeometryObject geoObject2 in geoElement2)
                    {
                        if (geoObject2 is Solid)
                        {
                            Solid solid = geoObject2 as Solid;
                            if (solid.SurfaceArea > 0) allSolids.Add(solid);
                        }
                    }
                }
            }

            return allSolids;
        }

        public static List<PlanarFace> GetHatchHaveName(ImportInstance cadInstance,
            string layerName)
        {
            List<PlanarFace> allHatch = new List<PlanarFace>();

            List<Solid> solids = GetSolids(cadInstance);

            if (solids.Count == 0) return allHatch;

            foreach (Solid solid in solids)
            {
                foreach (Face face in solid.Faces)
                {
                    GraphicsStyle graphicsStyle =
                           cadInstance.Document.GetElement(face.GraphicsStyleId)
                           as GraphicsStyle;

                    if (graphicsStyle == null) continue;
                    Category styleCategory = graphicsStyle.GraphicsStyleCategory;

                    if (styleCategory.Name.Equals(layerName))
                    {
                        allHatch.Add(face as PlanarFace);
                    }
                }
            }

            return allHatch;
        }

        internal static List<string> GetAllLayer(ImportInstance cadInstance)
        {
            List<string> allLayer = new List<string>();

            Options option = new Options();
            //option.View = cadInstance.Document.ActiveView;

            GeometryElement geometryElement = cadInstance.get_Geometry(option);

            foreach (GeometryObject geoObject in geometryElement)
            {
                if (geoObject is GeometryInstance)
                {
                    GeometryInstance geoInstance = geoObject as GeometryInstance;
                    GeometryElement geometryElement2 = geoInstance.GetInstanceGeometry();

                    foreach (GeometryObject geo in geometryElement2)
                    {
                        if (geo is Solid)
                        {
                            Solid solid = geo as Solid;

                            //if (solid.Volume == 0) continue;

                            foreach (Face face in solid.Faces)
                            {
                                ElementId graphicsStyleId = face.GraphicsStyleId;

                                GraphicsStyle graphicsStyle =
                                    cadInstance.Document.GetElement(graphicsStyleId)
                                        as GraphicsStyle;

                                if (graphicsStyle == null) continue;

                                Category styleCategory = graphicsStyle.GraphicsStyleCategory;

                                allLayer.Add(styleCategory.Name);
                            }
                        }
                        else // line, polyline,...
                        {
                            ElementId graphicsStyleId =  geo.GraphicsStyleId;

                            GraphicsStyle graphicsStyle =
                                cadInstance.Document.GetElement(graphicsStyleId)
                                    as GraphicsStyle;

                            if (graphicsStyle == null) continue;

                            Category styleCategory = graphicsStyle.GraphicsStyleCategory;

                            allLayer.Add(styleCategory.Name);
                        }
                    }
                }
            }

            List<string> result = allLayer.Distinct().ToList();

            result.Sort();
            return result;
        }
    }
}