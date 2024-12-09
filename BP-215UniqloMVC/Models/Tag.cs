namespace BP_215UniqloMVC.Models
{
	public class Tag:BaseEntity
	{
		public string Name { get; set; }
		public ICollection<Products> Products { get; set; }

	}
}
