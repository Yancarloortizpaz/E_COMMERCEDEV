using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IMarkByProviders
    {
        Task<IEnumerable<MarkByProviders>> LISTAR_MARKBYPROVIDERS_ASYNC();

        Task<IEnumerable<MarkByProviders>> FILTRAR_MARKBYPROVIDERS_ASYNC(
            int? markByProviderId,
            int? markByProviderMarkId,
            int? markByProviderProviderId,
            int? markByProviderCreatorId,
            DateTime? markByProviderCreationDate,
            int? markByProviderModificatorId,
            DateTime? markByProviderModificationDate,
            bool? markByProviderStatusId
        );

        Task<(int code, string message, int? templateId)> NUEVO_MARKBYPROVIDERS_ASYNC(
            int markByProviderMarkId,
            int markByProviderProviderId,
            int markByProviderCreatorId,
            int? markByProviderModificatorId,
            bool markByProviderStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_MARKBYPROVIDERS_ASYNC(
            int markByProviderId,
            int markByProviderMarkId,
            int markByProviderProviderId,
            int markByProviderCreatorId,
            int markByProviderModificatorId,
            bool markByProviderStatusId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_MARKBYPROVIDERS_ASYNC(
            int markByProviderId,
            int markByProviderMarkId,
            int markByProviderProviderId,
            int markByProviderCreatorId,
            int markByProviderModificatorId,
            bool markByProviderStatusId
        );
    }
}