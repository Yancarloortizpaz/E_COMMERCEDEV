using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class ProductImages_Services
    {
        private readonly IProductImagesRepository _repository;

        public ProductImages_Services(IProductImagesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductImages_DTOS>> LISTAR_PRODUCTIMAGES_ASYNC()
        {
            var data = await _repository.LISTAR_PRODUCTIMAGES_ASYNC();

            return data.Select(p => new ProductImages_DTOS
            {
                productImageId = p.productImageId,
                productImageProductId = p.productImageProductId,
                productImageURL = p.productImageURL,
                productImageDescription = p.productImageDescription,
                productImageIsPrincipal = p.productImageIsPrincipal,
                productImageCreatorId = p.productImageCreatorId,
                productImageCreationDate = p.productImageCreationDate,
                productImageModificatorId = p.productImageModificatorId,
                productImageModificationDate = p.productImageModificationDate,
                productImageStatusId = p.productImageStatusId
            });
        }

        public async Task<ProductImages_DTOS?> OBTENER_PRODUCTIMAGE_BY_ID_ASYNC(int productImageId)
        {
            var data = await _repository.LISTAR_PRODUCTIMAGES_ASYNC();

            return data.Select(p => new ProductImages_DTOS
            {
                productImageId = p.productImageId,
                productImageProductId = p.productImageProductId,
                productImageURL = p.productImageURL,
                productImageDescription = p.productImageDescription,
                productImageIsPrincipal = p.productImageIsPrincipal,
                productImageCreatorId = p.productImageCreatorId,
                productImageCreationDate = p.productImageCreationDate,
                productImageModificatorId = p.productImageModificatorId,
                productImageModificationDate = p.productImageModificationDate,
                productImageStatusId = p.productImageStatusId
            }).FirstOrDefault(p => p.productImageId == productImageId);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIMAGES_ASYNC(ProductImages_DTOS dto)
        {
            return await _repository.NUEVO_PRODUCTIMAGES_ASYNC(
                dto.productImageProductId ?? 0,
                dto.productImageURL ?? "",
                dto.productImageDescription ?? "",
                dto.productImageIsPrincipal ?? false,
                dto.productImageCreatorId ?? 0,
                dto.productImageStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIMAGES_ASYNC(ProductImages_DTOS dto)
        {
            return await _repository.ACTUALIZAR_PRODUCTIMAGES_ASYNC(
                dto.productImageId ?? 0,
                dto.productImageProductId ?? 0,
                dto.productImageURL ?? "",
                dto.productImageDescription ?? "",
                dto.productImageIsPrincipal ?? false,
                dto.productImageModificatorId ?? 0,
                dto.productImageStatusId ?? false,
                false
            );
        }

        public async Task<IEnumerable<ProductImages_DTOS>> OBTENER_POR_PRODUCTO(int productId)
        {
            var data = await _repository.OBTENER_POR_PRODUCTO_PRODUCTIMAGES_ASYNC(productId);

            return data.Select(p => new ProductImages_DTOS
            {
                productImageId = p.productImageId,
                productImageProductId = p.productImageProductId,
                productImageURL = p.productImageURL,
                productImageDescription = p.productImageDescription,
                productImageIsPrincipal = p.productImageIsPrincipal,
                productImageCreatorId = p.productImageCreatorId,
                productImageCreationDate = p.productImageCreationDate,
                productImageModificatorId = p.productImageModificatorId,
                productImageModificationDate = p.productImageModificationDate,
                productImageStatusId = p.productImageStatusId
            });
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIMAGES_ASYNC(int productImageId, int productImageModificatorId)
        {
            return await _repository.ELIMINAR_PRODUCTIMAGES_ASYNC(productImageId, productImageModificatorId);
        }
    }
}