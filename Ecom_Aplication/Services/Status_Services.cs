using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Status_Services
    {
        private readonly IStatus _statusRepository;

        public Status_Services(IStatus statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<Status_DTOS>> LISTAR_STATUS_ASYNC()
        {
            return await _statusRepository.LISTAR_STATUS_ASYNC();
        }

        public async Task<Status_DTOS?> OBTENER_STATUS_BY_ID_ASYNC(int statusId)
        {
            var data = await _statusRepository.FILTRAR_STATUS_ASYNC(statusId, "", null, null, null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<Status_DTOS>> FILTRAR_STATUS_ASYNC(
            int? statusId,
            string? statusName,
            int? statusCreatorId,
            DateTime? statusCreationDate,
            int? statusStatusId)
        {
            return await _statusRepository.FILTRAR_STATUS_ASYNC(
                statusId,
                statusName ?? "",
                statusCreatorId,
                statusCreationDate,
                statusStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STATUS_ASYNC(Status_DTOS dto)
        {
            return await _statusRepository.NUEVO_STATUS_ASYNC(
                dto.StatusName ?? "",
                dto.StatusCreatorId ?? 0,
                dto.StatusStatusId ?? 0
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_STATUS_ASYNC(Status_DTOS dto)
        {
            return await _statusRepository.ACTUALIZAR_STATUS_ASYNC(
                dto.StatusId ?? 0,
                dto.StatusName ?? "",
                dto.StatusCreatorId ?? 0,
                dto.StatusStatusId ?? 0
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_STATUS_ASYNC(int statusId, int statusStatusId)
        {
            return await _statusRepository.ELIMINAR_STATUS_ASYNC(statusId, statusStatusId);
        }
    }
}