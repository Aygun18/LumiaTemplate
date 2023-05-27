namespace LumiaTemplate.Models
{
	public class Team
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		public int PositionId { get; set; }
		public string Description { get; set; }
		public Position Position { get; set; }
	}
}
