using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace LearnRevitAPI._07_ModelFromCAD
{
   public class ColumnData
   {
      public ColumnData(PlanarFace planarFace)
      {
         Curve curveCanhNgan;

         List<Curve> allCurves = new List<Curve>();

         EdgeArrayArray edgeLoops = planarFace.EdgeLoops;

         foreach (EdgeArray edgeArray in edgeLoops)
         {
            foreach (Edge edge in edgeArray)
            {
               allCurves.Add(edge.AsCurve());
            }
         }


         if (allCurves.Count == 4)
         {
            if (allCurves[0].Length > allCurves[1].Length)
            {
               CanhNgan = allCurves[1].Length;
               CanhDai = allCurves[0].Length;
               curveCanhNgan = allCurves[1];
            }
            else
            {
               CanhNgan = allCurves[0].Length;
               CanhDai = allCurves[1].Length;
               curveCanhNgan = allCurves[0];
            }

            // Tọa độ 4 đỉnh của PlannarFace có 4 cạnh
            IList<XYZ> vertices = planarFace.Triangulate().Vertices;
            TamCot = vertices[0].Add(vertices[2]) / 2;

            XYZ huongCanhNgan = curveCanhNgan.GetEndPoint(1)
                .Subtract(curveCanhNgan.GetEndPoint(0))
                .Normalize();

            GocXoay = XYZ.BasisX.AngleTo(huongCanhNgan);
         }
      }

      /// <summary>
      /// đơn vị: feet
      /// </summary>
      public double CanhNgan { get; set; }
      public double CanhDai { get; set; }
      public XYZ TamCot { get; set; }

      /// <summary>
      /// Góc giữa BasicX với cạnh ngắn, đơn vị radian.
      /// Tính chất góc giữa 2 vecto: 0 <= GocXoay <= 90
      /// </summary>
      public double GocXoay { get; set; }
   }
}
