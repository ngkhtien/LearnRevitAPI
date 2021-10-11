using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace LearnRevitAPI.Lib
{
    public static class ElementSelector
    {
        /// <summary>
        /// Lấy về tất cả Text Note có BoundingBox "đụng" với BoundingBox của element
        /// </summary>
        /// <param name="element"> kiểu Element</param>
        /// <param name="doc">kiểu Document</param>
        /// <returns></returns>
        public static List<TextNote> GetTextNoteIntersectWithElement(
            this Element element,
            Document doc)
        {
            BoundingBoxXYZ box = element.get_BoundingBox(doc.ActiveView);

            double tolerance = UnitUtils.Convert(50, UnitTypeId.Millimeters, UnitTypeId.Feet);

            XYZ minPoint = new XYZ(box.Min.X - tolerance, 
                box.Min.Y - tolerance, 0);

            XYZ maxPoint = new XYZ(box.Max.X + tolerance, 
                box.Max.Y + tolerance, 0);

            Outline outlineElement = new Outline(minPoint, maxPoint);

            List<TextNote> allTextNote 
                = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_TextNotes)
                .Cast<TextNote>()
                .ToList();

            List<TextNote> listTextNote = new List<TextNote>();
            foreach (TextNote text in allTextNote)
            {
                BoundingBoxXYZ box2 = text.get_BoundingBox(doc.ActiveView);
                minPoint = new XYZ(box2.Min.X, box2.Min.Y, 0);
                maxPoint = new XYZ(box2.Max.X, box2.Max.Y, 0);
                Outline outlineText = new Outline(minPoint, maxPoint);

                bool b = outlineElement.Intersects(outlineText, 0.001);
                if (b) listTextNote.Add(text);
            }

            return listTextNote;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="doc"></param>
        /// <param name="saiSo">Khoang cach giua Text va framing, don vi mm</param>
        /// <returns></returns>
        public static List<TextNote> GetTextNoteIntersectWithElement(
            this Element element,
            Document doc, double saiSo)
        {
            BoundingBoxXYZ box = element.get_BoundingBox(doc.ActiveView);

            double tolerance = UnitUtils.Convert(saiSo, UnitTypeId.Millimeters, UnitTypeId.Feet);

         XYZ minPoint = new XYZ(box.Min.X - tolerance,
                box.Min.Y - tolerance, 0);

            XYZ maxPoint = new XYZ(box.Max.X + tolerance,
                box.Max.Y + tolerance, 0);

            Outline outlineElement = new Outline(minPoint, maxPoint);

            List<TextNote> allTextNote
                = new FilteredElementCollector(doc, doc.ActiveView.Id)
                    .OfCategory(BuiltInCategory.OST_TextNotes)
                    .Cast<TextNote>()
                    .ToList();

            List<TextNote> listTextNote = new List<TextNote>();
            foreach (TextNote text in allTextNote)
            {
                BoundingBoxXYZ box2 = text.get_BoundingBox(doc.ActiveView);
                minPoint = new XYZ(box2.Min.X, box2.Min.Y, 0);
                maxPoint = new XYZ(box2.Max.X, box2.Max.Y, 0);
                Outline outlineText = new Outline(minPoint, maxPoint);

                bool b = outlineElement.Intersects(outlineText, 0.001);
                if (b) listTextNote.Add(text);
            }

            return listTextNote;
        }


        #region Dành cho revit 2019 trở lên

        internal static List<ElementId> GetAllElementsInFilter(Document doc, ElementId filterId)
        {
            if (filterId == null) return null;
            Element e = doc.GetElement(filterId);
            FilterElement filter = e as FilterElement;
            if (filter == null) return new List<ElementId>();

            List<ElementId> allElements = new List<ElementId>();

            if (filter is ParameterFilterElement)
            {
                ParameterFilterElement paFil
                    = filter as ParameterFilterElement;

                ICollection<ElementId> catIdList = paFil.GetCategories();
                ElementFilter elementFilter = paFil.GetElementFilter();

                if (elementFilter == null)
                {
                    foreach (ElementId catId in catIdList)
                    {
                        FilteredElementCollector filterCol
                            = new FilteredElementCollector(filter.Document)
                            .WhereElementIsNotElementType().OfCategoryId(catId);
                        allElements.AddRange(filterCol.ToElementIds());
                    }
                }
                else
                {
                    foreach (ElementId catId in catIdList)
                    {
                        FilteredElementCollector filterCol
                            = (new FilteredElementCollector(filter.Document))
                            .WhereElementIsNotElementType()
                            .OfCategoryId(catId)
                            .WherePasses(elementFilter);
                        allElements.AddRange(filterCol.ToElementIds());
                    }
                }
            }
            else if (filter is SelectionFilterElement)
            {
                SelectionFilterElement selFil
                    = filter as SelectionFilterElement;
                allElements = selFil.GetElementIds().ToList();
            }

            return allElements;
        }
        /// <summary>
        /// Dành cho revit 2019 trở lên
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static List<ElementId> GetAllElementsInFilter(FilterElement filter)
        {
            List<ElementId> valueList = new List<ElementId>();

            #region Code

            if (filter == null) return valueList;

            if (filter is ParameterFilterElement)
            {
                ParameterFilterElement paFil
                    = filter as ParameterFilterElement;

                ICollection<ElementId> catIdList = paFil.GetCategories();
                ElementFilter elementFilter = paFil.GetElementFilter();

                if (elementFilter == null)
                {
                    foreach (ElementId catId in catIdList)
                    {
                        FilteredElementCollector filterCol
                            = (new FilteredElementCollector(filter.Document))
                            .WhereElementIsNotElementType().OfCategoryId(catId);
                        valueList.AddRange(filterCol.ToElementIds());
                    }
                }
                else
                {
                    foreach (ElementId catId in catIdList)
                    {
                        FilteredElementCollector filterCol
                            = (new FilteredElementCollector(filter.Document))
                            .WhereElementIsNotElementType()
                            .OfCategoryId(catId)
                            .WherePasses(elementFilter);
                        valueList.AddRange(filterCol.ToElementIds());
                    }
                }
            }
            else if (filter is SelectionFilterElement)
            {
                SelectionFilterElement selFil
                    = filter as SelectionFilterElement;
                valueList = selFil.GetElementIds()
                    .ToList<ElementId>();
            }

            #endregion Dành cho revit 2019 trở lên

            return valueList;
        }


        #endregion  Dành cho revit 2019 trở lên
    }
}
