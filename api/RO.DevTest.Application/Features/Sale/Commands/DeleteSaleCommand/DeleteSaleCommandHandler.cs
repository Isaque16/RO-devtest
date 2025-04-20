namespace RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;

using MediatR;
using Contracts.Persistance.Repositories;

/// <summary>
/// Handles the deletion of a sale based on the provided command.
/// This command handler processes <see cref="DeleteSaleCommand"/> to delete a sale entity.
/// </summary>
/// <remarks>
/// It retrieves the sale entity using <see cref="ISaleRepository.GetByIdAsync"/>
/// and performs the delete operation via <see cref="ISaleRepository.DeleteAsync"/>.
/// Returns a boolean indicating whether the deletion was successful or not.
/// </remarks>
public class DeleteSaleCommandHandler(ISaleRepository saleRepo) 
  : IRequestHandler<DeleteSaleCommand, bool>
{
  /// <summary>
  /// Handles the deletion of a sale based on the provided command.
  /// </summary>
  /// <param name="request">The command containing the ID of the sale to be deleted.</param>
  /// <param name="cancellationToken">The cancellation token to observe during the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
  public async Task<bool> Handle(
    DeleteSaleCommand request, CancellationToken cancellationToken)
  {
    var sale = await saleRepo.GetByIdAsync(request.Id, cancellationToken);
    if (sale == null) return false;

    return await saleRepo.DeleteAsync(sale);
  }
}
