using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface ISegmentRepository
    {
       
        Task<IEnumerable<Segments>> LISTAR_SEGMENT_ASYNC();

        Task<IEnumerable<Segments>> FILTRAR_SEGMENT_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_SEGMENT_ASYNC(
            string segmentName,
            string segmentDescription,
            int segmentCreatorId,
            bool segmentStatusId
        );
        Task<(int code, string message, int? templateId)> ACTUALIZAR_SEGMENT_ASYNC(
            int segmentId,
            string segmentName,
            string segmentDescription,
            int segmentModificatorId,
            bool segmentStatusId,
            bool forzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_SEGMENT_ASYNC(int segmentId, int segmentModificatorId);
    }
}