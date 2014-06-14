using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JAZS
{
   public class Geometry
   {
   
      public static Vector2 MidPoint (Vector2 p1, Vector2 p2)
      {
         return new Vector2(p1.x + p2.x / 2, p1.y + p2.y / 2);
      }
      
      public static void MoveToMidPoint (Vector2 move, Vector2 toward)
      {
         move = MidPoint(move, toward);
      }
      
      public static void InsertMidPoint (List<Vector2> vertices, int before, int after)
      {
         
         Vector2 midPoint = MidPoint(vertices[before], vertices[after]);
         vertices.Insert(before, midPoint);
      }
      
      public static void Inflate (Vector2[] vertices)
      {
         
      }
      
      public static void Inflate (Vector2[] vertices, Vector2 local, float weight)
      {
         
      }
   }
}
