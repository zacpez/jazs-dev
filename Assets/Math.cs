using UnityEngine;
using System.Collections;

namespace JAZS
{
   public static class Math
   {
     public static float PlusMinus (float spread)
     {
       return(-(spread / 2) + Random.Range(0f, spread));
     }
      
      public static int PlusMinus (int spread)
     {
        return(-(spread / 2) + Random.Range(0, spread));
      }
   }
}
