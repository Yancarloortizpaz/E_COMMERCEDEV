using Ecom_Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IStatus
    {
        Task<IEnumerable<Status_DTOS>> LISTAR_STATUS_ASYNC();

        Task<IEnumerable<Status_DTOS>> FILTRAR_STATUS_ASYNC(
            int? statusId,
            string statusName,
            int? statusCreatorId,
            DateTime? statusCreationDate,
            int? statusStatusId
        );

        Task<(int code, string message, int? templateId)> NUEVO_STATUS_ASYNC(
            string statusName,
            int statusCreatorId,
            int statusStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_STATUS_ASYNC(
            int statusId,
            string statusName,
            int statusCreatorId,
            int statusStatusId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_STATUS_ASYNC(
            int statusId,
            int statusStatusId
        );
    }
}