using APLICATION.DTOs.Products;
using APLICATION.Interfaces;
using DOMAIN.Products;
using DOMAIN.VariablesSalida;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APLICATION.Services
{
    public class ProductsServices
    {
        private readonly IProductsRepository _repository;

        public ProductsServices(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductsListarDTOs>> Listar_Products_async()
        {
            var data = await _repository.Listar_ProductsAsync();
            return data.Select(x => new ProductsListarDTOs
            {
                productId = x.productId,
                productName = x.productName,
                productDescription = x.productDescription,
                productIdentificatorId = x.productIdentificatorId,
                categoryId = x.categoryId,
                categoryName = x.categoryName,
                subCategoryId = x.subCategoryId,
                subCategoryName = x.subCategoryName,
                segmentId = x.segmentId,
                segmentName = x.segmentName,
                markByProviderId = x.markByProviderId,
                markId = x.markId,
                markName = x.markName,
                providerId = x.providerId,
                providerName = x.providerName,
                statusId = x.statusId
            });
        }

        public async Task<IEnumerable<ProductsFiltrarDTOs>> Filtrar_Products_async(string? searchTerm)
        {
            var data = await _repository.Filtrar_ProductsAsync(searchTerm);
            return data.Select(x => new ProductsFiltrarDTOs
            {
                productId = x.productId,
                productName = x.productName,
                productDescription = x.productDescription,
                productIdentificatorId = x.productIdentificatorId,
                categoryId = x.categoryId,
                categoryName = x.categoryName,
                subCategoryId = x.subCategoryId,
                subCategoryName = x.subCategoryName,
                segmentId = x.segmentId,
                segmentName = x.segmentName,
                markByProviderId = x.markByProviderId,
                markId = x.markId,
                markName = x.markName,
                providerId = x.providerId,
                providerName = x.providerName,
                statusId = x.statusId
            });
        }

        public async Task<OUTPUT> Insertar_Products_async(ProductsinsertarDTOs dto)
        {
            var modelo = new DM_Products_insertar
            {
                productName = dto.productName,
                productDescription = dto.productDescription,
                productProductIdentificatorId = dto.productProductIdentificatorId,
                productMarkByProviderId = dto.productMarkByProviderId,
                productCreatorId = dto.productCreatorId,
                productStatusId = dto.productStatusId
            };
            return await _repository.Insertar_ProductsAsync(modelo);
        }

        public async Task<OUTPUT> Editar_Products_async(ProductsEditarDTOs dto)
        {
            var modelo = new DM_Products_actualizar
            {
                productId = dto.productId,
                productName = dto.productName,
                productDescription = dto.productDescription,
                productProductIdentificatorId = dto.productProductIdentificatorId,
                productMarkByProviderId = dto.productMarkByProviderId,
                productModificatorId = dto.productModificatorId,
                productStatusId = dto.productStatusId,
                ForzarRecuperacion = dto.ForzarRecuperacion
            };
            return await _repository.Editar_ProductsAsync(modelo);
        }

        public async Task<OUTPUT> Eliminar_Products_async(int? productId, int? productModificatorId)
        {
            return await _repository.Eliminar_ProductsAsync(productId, productModificatorId);
        }
    }
}
