using SOLUDIAMAGHREB.Data;
using SOLUDIAMAGHREB.Models;
using SOLUDIAMAGHREB.Services.Contract;

namespace SOLUDIAMAGHREB.Services.Implementation
{
    public class ActmanagerRepository : Repository<Actmanager>, IActmanagerRepository
    {
        public DbsoludiaContext _dbsoludia => _dbsoludia as DbsoludiaContext;
        public ActmanagerRepository(DbsoludiaContext Context) : base(Context)
        {
        }

        public void Save()
        {
            _dbsoludia.SaveChanges();
        }

        public void Update(Actmanager actmanager)
        {
            //Actmanager Actmang = _dbsoludia.Actmanagers.Find(actmanager.IdactEng);
            //if(Actmang != null)
            //{
            //    Actmang.FullNamePdg = actmanager.FullNamePdg;
            //    Actmang.AdresseLaSociété = actmanager.AdresseLaSociété;
            //    Actmang.Qualité = actmanager.Qualité;
            //    Actmang.Capital = actmanager.Capital;
            //    Actmang.NomPersonnel = actmanager.NomPersonnel;
            //    Actmang.DateCreation = actmanager.DateCreation;
            //    Actmang.NCnss = actmanager.NCnss;
            //    Actmang.DateCreation = actmanager.DateCreation;
            //    Actmang.MontantDh = actmanager.MontantDh;
            //    Actmang.MontantHtTva = actmanager.MontantHtTva;
            //    Actmang.MontantTva = actmanager.MontantTva;
            //    Actmang.MontantTvaComprise = actmanager.MontantTvaComprise;
            //    Actmang.NPatente = actmanager.NPatente;
            //    Actmang.TauxTva = actmanager.TauxTva;
            //    Actmang.NRegistredeCommerce = actmanager.NRegistredeCommerce;
            //    //Actmang.TauxTva = actmanager.TauxTva;
            //}
        }
    }
}
