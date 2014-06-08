using JAZS;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
   
   static int deltaTime = 0;
   
   SpriteRenderer bitmap;
   Texture2D texture;
   Color foreground = new Color(0f, 0f, 0f, 1f);
   Color background;
   
   Vector2[] verts;
   int[] interior = new int[2];
   public int width = 128;
   public int height = 128;
   public int center = 64;
   
   bool publicTest = false;
   
   // Use this for initialization
   void Start () 
   {
      this.texture = new Texture2D(width, height);
      this.texture.filterMode = FilterMode.Point;
      this.texture.wrapMode = TextureWrapMode.Clamp;
      
      Paint.clear(texture, 0, 0, width, height);
      this.setCharacteristics(16, Random.Range(8,16));
      this.Draw();
      this.post();
   }
   
   void post()
   {
      if(this.publicTest)
      {
         Media.postImage(
            "http://www.zacpez.com/bitmapmaniac/upup.php?post",
            "file",
            this.texture.EncodeToPNG()
         );
      }
   }
   
   void setCharacteristics (int scale, int complexity) 
   {
      background = Colour.randomColour(false);
      
      float angleComplex = ((360 / complexity) * Mathf.PI) / 180;
      verts = new Vector2[complexity];
      
      int i;
      for(i = 0; i < complexity; i++) 
      {
         float x = center + Mathf.Cos(angleComplex*i) * (scale*Random.Range(0f,1.5f));
         float y = center + Mathf.Sin(angleComplex*i) * (scale*Random.Range(0f,1.5f));
         verts[i] = new Vector2(x,y);
         interior[0] += (int)x;
         interior[1] += (int)y;
      }
      // Next two lines are hacked in to fix half-expected gap
      verts[i-1].x = verts[0].x;
      verts[i-1].y = verts[0].y;
      
      interior[0] /= verts.Length;
      interior[1] /= verts.Length;
   }
   
   void Draw () 
   {
      int i = 1;
      for(i = 1; i < verts.Length; i++)
      {
         Paint.DrawLine(texture, (int)verts[i-1].x, (int)verts[i-1].y, (int)verts[i].x, (int)verts[i].y, foreground);
      }
      Paint.DrawLine(texture, (int)verts[0].x, (int)verts[0].y, (int)verts[i-1].x, (int)verts[i-1].y, foreground);
      
      Paint.FloodFill(texture, interior[0], interior[1], texture.GetPixel(interior[0], interior[1]), background);
      this.addEye();
      
      this.texture.Apply();
      this.bitmap = GetComponent<SpriteRenderer>();
      this.bitmap.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
   }  
   
   // WIP
   void addEye ()
   {
      texture.SetPixel(interior[0], interior[1], new Color(0f,0f,0f,1f));
      texture.SetPixel(interior[0]+1, interior[1], new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0]+1, interior[1]-1, new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0]+1, interior[1]+1, new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0], interior[1]+1, new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0], interior[1]-1, new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0]-1, interior[1], new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0]-1, interior[1]+1, new Color(1f,1f,1f,1f));
      texture.SetPixel(interior[0]-1, interior[1]-1, new Color(1f,1f,1f,1f));
   }
   
   
   // Update is called once per frame
   void Update () 
   {
      if(300 < deltaTime)
      {
         Paint.clear(texture, 0, 0, width, height);
         this.setCharacteristics(16, Random.Range(8,16));
         this.Draw();
         this.post();
         deltaTime = 0;
      }
      ++deltaTime;
   }
}