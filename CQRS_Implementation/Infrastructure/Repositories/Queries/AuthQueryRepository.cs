using CQRS_Implementation.Infrastructure.Data.MariaDb;
using CQRS_Implementation.Domain.Entities;
using CQRS_Implementation.Domain.Repositories.Queries;
using Microsoft.EntityFrameworkCore;


namespace CQRS_Implementation.Infrastructure.Repositories.Queries;

public class AuthQueryRepository(MariaDbContext dbContext) : IAuthQueryRepository
{
    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u!.Email == email, cancellationToken);
    }
}