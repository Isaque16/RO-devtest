namespace RO.DevTest.Persistence.Repositories;

using RO.DevTest.Application.Contracts.Persistance.Repositories;
using Domain.Entities;


public class ProductRepository(DefaultContext context)
    : BaseRepository<Product>(context), IProductRepository;
