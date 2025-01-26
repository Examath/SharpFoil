using System.Data;

namespace SharpFoil.Model
{
	public class Analysis
	{
		public PointProfile SourceProfile { get; private set; }

		public string Name { get; set; }

		private bool _IsVisible = true;
		public bool IsVisible
		{
			get => _IsVisible;
			set
			{
				if (_IsVisible != value)
				{
					_IsVisible = value;
					StateChanged?.Invoke(this, new());
				}
			}
		}

		private string _Color;
		public string Color
		{
			get => _Color;
			set
			{
				if (_Color != value)
				{
					_Color = value;
					StateChanged?.Invoke(this, new());
				}
			}
		}

		private int _Order = 5;
		public int Order
		{
			get => _Order;
			set
			{
				if (_Order != value)
				{
					_Order = value;
					UpdateCurveProfile();
				}
			}
		}

		public CurveProfile? CurveProfile { get; private set; }

		public Analysis(PointProfile sourceProfile, string name)
		{
			SourceProfile = sourceProfile;
			Name = name;
			_Color = Shared.HSV.GetRandomColor();
			UpdateCurveProfile();
		}

		private void UpdateCurveProfile()
		{
			CurveProfile = new(SourceProfile, Order);
			StateChanged?.Invoke(this, new EventArgs());
		}
		public event EventHandler? StateChanged;

		#region Default

		public static Analysis GenerateDefault()
		{
			(PointProfile profile, string name) = PointProfile.FromSeligDatFile(DEFAULT_PROFILE);
			return new Analysis(profile, name);
		}

		public override string ToString()
		{
			return $"{Name} Analysis";
		}

		private const string DEFAULT_PROFILE = @"NACA 4412
  1.000000  0.001300
  0.950000  0.014700
  0.900000  0.027100
  0.800000  0.048900
  0.700000  0.066900
  0.600000  0.081400
  0.500000  0.091900
  0.400000  0.098000
  0.300000  0.097600
  0.250000  0.094100
  0.200000  0.088000
  0.150000  0.078900
  0.100000  0.065900
  0.075000  0.057600
  0.050000  0.047300
  0.025000  0.033900
  0.012500  0.024400
  0.000000  0.000000
  0.012500 -0.014300
  0.025000 -0.019500
  0.050000 -0.024900
  0.075000 -0.027400
  0.100000 -0.028600
  0.150000 -0.028800
  0.200000 -0.027400
  0.250000 -0.025000
  0.300000 -0.022600
  0.400000 -0.018000
  0.500000 -0.014000
  0.600000 -0.010000
  0.700000 -0.006500
  0.800000 -0.003900
  0.900000 -0.002200
  0.950000 -0.001600
  1.000000 -0.001300";

		#endregion
	}
}
