namespace SOLUDIAMAGHREB.Models.ViewModel
{
    public class BordereauPrixPreviewViewModel
    {
        public List<MyBorderauItems> Items { get; set; }
        public decimal TotalHorsTVA { get; set; }
        public decimal TauxTVA { get; set; }
        public decimal TotalTTC { get; set; }
        public string TotalTTCChrac { get; set; }
        public DateTime DateCreation { get; set; }
        public string AppelOffres { get; set; }




    }
}
