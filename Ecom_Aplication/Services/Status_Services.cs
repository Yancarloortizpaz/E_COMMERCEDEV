using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Status_Services : IStatus
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

        public async Task<IEnumerable<Status_DTOS>> FILTRAR_STATUS_ASYNC(
            int? statusId,
            string statusName,
            int? statusCreatorId,
            DateTime? statusCreationDate,
            int? statusStatusId)
        {
            return await _statusRepository.FILTRAR_STATUS_ASYNC(
                statusId,
                statusName,
                statusCreatorId,
                statusCreationDate,
                statusStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STATUS_ASYNC(
            string statusName,
            int statusCreatorId,
            int statusStatusId)
        {
            return await _statusRepository.NUEVO_STATUS_ASYNC(statusName, statusCreatorId, statusStatusId);
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_STATUS_ASYNC(
            int statusId,
            string statusName,
            int statusCreatorId,
            int statusStatusId)
        {
            return await _statusRepository.ACTUALIZAR_STATUS_ASYNC(statusId, statusName, statusCreatorId, statusStatusId);
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_STATUS_ASYNC(
            int statusId,
            int statusStatusId)
        {
            return await _statusRepository.ELIMINAR_STATUS_ASYNC(statusId, statusStatusId);
        }
    }
}
