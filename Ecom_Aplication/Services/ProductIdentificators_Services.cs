using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class ProductIdentificators_Services : IProductIdentificators
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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int? productIdentificatorModificatorId,
            bool productIdentificatorStatusId)
        {
            return await _productIdentificatorsRepository.NUEVO_PRODUCTIDENTIFICATORS_ASYNC(
                productIdentificatorCategoryId,
                productIdentificatorSubCategoryId,
                productIdentificatorSegmentId,
                productIdentificatorCreatorId,
                productIdentificatorModificatorId,
                productIdentificatorStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorId,
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int productIdentificatorModificatorId,
            bool productIdentificatorStatusId)
        {
            return await _productIdentificatorsRepository.ACTUALIZAR_PRODUCTIDENTIFICATORS_ASYNC(
                productIdentificatorId,
                productIdentificatorCategoryId,
                productIdentificatorSubCategoryId,
                productIdentificatorSegmentId,
                productIdentificatorCreatorId,
                productIdentificatorModificatorId,
                productIdentificatorStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorId,
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int productIdentificatorModificatorId,
            bool productIdentificatorStatusId)
        {
            return await _productIdentificatorsRepository.ELIMINAR_PRODUCTIDENTIFICATORS_ASYNC(
                productIdentificatorId,
                productIdentificatorCategoryId,
                productIdentificatorSubCategoryId,
                productIdentificatorSegmentId,
                productIdentificatorCreatorId,
                productIdentificatorModificatorId,
                productIdentificatorStatusId
            );
        }
    }
}
