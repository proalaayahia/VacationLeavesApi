using VacationLeavesApi.Data;
using VacationLeavesApi.Interfaces;

namespace VacationLeavesApi.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DnacloudDb202001benefitContext _context;
        public UnitOfWork(DnacloudDb202001benefitContext context)
        {
            _context = context;
            VacationRepository = new Repository<Vacation>(context);
        }
        public IRepository<Vacation> VacationRepository { get; private set; }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
