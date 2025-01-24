namespace SharpFoil.Model
{
	public class Analysis
	{
		public PointProfile SourceProfile { get; private set; }

		public string Name { get; set; }

		public Analysis(PointProfile sourceProfile, string name)
		{
			SourceProfile = sourceProfile;
			Name = name;
		}
	}
}
