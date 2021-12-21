using System.Linq.Expressions;
using Ardalis.Specification;

namespace SummitDiary.SharedKernel.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken);
    public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken);
}