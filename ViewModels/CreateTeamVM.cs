namespace LumiaTemplate.ViewModels
{
	public class CreateTeamVM
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int PositionId { get; set; }
		public IFormFile Photo { get; set; }

	}
}
