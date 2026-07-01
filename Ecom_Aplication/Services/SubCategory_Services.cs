using Ecom_Domain;
using modu.application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class SubCategory_Services : ISubCategoryRepository
    {
        private readonly ISubCategoryRepository _subCategoryRepository;

        public SubCategory_Services(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<IEnumerable<SubCategories>> LISTAR_SUBCATEGORY_ASYNC()
        {
            return await _subCategoryRepository.LISTAR_SUBCATEGORY_ASYNC();
        }

        public async Task<IEnumerable<SubCategories>> FILTRAR_SUBCATEGORY_ASYNC(string searchTerm, bool? statusId)
        {
            return await _subCategoryRepository.FILTRAR_SUBCATEGORY_ASYNC(searchTerm, statusId);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_SUBCATEGORY_ASYNC(
            string subCategoryName,
            string subCategoryDescription,
            int subCategoryCreatorId,
            bool subCategoryStatusId)
        {
            return await _subCategoryRepository.NUEVO_SUBCATEGORY_ASYNC(
                subCategoryName,
                subCategoryDescription,
                subCategoryCreatorId,
                subCategoryStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_SUBCATEGORY_ASYNC(
            int subCategoryId,
            string subCategoryName,
            string subCategoryDescription,
            int subCategoryModificatorId,
            bool subCategoryStatusId,
            bool forzarRecuperacion)
        {
            return await _subCategoryRepository.ACTUALIZAR_SUBCATEGORY_ASYNC(
                subCategoryId,
                subCategoryName,
                subCategoryDescription,
                subCategoryModificatorId,
                subCategoryStatusId,
                forzarRecuperacion
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_SUBCATEGORY_ASYNC(int subCategoryId, int subCategoryModificatorId)
        {
            return await _subCategoryRepository.ELIMINAR_SUBCATEGORY_ASYNC(subCategoryId, subCategoryModificatorId);
        }
    }
}