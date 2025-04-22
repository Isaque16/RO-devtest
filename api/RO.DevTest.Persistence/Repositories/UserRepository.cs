namespace RO.DevTest.Persistence.Repositories;

using RO.DevTest.Application.Contracts.Persistance.Repositories;
using Domain.Entities;

public class UserRepository(DefaultContext context)
    : BaseRepository<User>(context), IUserRepository;
