using Microsoft.AspNetCore.Mvc.Rendering;

namespace SOLUDIAMAGHREB.Models.ViewModel
{
    public class BordereauPrixViewModel
    {
        public string SelectedBordereauNumber { get; set; } // New property to hold the selected bordereau number
        public List<SelectListItem> BordereauNumberOptions { get; set; }
        public string NomberBordereau { get; set; }
        public DateTime DateCreation { get; set; }
        public string Intitu_AppelOffres { get; set; }
        public List<BordereauItemViewModel> Items { get; set; } = new List<BordereauItemViewModel>();
    }
    public class BordereauItemViewModel
    {
        public int Id { get; set; }
        public int Nlot { get; set; }
        public string Designation { get; set; }

        /////////// Designation ///////////
        public string NomdeMarque { get; set; }
        public string Conditionnement { get; set; }

        /////////// Designation ///////////
        public string Unite_Compte { get; set; }
        public int Quantite { get; set; }
        public decimal Prix_Unit_TVA { get; set; }
        public decimal Prix_Total_TVA { get; set; }
    }

}
