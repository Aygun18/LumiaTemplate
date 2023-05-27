namespace LumiaTemplate.Models
{
	public class Position
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Team> Team { get; set; }
	}
}
