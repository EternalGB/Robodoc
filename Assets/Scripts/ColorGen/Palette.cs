using UnityEngine;
using System.Collections;


public class Palette
{

	float phi = 1.61803398875f;

	public static Color[] basicColors = {Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow};
	Color[] colors;

	public Palette(Color[] colors)
	{
		this.colors = colors;
	}

	public Palette(int numColors, float saturation, float luminance)
	{
		colors = new Color[numColors];
		float h = Random.value;
		for(int i = 0; i < colors.Length; i++) {
			h += 1/phi;
			h %= 1;
			colors[i] = (new HSBColor(h,saturation,luminance)).ToColor();
		}
	}

	public Palette(int numColors, float saturation, float luminance, float similarityTolerance)
	{
		colors = new Color[numColors];
		float[] hueValues = new float[numColors];
		float h = Random.value;
		for(int i = 0; i < colors.Length; i++) {
			h += 1/phi;
			h %= 1;
			//make sure the new hue isn't too close to another colour
			//this will become increasingly difficulty
			float diff = 0;
			int maxTries = 10;
			int tries = 0;
			do {
				for(int j = 0; j < i; j++) {
					diff = Mathf.Abs(h - hueValues[j]);
					if(diff < similarityTolerance) {
						h += Random.value;
						h %= 1;
						break;
					}
				}
				tries++;
			} while(diff <= similarityTolerance && tries < maxTries);
			hueValues[i] = h;
			colors[i] = (new HSBColor(h,saturation,luminance)).ToColor();
		}
	}

	public Palette(Color firstColor, int numColors, float maxOffset)
	{
		generatePalette(firstColor,numColors,maxOffset);

	}


	public Palette (int numColors, float maxOffset)
	{
		Color firstColor = new Color(Random.value,Random.value,Random.value);
		generatePalette(firstColor,numColors,maxOffset);
	}

	public Palette(
	   int colorCount,
	   float offsetAngle1,
	   float offsetAngle2,
	   float rangeAngle0,
	   float rangeAngle1,
	   float rangeAngle2,
	   float saturation, float luminance)
	{
		generatePalette (colorCount,offsetAngle1,offsetAngle2,rangeAngle0,rangeAngle1,rangeAngle2,saturation,luminance);
	}


	void generatePalette(int colorCount,
	                     float offsetAngle1,
	                     float offsetAngle2,
	                     float rangeAngle0,
	                     float rangeAngle1,
	                     float rangeAngle2,
	                     float saturation, float luminance)
	{
		colors = new Color[colorCount];
		float referenceAngle = Random.value * 360;
		
		for (int i = 0; i < colorCount; i++) {
			float randomAngle = 
				Random.value * (rangeAngle0 + rangeAngle1 + rangeAngle2);
			
			if (randomAngle > rangeAngle0) {
				if (randomAngle < rangeAngle0 + rangeAngle1) {
					randomAngle += offsetAngle1;
				}
				else {
					randomAngle += offsetAngle2;
				}
			}

			HSBColor hsbColor = new HSBColor(
				((referenceAngle + randomAngle) / 360.0f) % 1.0f,
				saturation, 
				luminance);
			
			colors[i] = hsbColor.ToColor();
		}
	}

	void generatePalette(Color firstColor, int numColors, float maxOffset)
	{
		colors = new Color[numColors];
		colors[0] = firstColor;
		float value = (firstColor.r + firstColor.g + firstColor.b)/3;
		for(int i = 1; i < numColors; i++) {
			float offset = Random.Range(0f,maxOffset);
			float newValue = value + (2*Random.value * offset) - offset;
			float valueRatio = newValue/value;
			Color next = new Color(firstColor.r * valueRatio,firstColor.g * valueRatio,firstColor.b * valueRatio);
			colors[i] = next;
		}
	}

	public Color random()
	{
		return colors[Random.Range(0,colors.Length)];
	}

	public Color GetColor(int index)
	{
		return colors[index];
	}

	public override string ToString()
	{
		string msg = "";
		for(int i = 0; i < colors.Length; i++) {
			msg += colors[i].ToString() + " ";
		}
		msg += "\n";
		return msg;
	}

	static Color from255(float r, float g, float b)
	{
		return new Color(r/255f,g/255f,b/255f);
	}

	public static Palette neon()
	{
		Color[] neons = new Color[16];
		neons[0] = from255(170,255,0);
		neons[1] = from255(255,170,0);
		neons[2] = from255(255,0,170);
		neons[3] = from255(170,0,255);
		neons[4] = from255(0,170,255);
		neons[5] = from255(243,243,21);
		neons[6] = from255(57,255,20);
		neons[7] = from255(236,19,65);
		neons[8] = from255(255,0,153);
		neons[9] = from255(180,49,244);
		neons[10] = from255(70,228,188);
		neons[11] = from255(255,36,164);
		neons[12] = from255(180,49,244);
		neons[13] = from255(172,233,0);
		neons[14] = from255(0,148,255);
		neons[15] = from255(6,0,255);
		return new Palette(neons);
	}

	public static Palette skyboxNeon()
	{
		Palette tmp = neon ();
		for(int i = 0; i < tmp.colors.Length; i++) {
			HSBColor hsbc = HSBColor.FromColor(tmp.colors[i]);
			hsbc.b = 0.5f;
			tmp.colors[i] = hsbc.ToColor();
		}
		return tmp;
	}

	public static Palette basic()
	{
		return new Palette(basicColors);
	}

	public static Color invertColor(Color c)
	{
		HSBColor tmp = HSBColor.FromColor(c);
		//MonoBehaviour.print(tmp.ToString());
		tmp.h = 1f - tmp.h;
		//MonoBehaviour.print(tmp.ToString());
		return tmp.ToColor();
	}

	public static Color complementaryColor(Color c)
	{
		HSBColor tmp = HSBColor.FromColor(c);
		float h = tmp.h*360f;
		float newH = Mathf.Abs (360-h) % 360;
		tmp.h = newH/360f;
		return tmp.ToColor();
	}

	public static Color setBrightness(Color c, float b)
	{
		HSBColor tmp = HSBColor.FromColor(c);
		tmp.b = b;
		return tmp.ToColor();
	}
}


