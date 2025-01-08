using Microsoft.EntityFrameworkCore;
using SOLUDIAMAGHERB.Services.Contract;
using SOLUDIAMAGHREB.Data;
using SOLUDIAMAGHREB.Models;

namespace SOLUDIAMAGHERB.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly DbsoludiaContext _dbContext;
        public UserService(DbsoludiaContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Utilisateur> GetUser(string username, string clave)
        {
            Utilisateur user_found = await _dbContext.Utilisateurs.Where(u => u.UserName == username && u.Clave == clave)
               .FirstOrDefaultAsync();

            return user_found;
        }

        public async Task<Utilisateur> SaveUser(Utilisateur model)
        {
            _dbContext.Utilisateurs.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
