using Ecom_Aplication.Dtos;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class SubCategory_Services
    {
        private readonly ISubCategoryRepository _subCategoryRepository;

        public SubCategory_Services(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        // 1. LISTAR (Mapeado a DTO)
        public async Task<IEnumerable<SubCategories_DTOS>> LISTAR_SUBCATEGORY_ASYNC()
        {
            var data = await _subCategoryRepository.LISTAR_SUBCATEGORY_ASYNC();

            return data.Select(s => new SubCategories_DTOS
            {
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.SubCategoryName,
                SubCategoryDescription = s.SubCategoryDescription,
                SubCategoryStatusId = s.SubCategoryStatusId
                // Agrega aquí el resto de tus propiedades si tu entidad las tiene
            });
        }

        // 2. OBTENER POR ID (Reutiliza el filtro convirtiendo el int a string tal como en tu SP)
        public async Task<SubCategories_DTOS?> OBTENER_SUBCATEGORY_BY_ID_ASYNC(int subCategoryId)
        {
            var data = await _subCategoryRepository.FILTRAR_SUBCATEGORY_ASYNC(subCategoryId.ToString(), null);

            return data.Select(s => new SubCategories_DTOS
            {
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.SubCategoryName,
                SubCategoryDescription = s.SubCategoryDescription,
                SubCategoryStatusId = s.SubCategoryStatusId
            }).FirstOrDefault();
        }

        // 3. FILTRAR (Mapeado a DTO)
        public async Task<IEnumerable<SubCategories_DTOS>> FILTRAR_SUBCATEGORY_ASYNC(string searchTerm, bool? statusId)
        {
            var data = await _subCategoryRepository.FILTRAR_SUBCATEGORY_ASYNC(searchTerm ?? "", statusId);

            return data.Select(s => new SubCategories_DTOS
            {
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.SubCategoryName,
                SubCategoryDescription = s.SubCategoryDescription,
                SubCategoryStatusId = s.SubCategoryStatusId
            });
        }

        // 4. NUEVO (Recibe el DTO y lo desempaqueta para el Repositorio)
        public async Task<(int code, string message, int? templateId)> NUEVO_SUBCATEGORY_ASYNC(SubCategories_DTOS dto)
        {
            return await _subCategoryRepository.NUEVO_SUBCATEGORY_ASYNC(
                dto.SubCategoryName ?? "",
                dto.SubCategoryDescription ?? "",
                dto.SubCategoryCreatorId ?? 0,
                dto.SubCategoryStatusId ?? false
            );
        }

        // 5. ACTUALIZAR (Recibe el DTO y lo desempaqueta para el Repositorio)
        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_SUBCATEGORY_ASYNC(SubCategories_DTOS dto)
        {
            return await _subCategoryRepository.ACTUALIZAR_SUBCATEGORY_ASYNC(
                dto.SubCategoryId ?? 0,
                dto.SubCategoryName ?? "",
                dto.SubCategoryDescription ?? "",
                dto.SubCategoryModificatorId ?? 0,
                dto.SubCategoryStatusId ?? false,
                false // Forzar recuperación por defecto en false
            );
        }

        // 6. ELIMINAR
        public async Task<(int code, string message, int? templateId)> ELIMINAR_SUBCATEGORY_ASYNC(int subCategoryId, int subCategoryModificatorId)
        {
            return await _subCategoryRepository.ELIMINAR_SUBCATEGORY_ASYNC(subCategoryId, subCategoryModificatorId);
        }
    }
}