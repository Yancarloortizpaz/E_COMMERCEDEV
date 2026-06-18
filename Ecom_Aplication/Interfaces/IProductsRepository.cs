using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> LISTAR_PRODUCTS_ASYNC();

        Task<IEnumerable<Products>> FILTRAR_PRODUCTS_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_PRODUCTS_ASYNC(
            string productName,
            string productDescription,
            int productProductIdentificatorId,
            int productMarkByProviderId,
            int productCreatorId,
            bool productStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTS_ASYNC(
            int productId,
            string productName,
            string productDescription,
            int productProductIdentificatorId,
            int productMarkByProviderId,
            int productModificatorId,
            bool productStatusId,
            bool ForzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTS_ASYNC(
            int productId,
            int productModificatorId
        );
    }
}