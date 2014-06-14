using JAZS;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
   
   static int deltaTime = 0;
   
   SpriteRenderer bitmap;
   Texture2D texture;
   Color skinColour;
   
   List<Vector2> verts;
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
      
      Paint.Clear(texture, 0, 0, width, height);
      this.SetCharacteristics(16, Random.Range(8,16));
      this.Draw();
      this.Post();
   }
   
   void Post()
   {
      if(this.publicTest)
      {
         Media.PostImage(
            "http://www.zacpez.com/bitmapmaniac/upup.php?post",
            "file",
            this.texture.EncodeToPNG()
         );
      }
   }
   
   void SetCharacteristics (int scale, int complexity) 
   {
      skinColour = Colour.RandomColour(false);
      
      float angleComplex = ((360 / complexity) * Mathf.PI) / 180;
      verts = new List<Vector2>();
      
      int i;
      for(i = 0; i < complexity; i++) 
      {
         float x = center + Mathf.Cos(angleComplex*i) * (scale*Random.Range(0f,1.5f));
         float y = center + Mathf.Sin(angleComplex*i) * (scale*Random.Range(0f,1.5f));
         verts.Add(new Vector2(x,y));
         interior[0] += (int)x;
         interior[1] += (int)y;
      }
      // Snap together the head and tail
      verts[i-1] = verts[0];
      
      interior[0] /= verts.Count;
      interior[1] /= verts.Count;
   }
   
   void Draw () 
   {
      int i = 1;
      for(i = 1; i < verts.Count; i++)
      {
         Paint.DrawLine(texture, (int)verts[i-1].x, (int)verts[i-1].y, (int)verts[i].x, (int)verts[i].y, Color.black);
      }
      Paint.DrawLine(texture, (int)verts[0].x, (int)verts[0].y, (int)verts[i-1].x, (int)verts[i-1].y, Color.black);
      
      Paint.FloodFill(texture, interior[0], interior[1], texture.GetPixel(interior[0], interior[1]), skinColour);
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
         Paint.Clear(texture, 0, 0, width, height);
         this.SetCharacteristics(16, Random.Range(8,16));
         this.Draw();
         this.Post();
         deltaTime = 0;
      }
      ++deltaTime;
   }
}