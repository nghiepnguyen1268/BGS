namespace Application.ViewModels.Management
{
    public class OldOrNewProducts
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Pictures { get; set; }
        public double ProductPrice { get; set; }
        public double? DiscountProductprice { get; set; }
        public bool IsFeatured { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Logics
        //public string Tag => CreatedDate >= DateTime.Now.AddDays(-5) ? "New" : "Old";
        public string Tag
        {
            get
            {
                if (CreatedDate >= DateTime.Now.AddDays(-5))
                {
                    return "New";
                }
                else if (CreatedDate >= DateTime.Now.AddDays(-10) && CreatedDate < DateTime.Now.AddDays(-5))
                {
                    return "Hot";
                }
                else if (DiscountProductprice < ProductPrice && CreatedDate <= DateTime.Now.AddDays(-365))
                {
                    return "Sale";
                }
                else
                {
                    return "Old";
                }
            }
        }
    }
}
