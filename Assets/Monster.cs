using Colour;
using UnityEngine;
using System.IO;
using System.Collections;

public class Monster : MonoBehaviour {
	
	static int deltaTime = 0;
	
	SpriteRenderer bitmap;
	Texture2D texture;
	Color White = new Color(1f, 1f, 1f, 1f);
	Color Clear = new Color(0f, 0f, 0f, 0f);
	Color foreground = new Color(0f, 0f, 0f, 1f);
	Color background;
	
	Vector2[] verts;
	int[] interior = new int[2];
	public int width = 128;
	public int height = 128;
	public int center = 64;
	
	bool post = true;
	
	// Use this for initialization
	void Start () 
	{
		this.texture = new Texture2D(width, height);
		this.texture.filterMode = FilterMode.Point;
		this.texture.wrapMode = TextureWrapMode.Clamp;
		
		this.setCharacteristics(16, Random.Range(8,16));
		this.Draw();
		this.postImage();
	}
	
	void clear ()
	{
		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				texture.SetPixel(x,y,Clear);
			}
		}
	}
	
	void postImage () {
		if(post)
		{
			var bytes = this.texture.EncodeToPNG();
			var form = new WWWForm();
			
			form.AddBinaryData("file", bytes, System.DateTime.UtcNow.ToString()+".png", "images/png");
			var www = new WWW("http://www.zacpez.com/bitmapmaniac/upup.php?post", form);
			
			while (!www.isDone) { }
			
			if (www.error != null) {
				print(www.error);
			} else {
				print (www.text);
			}
		}
	}
	
	void setCharacteristics (int scale, int complexity) 
	{
		background = Colour.Colour.randomColour(false);
		
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
			this.DrawLine(texture, (int)verts[i-1].x, (int)verts[i-1].y, (int)verts[i].x, (int)verts[i].y, foreground);
		}
		
		this.DrawLine(texture, (int)verts[0].x, (int)verts[0].y, (int)verts[i-1].x, (int)verts[i-1].y, foreground);
		this.FloodFill(interior[0], interior[1], texture.GetPixel(interior[0], interior[1]));
		this.addEye();
		this.alphaEdges();
		
		this.texture.Apply();
		this.bitmap = GetComponent<SpriteRenderer>();
		this.bitmap.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
	}
	
	void alphaEdges () {
		Color targetColor = this.texture.GetPixel(0,0);
		for(int x = 0; x < texture.width; ++x)
		{
			for(int y = 0; y < texture.height; ++y)
			{
				if(this.texture.GetPixel(x,y) == targetColor) {
					this.texture.SetPixel(x,y,this.Clear);
				} 
			}
		}
	}
	
	// Borrowed from Unity Wiki
	void DrawLine (Texture2D tex, int x0, int y0, int x1, int y1, Color col)
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
		
		tex.SetPixel(x0, y0, col);
		if (dx > dy) {
			fraction = dy - (dx >> 1);
			while (Mathf.Abs(x0 - x1) > 1) {
				if (fraction >= 0) {
					y0 += stepy;
					fraction -= dx;
				}
				x0 += stepx;
				fraction += dy;
				tex.SetPixel(x0, y0, col);
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
				tex.SetPixel(x0, y0, col);
			}
		}
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
	
	void FloodFill (int x, int y, Color targetColor) 
	{
		Color looking = texture.GetPixel(x,y);
		
		if (foreground == looking)
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
			texture.SetPixel(x, y, this.randomColor(background, 0.5f));
		}
		
		this.FloodFill(x+1, y, targetColor);
		this.FloodFill(x-1, y, targetColor);
		this.FloodFill(x, y+1, targetColor);
		this.FloodFill(x, y-1, targetColor);
		return;
	}
	
	
	float plusminus (float spread)
	{
		return -(spread/2) + Random.Range(0f, spread);
	}
	
	Color randomColor (Color sourceColor, float spread)
	{
		Color result = new Color(
			sourceColor.r + plusminus (spread),
			sourceColor.g + plusminus (spread),
			sourceColor.b + plusminus (spread),
			sourceColor.a + plusminus (spread)
			);
		return Colour.Colour.quantize(result);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(deltaTime > 300)
		{
			this.clear ();
			this.setCharacteristics(16, Random.Range(8,16));
			this.Draw();
			this.postImage();
			deltaTime = 0;
		}
		deltaTime++;
	}
}