using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Providers_Services
    {
        private readonly IProviderRepository _repository;

        public Providers_Services(IProviderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Providers_DTOS>> LISTAR_PROVIDERS_ASYNC()
        {
            var data = await _repository.LISTAR_PROVIDER_ASYNC();

            return data.Select(p => new Providers_DTOS
            {
                ProviderId = p.ProviderId,
                ProviderName = p.ProviderName,
                ProviderDescription = p.ProviderDescription,
                ProviderCreatorId = p.ProviderCreatorId,
                ProviderCreationDate = p.ProviderCreationDate,
                ProviderModificatorId = p.ProviderModificatorId,
                ProviderModificationDate = p.ProviderModificationDate,
                ProviderStatusId = p.ProviderStatusId
            });
        }

        public async Task<Providers_DTOS?> OBTENER_PROVIDER_BY_ID_ASYNC(int providerId)
        {
            var data = await _repository.FILTRAR_PROVIDER_ASYNC(null, providerId);

            return data.Select(p => new Providers_DTOS
            {
                ProviderId = p.ProviderId,
                ProviderName = p.ProviderName,
                ProviderDescription = p.ProviderDescription,
                ProviderCreatorId = p.ProviderCreatorId,
                ProviderCreationDate = p.ProviderCreationDate,
                ProviderModificatorId = p.ProviderModificatorId,
                ProviderModificationDate = p.ProviderModificationDate,
                ProviderStatusId = p.ProviderStatusId
            }).FirstOrDefault(p => p.ProviderId == providerId);
        }

        public async Task<IEnumerable<Providers_DTOS>> FILTRAR_PROVIDERS_ASYNC(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_PROVIDER_ASYNC(searchTerm ?? "", null);

            return data.Select(p => new Providers_DTOS
            {
                ProviderId = p.ProviderId,
                ProviderName = p.ProviderName,
                ProviderDescription = p.ProviderDescription,
                ProviderCreatorId = p.ProviderCreatorId,
                ProviderCreationDate = p.ProviderCreationDate,
                ProviderModificatorId = p.ProviderModificatorId,
                ProviderModificationDate = p.ProviderModificationDate,
                ProviderStatusId = p.ProviderStatusId
            });
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PROVIDERS_ASYNC(Providers_DTOS dto)
        {
            return await _repository.NUEVO_PROVIDER_ASYNC(
                dto.ProviderName ?? "",
                dto.ProviderDescription ?? "",
                dto.ProviderCreatorId ?? 0,
                dto.ProviderStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PROVIDERS_ASYNC(Providers_DTOS dto)
        {
            return await _repository.ACTUALIZAR_PROVIDER_ASYNC(
                dto.ProviderId ?? 0,
                dto.ProviderName ?? "",
                dto.ProviderDescription ?? "",
                dto.ProviderModificatorId ?? 0,
                dto.ProviderStatusId ?? false,
                false
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PROVIDERS_ASYNC(int providerId, int providerModificatorId)
        {
            return await _repository.ELIMINAR_PROVIDER_ASYNC(providerId, providerModificatorId);
        }
    }
}