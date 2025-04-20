namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

public class CreateSaleCommandHandler(ISaleRepository saleRepository) : IRequestHandler<CreateSaleCommand, Sale>
{
  public async Task<Sale> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
  {
    CreateSaleCommandValidator validator = new();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      throw new BadRequestException(validationResult);
    
    var sale = request.AssignTo();

    return await saleRepository.CreateAsync(sale, cancellationToken);
  }
}
