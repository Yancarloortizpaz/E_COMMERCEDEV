using DOMAIN.AttributeProductVariables;
using DOMAIN.VariablesSalida;

namespace APLICATION.Interfaces
{
    public interface IAttributeProductVariablesRepository
    {
        Task<OUTPUT> Insertar_AttributeProductVariablesAsync(DM_AttributeProductVariablesInsertar modelo);
        Task<OUTPUT> Editar_AttributeProductVariablesAsync(DM_AttributeProductVariablesEditar modelo);
        Task<OUTPUT> Eliminar_AttributeProductVariablesAsync(int? attributeProductVariableId, int? attributeProductVariableModificatorId);
        Task<IEnumerable<DM_AttributeProductVariablesListar>> Listar_AttributeProductVariablesAsync();
        Task<IEnumerable<DM_AttributeProductVariablesFiltrar>> Filtrar_AttributeProductVariablesAsync(DM_AttributeProductVariablesFiltrar filtro);
    }
}
