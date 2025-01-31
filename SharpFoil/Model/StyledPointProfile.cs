﻿namespace SharpFoil.Model
{
	/// <summary>
	/// Represents a <see cref="PointProfile"/> with additional styling info
	/// for rendering on a JavaScript canvas in front-end
	/// </summary>
	public class StyledPointProfile : PointProfile
	{
		/// <summary>
		/// Gets the HTML color to draw the profile in
		/// </summary>
		public string Color { get; private set; }

		/// <summary>
		/// Gets the line width of the drawn profile
		/// </summary>
		public int LineWidth { get; private set; }

		/// <summary>
		/// Gets whether to render points as markers, rather than lines
		/// </summary>
		public bool UsePoints { get; private set; }

		/// <summary>
		/// Creates a StyledPointProfile from the given <see cref="PointProfile"/> and styling attributes
		/// </summary>
		/// <param name="pointProfile">The <see cref="PointProfile"/> to clone</param>
		/// <param name="color">The HTML color to draw the profile in</param>
		/// <param name="usePoints">Whether to render points as markers, rather than lines</param>
		public StyledPointProfile(PointProfile pointProfile, string color, int lineWidth = 1, bool usePoints = false) 
			: base(pointProfile.X, pointProfile.Y)
		{
			Color = color;
			LineWidth = lineWidth;
			UsePoints = usePoints;
		}
	}
}
