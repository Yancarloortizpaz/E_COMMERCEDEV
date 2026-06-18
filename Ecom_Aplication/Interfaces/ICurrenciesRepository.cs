using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Interfaces
{
    public interface ICurrenciesRepository
    {
        Task<IEnumerable<Currencies>> LISTAR_CURRENCIES_ASYNC();

        Task<IEnumerable<Currencies>> FILTRAR_CURRENCIES_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_CURRENCIES_ASYNC(
            string currencyName,
            string currencyISO,
            int currencyCode,
            string currencyDescription,
            int currencyCreatorId,
            bool currencyStatusId
            );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_CURRENCIES_ASYNC(
            int currencyId,
            string currencyName,
            string currencyISO,
            int currencyCode,
            string currencyDescription,
            int currencyModificatorId,
            bool currencyStatusId,
            bool ForzarRecuperacion
            );
        Task<(int code, string message, int? templateId)> ELIMINAR_CURRENCIES_ASYNC(int currencyId, int currencyModificatorId);
    }
}