using Microsoft.AspNetCore.Mvc.Rendering;

namespace SOLUDIAMAGHREB.Models.ViewModel
{
    public class ViewModelDeclarationLh
    {
        public int IdDeclar { get; set; }
        public string NomberBordereau { get; set; }
        public string AppelOffer { get; set; }
        public string ObjectMarche { get; set; }
        public string Capital { get; set; }
        public DateTime DateCreation { get; set; }
        public List<SelectListItem> BordereauNumbers { get; set; }
        public List<SelectListItem> NLotNumbers { get; set; }
    }
}
