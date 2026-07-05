using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class ProductIdentificators_Services
    {
        private readonly IProductIdentificators _productIdentificatorsRepository;

        public ProductIdentificators_Services(IProductIdentificators productIdentificatorsRepository)
        {
            _productIdentificatorsRepository = productIdentificatorsRepository;
        }

        public async Task<IEnumerable<ProductIdentificators_DTOS>> LISTAR_PRODUCTIDENTIFICATORS_ASYNC()
        {
            return await _productIdentificatorsRepository.LISTAR_PRODUCTIDENTIFICATORS_ASYNC();
        }

        public async Task<ProductIdentificators_DTOS?> OBTENER_PRODUCTIDENTIFICATOR_BY_ID_ASYNC(int id)
        {
            var data = await _productIdentificatorsRepository.FILTRAR_PRODUCTIDENTIFICATORS_ASYNC(id, null, null, null, null, null, null, null, null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<ProductIdentificators_DTOS>> FILTRAR_PRODUCTIDENTIFICATORS_ASYNC(
            int? productIdentificatorId,
            int? productIdentificatorCategoryId,
            int? productIdentificatorSubCategoryId,
            int? productIdentificatorSegmentId,
            int? productIdentificatorCreatorId,
            DateTime? productIdentificatorCreationDate,
            int? productIdentificatorModificatorId,
            DateTime? productIdentificatorModificationDate,
            bool? productIdentificatorStatusId)
        {
            return await _productIdentificatorsRepository.FILTRAR_PRODUCTIDENTIFICATORS_ASYNC(
                productIdentificatorId,
                productIdentificatorCategoryId,
                productIdentificatorSubCategoryId,
                productIdentificatorSegmentId,
                productIdentificatorCreatorId,
                productIdentificatorCreationDate,
                productIdentificatorModificatorId,
                productIdentificatorModificationDate,
                productIdentificatorStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIDENTIFICATORS_ASYNC(ProductIdentificators_DTOS dto)
        {
            return await _productIdentificatorsRepository.NUEVO_PRODUCTIDENTIFICATORS_ASYNC(
                dto.ProductIdentificatorCategoryId ?? 0,
                dto.ProductIdentificatorSubCategoryId ?? 0,
                dto.ProductIdentificatorSegmentId ?? 0,
                dto.ProductIdentificatorCreatorId ?? 0,
                dto.ProductIdentificatorModificatorId,
                dto.ProductIdentificatorStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIDENTIFICATORS_ASYNC(ProductIdentificators_DTOS dto)
        {
            return await _productIdentificatorsRepository.ACTUALIZAR_PRODUCTIDENTIFICATORS_ASYNC(
                dto.ProductIdentificatorId ?? 0,
                dto.ProductIdentificatorCategoryId ?? 0,
                dto.ProductIdentificatorSubCategoryId ?? 0,
                dto.ProductIdentificatorSegmentId ?? 0,
                dto.ProductIdentificatorCreatorId ?? 0,
                dto.ProductIdentificatorModificatorId ?? 0,
                dto.ProductIdentificatorStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIDENTIFICATORS_ASYNC(ProductIdentificators_DTOS dto)
        {
            return await _productIdentificatorsRepository.ELIMINAR_PRODUCTIDENTIFICATORS_ASYNC(
                dto.ProductIdentificatorId ?? 0,
                dto.ProductIdentificatorCategoryId ?? 0,
                dto.ProductIdentificatorSubCategoryId ?? 0,
                dto.ProductIdentificatorSegmentId ?? 0,
                dto.ProductIdentificatorCreatorId ?? 0,
                dto.ProductIdentificatorModificatorId ?? 0,
                dto.ProductIdentificatorStatusId ?? false
            );
        }
    }
}