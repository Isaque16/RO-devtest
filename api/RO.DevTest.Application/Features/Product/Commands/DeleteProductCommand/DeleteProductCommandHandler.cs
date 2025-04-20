using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, bool>
{
  public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
  {
    if (request.Id == Guid.Empty) return false;
    
    var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
    if (product == null) return false;

    return await productRepository.DeleteAsync(product);
  }
}
