using Ecom_Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface IMarkRepository
    {
        Task<IEnumerable<Marks>> LISTAR_MARK_ASYNC();

        Task<IEnumerable<Marks>> FILTRAR_MARK_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_MARK_ASYNC(
            string markName,
            string markDescription,
            int markCreatorId,
            bool markStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_MARK_ASYNC(
            int markId,
            string markName,
            string markDescription,
            int markModificatorId,
            bool markStatusId,
            bool forzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_MARK_ASYNC(int markId, int markModificatorId);
    }
}