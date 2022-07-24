namespace MyAbilityFirst.Domain
{
	public class UserSubcategory
	{
		public int ID { get; set; }
		public int OwnerUserID { get; set; }
		public int SubCategoryID { get; set; }
		public bool Selected { get; set; }
		public string CustomValue { get; set; }
	}
}