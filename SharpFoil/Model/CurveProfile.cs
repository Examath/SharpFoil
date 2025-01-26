using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;

namespace SharpFoil.Model
{
	public class CurveProfile
	{
		public static int DisplayPoints { get; set; } = 400;

		public PointProfile RasterProfile { get; private set; }

		public PointProfile DisplayProfile { get; private set; }

		public double[] Coefficients { get; private set; }

		public double RSquared { get; private set; }

		public int Order { get; private set; }

		public CurveProfile(PointProfile original, int order)
		{
			Order = order;
			if (Order < 3) throw new ArgumentException("Order must be greater than 3");

			double[] x2 = original.GetSquaredRootX();

			//old Coefficients = Fit.Polynomial(x2, original.Y, order, DirectRegressionMethod.Svd);

			// Solving A * b = y for b
			// Where a is a m (rows) by n (order) matrix.
			//       [x0, x0^2, ... x0^n]   [b0 ]   [y0 ]
			//	   ~ [          ...     ] * [...] = [...]
			//		 [xm, xm^2, ... xm^n]   [bn ]   [yn ]

			// Generate predictor matrix
			double[][] A = new double[original.Length][];

			for (int row = 0; row < original.Length; row++)
			{
				double multiplier = x2[row];
				double value = 1;

				A[row] = new double[order];

				for (int col = 0; col < order; col++)
				{
					value *= multiplier;
					A[row][col] = value;
				}
			}

			Coefficients = MultipleRegression.Svd(A, original.Y);

			RasterProfile = new((double[])original.X.Clone(), Y(x2));
			RSquared = GoodnessOfFit.RSquared(RasterProfile.Y , original.Y);
			DisplayProfile = Rasterize(DisplayPoints);
		}

		private double[] Y(double[] x2)
		{
			double[] y = new double[x2.Length];
			for (int row = 0; row < x2.Length; row++)
			{
				//old y[row] = Polynomial.Evaluate(x2[row], Coefficients);

				double multiplier = x2[row];
				double value = 1;

				for (int col = 0; col < Order; col++)
				{
					value *= multiplier;
					y[row] += Coefficients[col] * value;
				}
			}
			return y;
		}

		public PointProfile Rasterize(int pointCount)
		{
			double[] t = Generate.LinearSpaced(pointCount, -1, 1);
			double[] x = new double[pointCount];
			for (int i = 0; i < pointCount; i++)
			{
				x[i] = t[i] * t[i];
			}
			return new(x, Y(t));
		}
	}
}
