using Ecom_Aplication.Dtos; // Asegura que apunte a donde tienes tu Carts_DTOS
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Carts_Services
    {
        private readonly ICartsRepository _repository;

        public Carts_Services(ICartsRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_CARTS_ASYNC(Carts_DTOS dto)
        {
            return await _repository.NUEVO_CARTS_ASYNC(
                dto.cartUserId ?? 0,
                dto.cartCreatorId ?? 0,
                dto.cartStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_CARTS_ASYNC(Carts_DTOS dto)
        {
            return await _repository.ACTUALIZAR_CARTS_ASYNC(
                dto.cartId ?? 0,
                dto.cartUserId ?? 0,
                dto.cartModificatorId ?? 0,
                dto.cartStatusId ?? false
            );
        }

        public async Task<IEnumerable<Carts_DTOS>> LISTAR_CARTS()
        {
            var data = await _repository.LISTAR_CARTS_ASYNC();

            return data.Select(c => new Carts_DTOS
            {
                cartId = c.cartId,
                cartUserId = c.cartUserId,
                cartCreatorId = c.cartCreatorId,
                cartCreationDate = c.cartCreationDate,
                cartModificatorId = c.cartModificatorId,
                cartModificationDate = c.cartModificationDate,
                cartStatusId = c.cartStatusId
            });
        }

        public async Task<IEnumerable<Carts_DTOS>> OBTENER_POR_USUARIO(int cartUserId)
        {
            var data = await _repository.OBTENER_POR_USUARIO_CARTS_ASYNC(cartUserId);

            return data.Select(c => new Carts_DTOS
            {
                cartId = c.cartId,
                cartUserId = c.cartUserId,
                cartCreatorId = c.cartCreatorId,
                cartCreationDate = c.cartCreationDate,
                cartModificatorId = c.cartModificatorId,
                cartModificationDate = c.cartModificationDate,
                cartStatusId = c.cartStatusId
            });
        }

        public async Task<Carts_DTOS?> Obtener_Carts_Por_Filtro(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_CARTS_ASYNC(searchTerm, statusId);

            return data.Select(c => new Carts_DTOS
            {
                cartId = c.cartId,
                cartUserId = c.cartUserId,
                cartCreatorId = c.cartCreatorId,
                cartCreationDate = c.cartCreationDate,
                cartModificatorId = c.cartModificatorId,
                cartModificationDate = c.cartModificationDate,
                cartStatusId = c.cartStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_Carts(int cartId, int cartModificatorId)
        {
            return await _repository.ELIMINAR_CARTS_ASYNC(cartId, cartModificatorId);
        }
    }
}
