namespace SOLUDIAMAGHREB.Models
{
    public class MyBorderauItems
    {
        public int id { get; set; }

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

        public string AppelOffres { get; set; }
        public string NomberBordereau { get; set; }



    }
}
