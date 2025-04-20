namespace RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;

using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

public class UpdateSaleCommandHandler(ISaleRepository saleRepository) : IRequestHandler<UpdateSaleCommand, Sale>
{
  public async Task<Sale> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
  {
    UpdateSaleCommandValidator validator = new();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid) 
      throw new BadRequestException(validationResult);

    var sale = await saleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Sale with ID {request.Id} not found.");
    sale = request.AssignToSale(sale);
    
    return await saleRepository.UpdateAsync(sale) ?? throw new Exception($"Failed to update sale with ID {request.Id}.");
  }
}