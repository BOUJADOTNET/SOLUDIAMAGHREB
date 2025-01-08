namespace SOLUDIAMAGHREB.Models
{
    public class BordereauManager
    {
        public string NomberBordereau { get; set; }
        public DateTime DateCreation { get; set; }
        public string Intitu_AppelOffres { get; set; }

        public List<BordereauItem> Items { get; set; } = new List<BordereauItem>();
    }
}
