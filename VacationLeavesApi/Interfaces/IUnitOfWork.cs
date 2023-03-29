using VacationLeavesApi.Data;

namespace VacationLeavesApi.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<Vacation> VacationRepository { get; }
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
