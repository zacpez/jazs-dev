using UnityEngine;
using System.Collections;

namespace JAZS
{
   public static class Math
   {
	  public static float plusminus (float spread)
	  {
		 return(-(spread / 2) + Random.Range(0f, spread));
	  }
	   
	  public static int plusminus (int spread)
	  {
	     return(-(spread / 2) + Random.Range(0, spread));
      }
   }
}
