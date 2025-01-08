namespace SOLUDIAMAGHREB.Models
{
    public class BordereauItem
    {
        public int Id { get; set; }
        public int Nlot { get; set; }
        public string Designation { get; set; }
        public string Unite_Compte { get; set; }
        public int Quantite { get; set; }
        public decimal Prix_Unit_TVA { get; set; }
        public decimal Prix_Total_TVA { get; set; }

        // Foreign key to link back to BordereauManager
        public string NomberBordereau { get; set; }
        public BordereauManager BordereauManager { get; set; }

        // Navigation property for Analyse
        public ICollection<Analyse> Analyses { get; set; }
    }
}
