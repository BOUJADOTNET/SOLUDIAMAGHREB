using SOLUDIAMAGHREB.Models;

namespace SOLUDIAMAGHERB.Services.Contract
{
    public interface IUserService
    {
        Task<Utilisateur> GetUser(string username, string clave);
        Task<Utilisateur> SaveUser(Utilisateur model);

    }
}
