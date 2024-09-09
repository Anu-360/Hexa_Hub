using Hexa_Hub.Interface;

namespace Hexa_Hub.Repository
{
    public class AuditRepo : IAuditRepo
    {
        public Task AddAuditReq(Audit audit)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAuditReq(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Audit>> GetAllAudits()
        {
            throw new NotImplementedException();
        }

        public Task<Audit?> GetAuditById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task<Audit> UpdateAudit(Audit audit)
        {
            throw new NotImplementedException();
        }
    }
}
