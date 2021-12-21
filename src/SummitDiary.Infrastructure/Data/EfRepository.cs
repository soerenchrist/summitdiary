using System.Linq.Expressions;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SummitDiary.SharedKernel.Interfaces;

namespace SummitDiary.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;

    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken)
    {
        return _dbContext.Set<T>().SumAsync(selector, cancellationToken);
    }
    
    public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken)
    {
        return _dbContext.Set<T>().SumAsync(selector, cancellationToken);
    }
}