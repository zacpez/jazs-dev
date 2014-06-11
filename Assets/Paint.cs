using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JAZS
{
   public class Paint
   {
   
      private Dictionary<string,Color[]> dataBlock;
      
      public Paint ()
      {
         dataBlock = new Dictionary<string, Color[]>();
      }
      
      public static void Clear (Texture2D texture, int x, int y, int w, int h)
      {
         for(int dx = x; dx < w; dx++)
         {
            for(int dy = y; dy < h; dy++)
            {
               texture.SetPixel(dx, dy, Color.clear);
            }
         }
      }
      
      // Borrowed from Unity Wiki
      public static void DrawLine (Texture2D texture, int x0, int y0, int x1, int y1, Color col)
      {
         int dy = (int)(y1-y0);
         int dx = (int)(x1-x0);
         int stepx, stepy;
         
         if (dy < 0) {dy = -dy; stepy = -1;}
         else {stepy = 1;}
         if (dx < 0) {dx = -dx; stepx = -1;}
         else {stepx = 1;}
         dy <<= 1;
         dx <<= 1;
         
         float fraction = 0;
         
         texture.SetPixel(x0, y0, col);
         if (dx > dy) {
            fraction = dy - (dx >> 1);
            while (Mathf.Abs(x0 - x1) > 1) {
               if (fraction >= 0) {
                  y0 += stepy;
                  fraction -= dx;
               }
               x0 += stepx;
               fraction += dy;
               texture.SetPixel(x0, y0, col);
            }
         }
         else {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs(y0 - y1) > 1) {
               if (fraction >= 0) {
                  x0 += stepx;
                  fraction -= dy;
               }
               y0 += stepy;
               fraction += dx;
               texture.SetPixel(x0, y0, col);
            }
         }
      }
      
      public static void FloodFill (Texture2D texture, int x, int y, Color targetColor, Color paint) 
      {
         Color looking = texture.GetPixel(x,y);
         
         if (Color.black == looking)
         {
            return;
         }
         if (targetColor != looking)
         {
            return;
         } 
         else 
         {
            // Skin colour variation
            texture.SetPixel(x, y, Colour.RandomColor(paint, 0.5f, false));
         }
         
         FloodFill(texture, x+1, y, targetColor, paint);
         FloodFill(texture, x-1, y, targetColor, paint);
         FloodFill(texture, x, y+1, targetColor, paint);
         FloodFill(texture, x, y-1, targetColor, paint);
         return;
      }
      
      public static Texture2D Flatten (Texture2D bottom, Texture2D top, int x, int y)
      {
         bottom.SetPixels(x, y, top.width, top.height, top.GetPixels());
         return bottom;
      }
      
      public bool SetItem (string key, Color[] value)
      {
         dataBlock[key] = value;
         return true;
      }
     
      public Color[] GetItem (string key)
      {
         if(dataBlock.ContainsKey(key))
         {
            return dataBlock[key];
         }
         else
         {
            return null;
         }
      }
   }
}