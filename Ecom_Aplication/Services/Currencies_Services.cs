using Ecom_Aplication.Dtos; 
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Currencies_Services
    {
        private readonly ICurrenciesRepository _repository;

        public Currencies_Services(ICurrenciesRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_CURRENCIES_ASYNC(Currencies_DTOS dto)
        {
            return await _repository.NUEVO_CURRENCIES_ASYNC(
                dto.currencyName,
                dto.currencyISO,
                dto.currencyCode ?? 0,
                dto.currencyDescription,
                dto.currencyCreatorId ?? 0,
                dto.currencyStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_CURRENCIES_ASYNC(Currencies_DTOS dto)
        {
            return await _repository.ACTUALIZAR_CURRENCIES_ASYNC(
                dto.currencyId ?? 0,
                dto.currencyName,
                dto.currencyISO,
                dto.currencyCode ?? 0,
                dto.currencyDescription,
                dto.currencyModificatorId ?? 0,
                dto.currencyStatusId ?? false,
                false 
            );
        }

        public async Task<IEnumerable<Currencies_DTOS>> LISTAR_CURRENCIES()
        {
            var data = await _repository.LISTAR_CURRENCIES_ASYNC();

            return data.Select(c => new Currencies_DTOS
            {
                currencyId = c.currencyId,
                currencyName = c.currencyName,
                currencyISO = c.currencyISO,
                currencyCode = c.currencyCode,
                currencyDescription = c.currencyDescription,
                currencyCreatorId = c.currencyCreatorId,
                currencyCreationDate = c.currencyCreationDate,
                currencyModificatorId = c.currencyModificatorId,
                currencyModificationDate = c.currencyModificationDate,
                currencyStatusId = c.currencyStatusId
            });
        }

        public async Task<Currencies_DTOS?> Obtener_Currencies_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_CURRENCIES_ASYNC(searchTerm, statusId);

            return data.Select(c => new Currencies_DTOS
            {
                currencyId = c.currencyId,
                currencyName = c.currencyName,
                currencyISO = c.currencyISO,
                currencyCode = c.currencyCode,
                currencyDescription = c.currencyDescription,
                currencyCreatorId = c.currencyCreatorId,
                currencyCreationDate = c.currencyCreationDate,
                currencyModificatorId = c.currencyModificatorId,
                currencyModificationDate = c.currencyModificationDate,
                currencyStatusId = c.currencyStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_Currencies(int currencyId, int currencyModificatorId)
        {
            return await _repository.ELIMINAR_CURRENCIES_ASYNC(currencyId, currencyModificatorId);
        }
    }
}
