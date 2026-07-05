using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IProductImagesRepository
    {
        Task<IEnumerable<ProductImages>> LISTAR_PRODUCTIMAGES_ASYNC();

        Task<IEnumerable<ProductImages>> OBTENER_POR_PRODUCTO_PRODUCTIMAGES_ASYNC(int ProductId);

        Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIMAGES_ASYNC(
            int productImageProductId,
            string productImageURL,
            string productImageDescription,
            bool productImageIsPrincipal,
            int productImageCreatorId,
            bool productImageStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIMAGES_ASYNC(
            int productImageId,
            int productImageProductId,
            string productImageURL,
            string productImageDescription,
            bool productImageIsPrincipal,
            int productImageModificatorId,
            bool productImageStatusId,
            bool ForzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIMAGES_ASYNC(
            int productImageId,
            int productImageModificatorId
        );
    }
}