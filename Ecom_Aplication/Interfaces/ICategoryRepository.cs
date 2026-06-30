using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CATEGORIES>> LISTAR_CATEGORY_ASYNC();

        Task<IEnumerable<CATEGORIES>> FILTRAR_CATEGORY_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_CATEGORY_ASYNC(
            string categoryName,
            string categoryDescription,
            int categoryCreatorId,
            bool categoryStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_CATEGORY_ASYNC(
            int categoryId,
            string categoryName,
            string categoryDescription,
            int categoryModificatorId,
            bool categoryStatusId,
            bool forzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_CATEGORY_ASYNC(int categoryId, int categoryModificatorId);
    }
}
