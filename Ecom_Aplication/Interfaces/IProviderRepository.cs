using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface IProviderRepository
    {
     
        Task<IEnumerable<Providers>> LISTAR_PROVIDER_ASYNC();
        Task<IEnumerable<Providers>> FILTRAR_PROVIDER_ASYNC(string searchTerm, int? providerId);
        Task<(int code, string message, int? templateId)> NUEVO_PROVIDER_ASYNC(
            string providerName,
            string providerDescription,
            int providerCreatorId,
            bool providerStatusId
        );
        Task<(int code, string message, int? templateId)> ACTUALIZAR_PROVIDER_ASYNC(
            int providerId,
            string providerName,
            string providerDescription,
            int providerModificatorId,
            bool providerStatusId,
            bool forzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PROVIDER_ASYNC(int providerId, int providerModificatorId);
    }
}