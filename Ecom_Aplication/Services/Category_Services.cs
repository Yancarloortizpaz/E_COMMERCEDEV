using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Category_Services
    {
        private readonly ICategoryRepository _repository;

        public Category_Services(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_CATEGORY_ASYNC(Categories_DTOS dto)
        {
            
            return await _repository.NUEVO_CATEGORY_ASYNC(
                dto.CategoryName,
                dto.CategoryDescription,
                dto.CategoryCreatorId ?? 0, 
                dto.CategoryStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_CATEGORY_ASYNC(Categories_DTOS dto)
        {
            
            return await _repository.ACTUALIZAR_CATEGORY_ASYNC(
                dto.CategoryId ?? 0,
                dto.CategoryName,
                dto.CategoryDescription,
                dto.CategoryModificatorId ?? 0,
                dto.CategoryStatusId ?? false,
                false 
            );
        }

        public async Task<IEnumerable<Categories_DTOS>> LISTAR_CATEGORY()
        {
           
            var data = await _repository.LISTAR_CATEGORY_ASYNC();

           
            return data.Select(p => new Categories_DTOS
            {
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,
                CategoryDescription = p.CategoryDescription,
                CategoryCreatorId = p.CategoryCreatorId,
                CategoryCreationDate = p.CategoryCreationDate,
                CategoryModificatorId = p.CategoryModificatorId,
                CategoryModificationDate = p.CategoryModificationDate,
                CategoryStatusId = p.CategoryStatusId,
            });
        }

        public async Task<Categories_DTOS?> Obtener_Categories_Por_Id(string searchTerm, bool? statusId)
        {
            
            var data = await _repository.FILTRAR_CATEGORY_ASYNC(searchTerm, statusId);

            return data.Select(item => new Categories_DTOS
            {
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName,
                CategoryDescription = item.CategoryDescription,
                CategoryCreatorId = item.CategoryCreatorId,
                CategoryCreationDate = item.CategoryCreationDate,
                CategoryModificatorId = item.CategoryModificatorId,
                CategoryModificationDate = item.CategoryModificationDate,
                CategoryStatusId = item.CategoryStatusId,
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_Category(int categoryId, int categoryModificatorId)
        {
            return await _repository.ELIMINAR_CATEGORY_ASYNC(categoryId, categoryModificatorId);
        }
    }
}