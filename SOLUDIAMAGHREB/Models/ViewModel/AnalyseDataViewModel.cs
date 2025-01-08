using Microsoft.AspNetCore.Mvc.Rendering;

namespace SOLUDIAMAGHREB.Models.ViewModel
{
    public class AnalyseDataViewModel
    {
        public string idAvisAppelOff { get; set; }
        public string NomberBordereau { get; set; }
        public DateTime DateCreation { get; set; }
        public List<SelectListItem> BordereauNumbers { get; set; }


    }
}
