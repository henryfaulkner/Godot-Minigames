using Godot;
using System;

public class PartyColorBusiness
{
	private readonly Color[] _colorCache = new Color[256];
	private const int Range = 255;

	public PartyColorBusiness()
	{
		// Precompute the color wheel once during initialization
		for (int i = 0; i <= Range; i++)
		{
			_colorCache[i] = CalculateWheelColor(i);
		}
	}

	public Color GetWheelColor(int pos)
	{
		// Modulo to ensure pos is between 0 and 255
		pos = pos % (Range + 1);
		return _colorCache[pos];
	}

	private Color CalculateWheelColor(int pos)
	{
		int r = 0, g = 0, b = 0, a = 0, range = 255;

		// Get rgb values
		if (pos < 0) { }
		else if (pos < 85)
		{
			r = pos * 3;
			g = range - (pos * 3);
			b = 0;
		}
		else if (pos < 170)
		{
			pos -= 85;
			r = range - (pos * 3);
			g = 0;
			b = pos * 3;
		}
		else
		{
			pos -= 170;
			r = 0;
			g = pos * 3;
			b = range - (pos * 3);
		}

		// Normalize rgba values
		var result = new Color(
			r / (float)range,
			g / (float)range,
			b / (float)range,
			a / (float)range
		);
		return result;
	}
}
