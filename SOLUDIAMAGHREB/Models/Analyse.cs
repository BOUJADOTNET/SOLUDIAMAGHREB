namespace SOLUDIAMAGHREB.Models
{
    public class Analyse
    {
        public int idAvisAppelOff { get; set; }
        public int Nlot { get; set; }
        public decimal Montant_total_DHS { get; set; }
        public string Montant_total_littre_DHS { get; set; }
        public string NomberBordereau { get; set; }
        public DateTime DateCreation { get; set; }


        // Navigation property for BordereauItem
        public BordereauItem BordereauItem { get; set; }
    }
}
