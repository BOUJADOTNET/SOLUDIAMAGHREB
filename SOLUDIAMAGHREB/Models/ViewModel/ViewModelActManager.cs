using Microsoft.AspNetCore.Mvc.Rendering;

namespace SOLUDIAMAGHREB.Models.ViewModel
{
    public class ViewModelActManager
    {
        //public string SelectedBordereau { get; set; }
        //public List<string> BordereauNumbers { get; set; } = new List<string>();
        public string IdactEng { get; set; }
        public string Appel_Offres { get; set; }
        public string Objet_du_Marche { get; set; }
        public string Marche_passe { get; set; }
        public string Capital { get; set; }
        public string NLot { get; set; }
        public decimal TauxTva { get; set; }
        public decimal MontantHtTva { get; set; }

        public decimal MontantTva { get; set; }

        public string MontantDh { get; set; }

        public decimal MontantTvaComprise { get; set; }

        public DateTime DateCreation { get; set; }
        public string NomberBordereau { get; set; }
        // Add these properties for dropdowns
        public List<SelectListItem> BordereauNumbers { get; set; }
        public List<SelectListItem> NLotNumbers { get; set; }
    }
    public class ActManagerEM
    {

        public Actmanager MyActmanag { get; set; }
        public List<SelectListItem> BordereauNumberOptions { get; set; }
        public Dictionary<string, List<int>> NLotOptions { get; set; }
        public string SelectedBordereauNumber { get; set; }
        public List<MyBorderauItems> BordereauItems { get; set; }
        public int SelectedNLot { get; set; }

    }
}
