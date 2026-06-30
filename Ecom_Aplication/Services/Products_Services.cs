using Ecom_Aplication.Dtos; 
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Products_Services
    {
        private readonly IProductsRepository _repository;

        public Products_Services(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTS_ASYNC(Products_DTOS dto)
        {
            return await _repository.NUEVO_PRODUCTS_ASYNC(
                dto.productName,
                dto.productDescription,
                dto.productProductIdentificatorId ?? 0,
                dto.productMarkByProviderId ?? 0,
                dto.productCreatorId ?? 0,
                dto.productStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTS_ASYNC(Products_DTOS dto)
        {
            return await _repository.ACTUALIZAR_PRODUCTS_ASYNC(
                dto.productId ?? 0,
                dto.productName,
                dto.productDescription,
                dto.productProductIdentificatorId ?? 0,
                dto.productMarkByProviderId ?? 0,
                dto.productModificatorId ?? 0,
                dto.productStatusId ?? false,
                false 
            );
        }

        public async Task<IEnumerable<Products_DTOS>> LISTAR_PRODUCTS()
        {
            var data = await _repository.LISTAR_PRODUCTS_ASYNC();

            return data.Select(p => new Products_DTOS
            {
                productId = p.productId,
                productName = p.productName,
                productDescription = p.productDescription,
                productProductIdentificatorId = p.productProductIdentificatorId,
                productMarkByProviderId = p.productMarkByProviderId,
                productCreatorId = p.productCreatorId,
                productCreationDate = p.productCreationDate,
                productModificatorId = p.productModificatorId,
                productModificationDate = p.productModificationDate,
                productStatusId = p.productStatusId
            });
        }

        public async Task<Products_DTOS?> Obtener_Products_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_PRODUCTS_ASYNC(searchTerm, statusId);

            return data.Select(p => new Products_DTOS
            {
                productId = p.productId,
                productName = p.productName,
                productDescription = p.productDescription,
                productProductIdentificatorId = p.productProductIdentificatorId,
                productMarkByProviderId = p.productMarkByProviderId,
                productCreatorId = p.productCreatorId,
                productCreationDate = p.productCreationDate,
                productModificatorId = p.productModificatorId,
                productModificationDate = p.productModificationDate,
                productStatusId = p.productStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_Products(int productId, int productModificatorId)
        {
            return await _repository.ELIMINAR_PRODUCTS_ASYNC(productId, productModificatorId);
        }
    }
}
