using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class MarkByProviders_Services
    {
        private readonly IMarkByProviders _markByProvidersRepository;

        public MarkByProviders_Services(IMarkByProviders markByProvidersRepository)
        {
            _markByProvidersRepository = markByProvidersRepository;
        }

        public async Task<IEnumerable<MarkByProviders>> LISTAR_MARKBYPROVIDERS_ASYNC()
        {
            return await _markByProvidersRepository.LISTAR_MARKBYPROVIDERS_ASYNC();
        }

        public async Task<MarkByProviders?> OBTENER_MARKBYPROVIDER_BY_ID_ASYNC(int id)
        {
            var data = await _markByProvidersRepository.FILTRAR_MARKBYPROVIDERS_ASYNC(id, null, null, null, null, null, null, null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<MarkByProviders>> FILTRAR_MARKBYPROVIDERS_ASYNC(
            int? markByProviderId,
            int? markByProviderMarkId,
            int? markByProviderProviderId,
            int? markByProviderCreatorId,
            DateTime? markByProviderCreationDate,
            int? markByProviderModificatorId,
            DateTime? markByProviderModificationDate,
            bool? markByProviderStatusId)
        {
            return await _markByProvidersRepository.FILTRAR_MARKBYPROVIDERS_ASYNC(
                markByProviderId,
                markByProviderMarkId,
                markByProviderProviderId,
                markByProviderCreatorId,
                markByProviderCreationDate,
                markByProviderModificatorId,
                markByProviderModificationDate,
                markByProviderStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_MARKBYPROVIDERS_ASYNC(MarkByProviders_DTOS dto)
        {
            return await _markByProvidersRepository.NUEVO_MARKBYPROVIDERS_ASYNC(
                dto.MarkByProviderMarkId ?? 0,
                dto.MarkByProviderProviderId ?? 0,
                dto.MarkByProviderCreatorId ?? 0,
                dto.MarkByProviderModificatorId,
                dto.MarkByProviderStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_MARKBYPROVIDERS_ASYNC(MarkByProviders_DTOS dto)
        {
            return await _markByProvidersRepository.ACTUALIZAR_MARKBYPROVIDERS_ASYNC(
                dto.MarkByProviderId ?? 0,
                dto.MarkByProviderMarkId ?? 0,
                dto.MarkByProviderProviderId ?? 0,
                dto.MarkByProviderCreatorId ?? 0,
                dto.MarkByProviderModificatorId ?? 0,
                dto.MarkByProviderStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_MARKBYPROVIDERS_ASYNC(MarkByProviders_DTOS dto)
        {
            return await _markByProvidersRepository.ELIMINAR_MARKBYPROVIDERS_ASYNC(
                dto.MarkByProviderId ?? 0,
                dto.MarkByProviderMarkId ?? 0,
                dto.MarkByProviderProviderId ?? 0,
                dto.MarkByProviderCreatorId ?? 0,
                dto.MarkByProviderModificatorId ?? 0,
                dto.MarkByProviderStatusId ?? false
            );
        }
    }
}