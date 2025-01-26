namespace SharpFoil.Model
{
	/// <summary>
	/// Represents a airfoil profile using a list of points
	/// </summary>
	public class PointProfile
	{
		/// <summary>
		/// Gets the coordinates in the longitudinal direction of the profile
		/// </summary>
		/// <remarks>
		/// The points are ordered starting from trailing edge, 
		/// along the upper surface to the leading edge 
		/// and back around the lower surface to trailing edge.
		/// </remarks>
		public double[] X { get; private set; }

		/// <summary>
		/// Gets the coordinates in the vertical direction of the profile
		/// </summary>
		/// <remarks>
		/// The points are ordered starting from trailing edge, 
		/// along the upper surface to the leading edge 
		/// and back around the lower surface to trailing edge.
		/// </remarks>
		public double[] Y { get; private set; }

		/// <summary>
		/// Gets the index of the leading edge (minimum x) point
		/// </summary>
		public int InflectionIndex { get; private set; }

		/// <summary>
		/// Gets the number of points defining this profile
		/// </summary>
		public int Length { get; private set; }

		/// <summary>
		/// Creates an new PointProfile from coordinates
		/// </summary>
		/// <remarks>
		/// Note: <paramref name="x"/> and <paramref name="y"/> must have the same length. 
		/// They form pairs of cartesian coordinates starting from the trailing edge, 
		/// along the upper surface to the leading edge,
		/// and back around the lower surface to trailing edge.
		/// </remarks>
		/// <param name="x">An array of coordinates in the longitudinal direction</param>
		/// <param name="y">An array of coordinates in the vertical direction</param>
		/// <exception cref="ArgumentException"></exception>
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
			Length = X.Length;
			UpdateInflection();
		}

		private void UpdateInflection()
		{
			InflectionIndex = 0;

			for (int i = 0; i < Length; i++)
			{
				if (X[i] < X[InflectionIndex])
				{
					InflectionIndex = i;
				}
			}
		}

		/// <summary>
		/// Finds the square root of the <see cref="X"/> coordinates of the points,
		/// where points on the upper surface are negative
		/// </summary>
		/// <returns>An array with the squared values</returns>
		public double[] GetSquaredRootX()
		{
			double[] x2 = new double[Length];
			for (int i = 0; i < X.Length; i++)
			{
				if (i <= InflectionIndex)
				{
					x2[i] = - Math.Sqrt(X[i]);
				}
				else
				{
					x2[i] = Math.Sqrt(X[i]);
				}				
			}
			return x2;
		}

		/// <summary>
		/// Creates a <see cref="PointProfile"/> from the provided Selig format data file
		/// </summary>
		/// <param name="file">The Selig format data file to interpret</param>
		/// <returns>
		/// A tuple with the generated <see cref="PointProfile"/> 
		/// and the name or description of the profile.
		/// </returns>
		/// <exception cref="InvalidDataException"></exception>
		/// <exception cref="NotImplementedException"></exception>
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
