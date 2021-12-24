#region Namespaces

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using View = Autodesk.Revit.DB.View;

#endregion

namespace LearnRevitAPI.Lib
{
    internal static class ViewUtils
    {
        /// <summary>
        /// Lấy về tất cả các Views có trong document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        internal static List<View> GetAllViews(this Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .Cast<View>()
                .Where(view => !view.IsTemplate)
                .ToList();
        }

        /// <summary>
        /// Lấy về tất cả ViewType của các View có trên doc hoặc của selectedViews nếu isCurentSelected = true
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="iscurentSelected"></param>
        /// <param name="selectedViews"></param>
        /// <returns></returns>
        internal static List<ViewType> GetAllViewsType(Document doc, 
            bool iscurentSelected,
            List<View> selectedViews)
        {
            List<ViewType> viewTypes;

            if (iscurentSelected)
            {
                viewTypes = selectedViews
                    .Where(view => !view.IsTemplate)
                    .Select(view => view.ViewType)
                    .ToList();
            }
            else
            {
                viewTypes = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Views)
                    .Cast<View>()
                    .Where(view => !view.IsTemplate)
                    .Select(view => view.ViewType)
                    .ToList();
            }
            viewTypes =
                viewTypes.Distinct(new EqualityComparer()).ToList();
            viewTypes.Sort(new AlphaComparer());
            return viewTypes;
        }

        /// <summary>
        /// Lấy về tất cả View có Type là viewType trong doc hoặc trong selectedViews nếu iscurentSelected = true
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewType"></param>
        /// <param name="iscurentSelected"></param>
        /// <param name="selectedViews"></param>
        /// <returns></returns>
        internal static List<View> GetAllViewsWithViewType(Document doc,
            ViewType viewType,
            bool iscurentSelected,
            List<View> selectedViews)
        {
            List<View> resultViews;

            if (iscurentSelected)
            {
                resultViews
                    = selectedViews
                        .Where(view
                            => view.ViewType.ToString()
                                .Equals(viewType.ToString()))
                        .ToList();
            }
            else
            {
                resultViews
                    = doc.GetAllViews()
                        .Where(view 
                            => view.ViewType.ToString()
                                .Equals(viewType.ToString()))
                        .ToList();

            }

            resultViews.Sort(new AlphaComparer());
            return resultViews;
        }
    }
}
