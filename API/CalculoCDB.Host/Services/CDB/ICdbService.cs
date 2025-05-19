using CalculoCDB.API.Models.CDB;
using FluentValidation.Results;

namespace CalculoCDB.API.Services.CDB
{
    public interface ICdbService
    {
        /// <summary>
        /// Executa o cálculo do CDB a partir de um valor inicial e um prazo em meses.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        Task<CdbResult> CalculateCDBAsync(decimal initialValue, int months);

        /// <summary>
        /// Verifica se o valores informados na requisição são válidos.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ValidationResult> ValidateRequestAsync(CdbRequest request);
    }
}
