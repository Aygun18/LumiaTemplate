namespace LumiaTemplate.ViewModels
{
	public class UpdateTeamVM
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public int PositionId { get; set; }
		public IFormFile Photo { get; set; }
	}
}
