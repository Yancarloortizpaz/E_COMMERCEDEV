using APLICATION.DTOs.AttributeProductVariables;
using APLICATION.Interfaces;
using DOMAIN.AttributeProductVariables;
using DOMAIN.VariablesSalida;

namespace APLICATION.Services
{
    public class AttributeProductVariablesServices
    {
        private readonly IAttributeProductVariablesRepository _repository;

        public AttributeProductVariablesServices(IAttributeProductVariablesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DM_AttributeProductVariablesListar>> Listar_AttributeProductVariables_async()
        {
            return await _repository.Listar_AttributeProductVariablesAsync();
        }

        public async Task<IEnumerable<DM_AttributeProductVariablesFiltrar>> Filtrar_AttributeProductVariables_async(AttributeProductVariablesFilterDTOs dto)
        {
            var modelo = new DM_AttributeProductVariablesFiltrar
            {
                AttributeProductVariableId = dto.AttributeProductVariableId,
                AttributeProductVariableProductVariableId = dto.AttributeProductVariableProductVariableId,
                AttributeProductVariableAttributeProductId = dto.AttributeProductVariableAttributeProductId,
                AttributeProductVariableValue = dto.AttributeProductVariableValue,
                AttributeProductVariableCreatorId = dto.AttributeProductVariableCreatorId,
                AttributeProductVariableCreationDate = dto.AttributeProductVariableCreationDate,
                AttributeProductVariableModificatorId = dto.AttributeProductVariableModificatorId,
                AttributeProductVariableModificationDate = dto.AttributeProductVariableModificationDate,
                AttributeProductVariableStatusId = dto.AttributeProductVariableStatusId
            };
            return await _repository.Filtrar_AttributeProductVariablesAsync(modelo);
        }

        public async Task<OUTPUT> Insertar_AttributeProductVariables_async(AttributeProductVariablesInsertarDTOs dto)
        {
            var modelo = new DM_AttributeProductVariablesInsertar
            {
                AttributeProductVariableProductVariableId = dto.AttributeProductVariableProductVariableId,
                AttributeProductVariableAttributeProductId = dto.AttributeProductVariableAttributeProductId,
                AttributeProductVariableValue = dto.AttributeProductVariableValue,
                AttributeProductVariableCreatorId = dto.AttributeProductVariableCreatorId,
                AttributeProductVariableStatusId = dto.AttributeProductVariableStatusId
            };
            return await _repository.Insertar_AttributeProductVariablesAsync(modelo);
        }

        public async Task<OUTPUT> Editar_AttributeProductVariables_async(AttributeProductVariablesEditarDTOs dto)
        {
            var modelo = new DM_AttributeProductVariablesEditar
            {
                AttributeProductVariableId = dto.AttributeProductVariableId,
                AttributeProductVariableProductVariableId = dto.AttributeProductVariableProductVariableId,
                AttributeProductVariableAttributeProductId = dto.AttributeProductVariableAttributeProductId,
                AttributeProductVariableValue = dto.AttributeProductVariableValue,
                AttributeProductVariableModificatorId = dto.AttributeProductVariableModificatorId,
                AttributeProductVariableStatusId = dto.AttributeProductVariableStatusId
            };
            return await _repository.Editar_AttributeProductVariablesAsync(modelo);
        }

        public async Task<OUTPUT> Eliminar_AttributeProductVariables_async(int? attributeProductVariableId, int? attributeProductVariableModificatorId)
        {
            return await _repository.Eliminar_AttributeProductVariablesAsync(attributeProductVariableId, attributeProductVariableModificatorId);
        }
    }
}
