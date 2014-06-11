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
      
      public static Color Quantize (Color input)
      {
         input.r = (input.r * quantum) / quantum;
         input.g = (input.g * quantum) / quantum;
         input.b = (input.b * quantum) / quantum;
         input.a = (input.a * quantum) / quantum;
         return input;
      }
            
      public static Color RandomColour (bool alpha)
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
         return Quantize(result);
      }
      
      public static Color RandomColor (Color sourceColor, float spread, bool alpha)
      {
         sourceColor.r += Math.PlusMinus(spread);
         sourceColor.g += Math.PlusMinus(spread);
         sourceColor.b += Math.PlusMinus(spread);
         
         if(alpha)
         {
            sourceColor.a += Math.PlusMinus(spread);
         }

         return Quantize(sourceColor);
      }
      
      public static Color RandomColour (float r, float g, float b, float a)
      {
         Color result = new Color(r,g,b,a);
         return Quantize(result);
      }
      
      public static Color Lerp (Color c1, Color c2, float time)
      {
         return Quantize(Color.Lerp (c1, c2, time));
      }
      
      public static Color Sub (Color c1, Color c2) 
      {
         return Quantize(c1 - c2);
      }
      
      public static Color Add (Color c1, Color c2) 
      {
         return Quantize(c1 + c2);
      }
      
      public static Color Multiply (Color c1, Color c2) 
      {
         return Quantize(c1 * c2);
      }
      
      public static Color Divide (Color c1, float divisor) 
      {
         return Quantize(c1 / divisor);
      }
      
      // This is almost redundant, but more mix types can be added
      public static Color Mix (Color c1, Color c2, MixType mixType)
      {
         Color result;
         switch (mixType)
         {
            case MixType.average:
               result = Divide(Add (c1, c2), 2);
               break;
               
            case MixType.subtractive:
               result = Sub(c1, c2);
               break;
               
            case MixType.additive:
               result = Add(c1, c2);
               break;
               
            case MixType.multiply:
               result = Multiply(c1, c2);
               break;
            
            default: 
               result = Divide(Add (c1, c2), 2);;
               break;
         }
         return result;
      }
      
      public static Color Mix (Color c1, float divisor, MixType mixType)
      {
         Color result;
         if (mixType == MixType.divide) 
         {
            result = Divide(c1, divisor);
         } 
         else 
         {
            result = Quantize(c1);
         }
         return result;
      }
   }
}

