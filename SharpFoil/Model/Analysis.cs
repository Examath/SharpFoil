using System.Data;

namespace SharpFoil.Model
{
	public class Analysis
	{
		public PointProfile SourceProfile { get; private set; }

		public string Name { get; set; }

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
			Color = Shared.HSV.GetRandomColor();
			UpdateCurveProfile();
		}

		private void UpdateCurveProfile()
		{
			CurveProfile = new(SourceProfile, Order);
			StateChanged?.Invoke(this, new EventArgs());
		}

		public event EventHandler? StateChanged;
	}
}
