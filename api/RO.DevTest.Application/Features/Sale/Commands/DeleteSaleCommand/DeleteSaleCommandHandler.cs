namespace RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;
using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

public class DeleteSaleCommandHandler(ISaleRepository saleRepository) : IRequestHandler<DeleteSaleCommand, bool>
{
  public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
  {
    var sale = await saleRepository.GetByIdAsync(request.Id, cancellationToken);
    if (sale == null) return false;

    return await saleRepository.DeleteAsync(sale);
  }
}
