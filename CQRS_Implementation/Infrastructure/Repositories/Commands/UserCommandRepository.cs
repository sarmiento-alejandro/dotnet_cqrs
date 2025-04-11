using CQRS_Implementation.Infrastructure.Data.MariaDb;
using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace CQRS_Implementation.Infrastructure.Repositories.Commands;

public class UserCommandRepository : IUserCommandRepository
{
    private readonly MariaDbContext _dbContext;
        
    public UserCommandRepository(MariaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
    }
        
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
        
    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
        
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
        
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}