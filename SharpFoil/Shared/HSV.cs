using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFoil.Shared
{
	// Pilfered from https://github.com/Examath/Core/blob/master/Utils/HSV.cs

	/// <summary>
	/// Helper class to convert from HSV to RGB values,
	/// and generate random colors using the HSV space
	/// </summary>
	public class HSV
	{
		/// <summary>
		/// Creates a color using specified HSV values.
		/// </summary>
		/// <param name="hue">Hue between 0 and 360 degrees</param>
		/// <param name="saturation">Chroma between 0 and 1</param>
		/// <param name="value">Brightness between 0 and 1</param>
		/// <returns>A <see cref="Color"/></returns>
		/// <remarks>
		/// From https://stackoverflow.com/a/1626232/10701111
		/// </remarks>
		public static string ToColor(float hue, float saturation, float value)
		{
			int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
			double f = hue / 60 - Math.Floor(hue / 60);

			value *= 255;
			byte v = Convert.ToByte(value);
			byte p = Convert.ToByte(value * (1 - saturation));
			byte q = Convert.ToByte(value * (1 - f * saturation));
			byte t = Convert.ToByte(value * (1 - (1 - f) * saturation));

			if (hi == 0)
				return ToHex(v, t, p);
			else if (hi == 1)
				return ToHex(q, v, p);
			else if (hi == 2)
				return ToHex(p, v, t);
			else if (hi == 3)
				return ToHex(p, q, v);
			else if (hi == 4)
				return ToHex(t, p, v);
			else
				return ToHex(v, p, q);
		}

		private static readonly Random _Random = new();

		/// <summary>
		/// Generate a random color
		/// </summary>
		/// <param name="hueMin">Minimum hue between 0 and 360 degrees</param>
		/// <param name="hueMax">Maximum hue between 0 and 360 degrees</param>
		/// <param name="saturationMin">Minimum chroma between 0 and 1</param>
		/// <param name="saturationMax">Maximum chroma between 0 and 1</param>
		/// <param name="valueMin">Minimum rightness between 0 and 1</param>
		/// <param name="valueMax">Maximum rightness between 0 and 1</param>
		/// <param name="random">Override the Random Number Generator</param>
		/// <returns>A <see cref="Color"/></returns>
		public static string GetRandomColor(
			float hueMin = 0f, float hueMax = 360f,
			float saturationMin = 0.7f, float saturationMax = 1f,
			float valueMin = 0.7f, float valueMax = 1f,
			Random? random = null)
		{
			random ??= _Random;
			return ToColor(next(hueMin, hueMax), next(saturationMin, saturationMax), next(valueMin, valueMax));

			float next(float min, float max)
			{
				return min + (float)random.NextDouble() * (max - min);
			}
		}

		private static string ToHex(byte r, byte g, byte b)
		{
			return $"#{r:X2}{g:X2}{b:X2}";
		}
	}
}