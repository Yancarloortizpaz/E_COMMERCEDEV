using Ecom_Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IProductIdentificators
    {
        Task<IEnumerable<ProductIdentificators_DTOS>> LISTAR_PRODUCTIDENTIFICATORS_ASYNC();

        Task<IEnumerable<ProductIdentificators_DTOS>> FILTRAR_PRODUCTIDENTIFICATORS_ASYNC(
            int? productIdentificatorId,
            int? productIdentificatorCategoryId,
            int? productIdentificatorSubCategoryId,
            int? productIdentificatorSegmentId,
            int? productIdentificatorCreatorId,
            DateTime? productIdentificatorCreationDate,
            int? productIdentificatorModificatorId,
            DateTime? productIdentificatorModificationDate,
            bool? productIdentificatorStatusId
        );

        Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int? productIdentificatorModificatorId,
            bool productIdentificatorStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorId,
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int productIdentificatorModificatorId,
            bool productIdentificatorStatusId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorId,
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int productIdentificatorModificatorId,
            bool productIdentificatorStatusId
        );
    }
}
