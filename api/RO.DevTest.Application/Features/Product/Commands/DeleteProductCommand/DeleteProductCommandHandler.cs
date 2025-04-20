namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

using MediatR;
using Contracts.Persistance.Repositories;

public class DeleteProductCommandHandler(IProductRepository productRepository) 
  : IRequestHandler<DeleteProductCommand, bool>
{
  public async Task<bool> Handle(
    DeleteProductCommand request, CancellationToken cancellationToken)
  {
    if (request.Id == Guid.Empty) return false;
    
    var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
    if (product == null) return false;

    return await productRepository.DeleteAsync(product);
  }
}
