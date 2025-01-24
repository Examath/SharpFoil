namespace SharpFoil.Model
{
	public class PointProfile
	{
		public double[] X;

		public double[] Y;

		int InflectionIndex;

		public PointProfile(double[] x, double[] y)
		{
			if (x.Length == 0 || y.Length == 0)
			{
				throw new ArgumentException("Length of arrays cannot be zero");
			}
			else if (x.Length != y.Length)
			{
				throw new ArgumentException("Length of x and y arrays must match");
			}

			X = x;
			Y = y;
			UpdateInflection();
		}

		private void UpdateInflection()
		{
			InflectionIndex = 0;

			for (int i = 0; i < X.Length; i++)
			{
				if (X[i] < X[InflectionIndex])
				{
					InflectionIndex = i;
				}
			}
		}

		public static (PointProfile, string) FromSeligDatFile(string file)
		{
			// Lednicer/Selig rules (R*) from http://airfoiltools.com/airfoil/index

			// R1: The file is read a line at a time starting from the top.
			string[] lines = file.Split('\n');

			// Minimum number of points for a closed profile (3) + name
			if (lines.Length <= 4)
			{
				throw new InvalidDataException("Dat file too short to be a valid profile");
			}

			// R2: Name is first line
			string name = lines[0];

			List<double> xs = [], ys = [];

			for (int i = 1; i < lines.Length; i++)
			{
				// R1: Blank lines are discarded.
				if (string.IsNullOrWhiteSpace(lines[i])) continue;

				// R3: All subsequent lines must have 2 numeric values separated by white space characters
				string[] pointRaw = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (pointRaw.Length != 2 
					|| !double.TryParse(pointRaw[0], out double x)
					|| !double.TryParse(pointRaw[1], out double y))
				{
					throw new InvalidDataException($"Could not parse point on line #{i}");
				}

				// R4: If the first numeric values are greater than 1 the file is assumed
				// to be in Lednicer format and these indicate the number of coordinates
				// on the top and bottom surfaces.
				if (x > 1 || y > 1)
				{
					throw new NotImplementedException("Lednicer format not supported");
				}

				// Add points. Order of points from: https://m-selig.ae.illinois.edu/ads.html
				// Add Selig point: 
				xs.Add(x);
				ys.Add(y);

				// R5 and R6 (bounds checking) skipped.
			}

			return (new PointProfile(xs.ToArray(), ys.ToArray()), name);
		}
	}
}
