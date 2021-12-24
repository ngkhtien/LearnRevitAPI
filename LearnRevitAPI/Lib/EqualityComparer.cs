using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace LearnRevitAPI.Lib
{
    public class EqualityComparer : IEqualityComparer<ElementId>,
        IEqualityComparer<Face>, IEqualityComparer<FamilySymbol>,
        IEqualityComparer<ElementType>, IEqualityComparer<ViewType>
    {
        public bool Equals(ElementId firstInstance, ElementId secondInstance)
        {
            return firstInstance == secondInstance;
        }

        public bool Equals(Face face1, Face face2)
        {
            return face1.Area == face1.Area;
        }

        public int GetHashCode(Face obj)
        {
            return 0;
        }

        public int GetHashCode(ElementId obj)
        {
            return 0;
        }

        public int GetHashCode(Element obj)
        {
            return 0;
        }

        public bool Equals(FamilySymbol x, FamilySymbol y)
        {
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(FamilySymbol obj)
        {
            return 0;
        }

        public bool Equals(ElementType x, ElementType y)
        {
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(ElementType obj)
        {
            return 0;
        }

        public bool Equals(ViewType x, ViewType y)
        {
            return x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(ViewType obj)
        {
            return 0;
        }
    }
}