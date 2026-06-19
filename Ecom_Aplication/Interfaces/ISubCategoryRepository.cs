using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface ISubCategoryRepository
    {

        Task<IEnumerable<SubCategories>> LISTAR_SUBCATEGORY_ASYNC();

        Task<IEnumerable<SubCategories>> FILTRAR_SUBCATEGORY_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_SUBCATEGORY_ASYNC(
            string subCategoryName,
            string subCategoryDescription,
            int subCategoryCreatorId,
            bool subCategoryStatusId
        );
        Task<(int code, string message, int? templateId)> ACTUALIZAR_SUBCATEGORY_ASYNC(
            int subCategoryId,
            string subCategoryName,
            string subCategoryDescription,
            int subCategoryModificatorId,
            bool subCategoryStatusId,
            bool forzarRecuperacion
        );
        Task<(int code, string message, int? templateId)> ELIMINAR_SUBCATEGORY_ASYNC(int subCategoryId, int subCategoryModificatorId);
    }
}