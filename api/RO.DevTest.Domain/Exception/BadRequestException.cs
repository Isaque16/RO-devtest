using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace RO.DevTest.Domain.Exception;

/// <summary>
/// Retorna um <see cref="HttpStatusCode.BadRequest"/> para a solicitação.
/// </summary>
public class BadRequestException : ApiException {
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    // Propriedade para armazenar erros padronizados
    public new IEnumerable<string> Errors { get; }

    public BadRequestException(IdentityResult result) : base("Erro de solicitação inválida.") {
        Errors = result.Errors.Select(e => e.Description);
    }

    public BadRequestException(string error) : base(error) {
        Errors = new List<string> { error };
    }

    public BadRequestException(ValidationResult validationResult) : base("Erro de validação.") {
        Errors = validationResult.Errors.Select(e => e.ErrorMessage);
    }
}
