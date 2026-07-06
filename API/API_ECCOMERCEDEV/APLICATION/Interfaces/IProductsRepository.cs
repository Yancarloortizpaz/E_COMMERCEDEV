using DOMAIN.Products;
using DOMAIN.VariablesSalida;

namespace APLICATION.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<DM_Products_listar>> Listar_ProductsAsync();
        Task<IEnumerable<DM_Products_filtrar>> Filtrar_ProductsAsync(string? searchTerm);
        Task<OUTPUT> Insertar_ProductsAsync(DM_Products_insertar modelo);
        Task<OUTPUT> Editar_ProductsAsync(DM_Products_actualizar modelo);
        Task<OUTPUT> Eliminar_ProductsAsync(int? productId, int? productModificatorId);
    }
}
