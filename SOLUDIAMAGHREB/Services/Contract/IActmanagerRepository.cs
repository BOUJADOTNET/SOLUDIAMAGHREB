using SOLUDIAMAGHREB.Models;

namespace SOLUDIAMAGHREB.Services.Contract
{
    public interface IActmanagerRepository : IRepository<Actmanager>
    {
        void Update(Actmanager actmanager);
        void Save();
    }
}
