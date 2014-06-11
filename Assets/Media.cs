using UnityEngine;
using System.IO;
using System.Collections;

namespace JAZS
{
   public static class Media 
   {
      public static void PostImage (string url, string key, byte[] data)
      {
         var form = new WWWForm();
         
         form.AddBinaryData(key, data, System.DateTime.UtcNow.ToString()+".png", "images/png");
         var www = new WWW(url, form);
         
         // Busy Wait
         while (!www.isDone) { }
         
         if (www.error != null) 
         {
            Debug.LogWarning(www.error);
         } 
         else 
         {
            Debug.Log(www.text);
         }
      }
   }   
}
