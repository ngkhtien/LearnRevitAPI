#region Namespaces
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.DB;
#endregion

namespace LearnRevitAPI.Lib
{
    public static class FamilySymbolUtils
    {
        public static FamilySymbol GetFamilySymbolFraming(Family family, double b, double h)
        {
            List<FamilySymbol> allFamilySymbol = family.GetAllFamilySymbol();

            foreach (var familySymbol in allFamilySymbol)
            {
                Parameter bParameter = familySymbol.LookupParameter("b");
                Parameter hParameter = familySymbol.LookupParameter("h");
                double bvalue = bParameter.AsDouble();
                double hvalue = hParameter.AsDouble();

                if (bvalue == b && hvalue == h)
                {
                    return familySymbol;
                }
            }


            // làm tròn đến hàng đơn vị, ví dụ: 2995.5 -> 2996
            double sectionX = Math.Round(UnitUtils.Convert(b, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS), 0);
            double sectionY = Math.Round(UnitUtils.Convert(h, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS), 0);
            string name = string.Concat(sectionX, "x", sectionY);

            FamilySymbol result = null;
            using (Transaction tx = new Transaction(family.Document))
            {
                tx.Start("Create new Framing Type");
                ElementType s1 = allFamilySymbol[0].Duplicate(name);
                s1.LookupParameter("b").Set(b);
                s1.LookupParameter("h").Set(h);

                result = s1 as FamilySymbol;
                tx.Commit();
            }
            return result;
        }

        /// <summary>
        /// Lấy về hoặc tạo ra một FamilySymbol của một Family Cột, với kích thước b,h
        /// </summary>
        /// <param name="family"></param>
        /// <param name="b">don vi la feet</param>
        /// <param name="h">don vi la feet</param>
        /// <param name="bPara"></param>
        /// <param name="hPara"></param>
        /// <returns></returns>
        public static FamilySymbol GetFamilySymbolColumn(Family family, double b, double h,
            string bPara, string hPara)
        {
            List<FamilySymbol> allFamilySymbol = family.GetAllFamilySymbol();

            Parameter bParameter = allFamilySymbol[0].LookupParameter(bPara);
            Parameter hParameter = allFamilySymbol[0].LookupParameter(hPara);

            if (bParameter == null || hParameter == null)
            {
                MessageBox.Show("Two parameters dimemsion of column family have to name is "
                                + bPara +" & "+ hPara);
                return null;
            }

            // Tìm trong list type của Column đã có type với kích thước b, h chưa.
            // Nếu đã có thì lấy về type đó. chưa có thì tạo mới
            foreach (FamilySymbol symbol in allFamilySymbol)
            {
                bParameter = symbol.LookupParameter(bPara);
                hParameter = symbol.LookupParameter(hPara);

                double bvalue = bParameter.AsDouble();
                double hvalue = hParameter.AsDouble();

                if (Math.Abs(bvalue - b) < 0.001 && Math.Abs(hvalue - h) < 0.001)
                {
                    return symbol;
                }
            }

            // làm tròn đến hàng đơn vị, ví dụ: 2995.5 -> 2996
            //double sectionX = Math.Round(DLQUnitUtils.FeetToMm(b), 0);
            //double sectionY = Math.Round(DLQUnitUtils.FeetToMm(h), 0);
            //string name = string.Concat(sectionX, "x", sectionY);

            string newName = string.Concat(Math.Round(UnitUtils.Convert(b, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS), 0).ToString(),
                "x",Math.Round(UnitUtils.Convert(h, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS), 0).ToString());

            // 300x600
            
            //if (name.Equals("0x0")) return null;
            //if (name.Equals("0x0") || Math.Abs(sectionX) < 0.01 ||
            //    Math.Abs(sectionY) < 0.01) return null;

            FamilySymbol result = null;
            using (Transaction tx = new Transaction(family.Document))
            {
                tx.Start("Create new Column Type");

                ElementType s1 = allFamilySymbol[0].Duplicate(newName);

                s1.LookupParameter(bPara).Set(b);
                s1.LookupParameter(hPara).Set(h);

                result = s1 as FamilySymbol;

                tx.Commit();
            }

            return result;
        }
        public static List<FamilySymbol> GetAllFamilySymbol(this Family family)
        {
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();

            foreach (ElementId familySymbolId in family.GetFamilySymbolIds())
            {
                FamilySymbol familySymbol = family.Document.GetElement(familySymbolId) as FamilySymbol;
                familySymbols.Add(familySymbol);
            }

            return familySymbols;
        }
    }
}