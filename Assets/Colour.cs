using UnityEngine;
using System.Collections;

namespace JAZS
{
   public static class Colour
   {
      const float quantum = 100f;
      public enum MixType {
         average,
         subtractive,
         additive,
         multiply,
         divide
      }
      
      public static Color quantize (Color input)
      {
         input.r = (input.r * quantum) / quantum;
         input.g = (input.g * quantum) / quantum;
         input.b = (input.b * quantum) / quantum;
         input.a = (input.a * quantum) / quantum;
         return input;
      }
            
      public static Color randomColour (bool alpha)
      {
         Color result;
         switch(alpha)
         {
            case true:
               result = new Color(
                  Random.Range(0f,1f),
                  Random.Range(0f,1f),
                  Random.Range(0f,1f),
                  Random.Range(0f,1f)
               );
               break;
            
            case false:
            default:
               result = new Color(
                  Random.Range(0f,1f),
                  Random.Range(0f,1f),
                  Random.Range(0f,1f),
                  1f
               );
               break;
            
         }
         return quantize(result);
         
      }
      
      public static Color randomColour (float r, float g, float b, float a)
      {
         Color result = new Color(r,g,b,a);
         return quantize(result);
      }
      
      public static Color Lerp (Color c1, Color c2, float time)
      {
         return quantize(Color.Lerp (c1, c2, time));
      }
      
      public static Color sub (Color c1, Color c2) 
      {
         return quantize(c1 - c2);
      }
      
      public static Color add (Color c1, Color c2) 
      {
         return quantize(c1 + c2);
      }
      
      public static Color multiply (Color c1, Color c2) 
      {
         return quantize(c1 * c2);
      }
      
      public static Color divide (Color c1, float divisor) 
      {
         return quantize(c1 / divisor);
      }
      
      // This is almost redundant, but more mix types can be added
      public static Color mix (Color c1, Color c2, MixType mixType)
      {
         Color result;
         switch (mixType)
         {
            case MixType.average:
               result = divide(add (c1, c2), 2);
               break;
               
            case MixType.subtractive:
               result = sub(c1, c2);
               break;
               
            case MixType.additive:
               result = add(c1, c2);
               break;
               
            case MixType.multiply:
               result = multiply(c1, c2);
               break;
            
            default: 
               result = divide(add (c1, c2), 2);;
               break;
         }
         return result;
      }
      
      public static Color mix (Color c1, float divisor, MixType mixType)
      {
         Color result;
         if (mixType == MixType.divide) 
         {
            result = divide(c1, divisor);
         } else {
            result = quantize(c1);
         }
         return result;
      }
   }
}

