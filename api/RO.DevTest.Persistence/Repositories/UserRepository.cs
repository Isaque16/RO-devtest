namespace RO.DevTest.Persistence.Repositories;

using RO.DevTest.Application.Contracts.Persistance.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class UserRepository(DefaultContext context)
    : BaseRepository<User>(context), IUserRepository;
