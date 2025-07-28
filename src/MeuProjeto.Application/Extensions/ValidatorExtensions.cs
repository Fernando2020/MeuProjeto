using FluentValidation.Results;

namespace MeuProjeto.Application.Extensions
{
    public static class ValidatorExtensions
    {
        public static List<string> ToStringList(this List<ValidationFailure>? value) =>
            value == null ? new() : value.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
    }
}
