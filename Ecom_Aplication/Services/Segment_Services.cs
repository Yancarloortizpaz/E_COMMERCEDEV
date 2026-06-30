using Ecom_Aplication.Dtos; // Asegúrate de que apunte a tus DTOs (ej. Segments_DTOS)
using Ecom_Aplication.Interfaces;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Segments_Services
    {
        private readonly ISegmentRepository _repository;

        public Segments_Services(ISegmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_SEGMENT_ASYNC(Segments_DTOS dto)
        {
            return await _repository.NUEVO_SEGMENT_ASYNC(
                dto.SegmentName,
                dto.SegmentDescription,
                dto.SegmentCreatorId ?? 0,
                dto.SegmentStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_SEGMENT_ASYNC(Segments_DTOS dto)
        {
            return await _repository.ACTUALIZAR_SEGMENT_ASYNC(
                dto.SegmentId ?? 0,
                dto.SegmentName,
                dto.SegmentDescription,
                dto.SegmentModificatorId ?? 0,
                dto.SegmentStatusId ?? false,
                false
            );
        }

        public async Task<IEnumerable<Segments_DTOS>> LISTAR_SEGMENT()
        {
            var data = await _repository.LISTAR_SEGMENT_ASYNC();

            return data.Select(s => new Segments_DTOS
            {
                SegmentId = s.SegmentId,
                SegmentName = s.SegmentName,
                SegmentDescription = s.SegmentDescription,
                SegmentCreatorId = s.SegmentCreatorId,
                SegmentCreationDate = s.SegmentCreationDate,
                SegmentModificatorId = s.SegmentModificatorId,
                SegmentModificationDate = s.SegmentModificationDate,
                SegmentStatusId = s.SegmentStatusId
            });
        }

        public async Task<Segments_DTOS?> Obtener_Segment_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_SEGMENT_ASYNC(searchTerm, statusId);

            return data.Select(s => new Segments_DTOS
            {
                SegmentId = s.SegmentId,
                SegmentName = s.SegmentName,
                SegmentDescription = s.SegmentDescription,
                SegmentCreatorId = s.SegmentCreatorId,
                SegmentCreationDate = s.SegmentCreationDate,
                SegmentModificatorId = s.SegmentModificatorId,
                SegmentModificationDate = s.SegmentModificationDate,
                SegmentStatusId = s.SegmentStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_Segment(int segmentId, int segmentModificatorId)
        {
            return await _repository.ELIMINAR_SEGMENT_ASYNC(segmentId, segmentModificatorId);
        }
    }
}