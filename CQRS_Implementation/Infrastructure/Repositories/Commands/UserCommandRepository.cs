using CQRS_Implementation.Infrastructure.Data.MariaDb;
using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace CQRS_Implementation.Infrastructure.Repositories.Commands;

public class UserCommandRepository(MariaDbContext dbContext) : IUserCommandRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
    }
        
    public async Task AddAsync(User? user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
        
    public async Task UpdateAsync(User? user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
        
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
        
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AnyAsync(u => u!.Email == email, cancellationToken);
    }
}