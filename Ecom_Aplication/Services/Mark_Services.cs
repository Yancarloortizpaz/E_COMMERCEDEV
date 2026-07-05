using Ecom_Aplication.Dtos; 
using Ecom_Aplication.Interfaces;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Mark_Services
    {
        private readonly IMarkRepository _repository;

        public Mark_Services(IMarkRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_MARK_ASYNC(Marks_DTOS dto)
        {
            return await _repository.NUEVO_MARK_ASYNC(
                dto.MarkName,
                dto.MarkDescription,
                dto.MarkCreatorId ?? 0,
                dto.MarkStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_MARK_ASYNC(Marks_DTOS dto)
        {
            return await _repository.ACTUALIZAR_MARK_ASYNC(
                dto.MarkId ?? 0,
                dto.MarkName,
                dto.MarkDescription,
                dto.MarkModificatorId ?? 0,
                dto.MarkStatusId ?? false,
                false 
            );
        }

        public async Task<IEnumerable<Marks_DTOS>> LISTAR_MARK()
        {
            var data = await _repository.LISTAR_MARK_ASYNC();

            return data.Select(m => new Marks_DTOS
            {
                MarkId = m.MarkId,
                MarkName = m.MarkName,
                MarkDescription = m.MarkDescription,
                MarkCreatorId = m.MarkCreatorId,
                MarkCreationDate = m.MarkCreationDate,
                MarkModificatorId = m.MarkModificatorId,
                MarkModificationDate = m.MarkModificationDate,
                MarkStatusId = m.MarkStatusId
            });
        }
        public async Task<IEnumerable<Marks_DTOS>> Filtrar_Marks(string? searchTerm, bool? statusId)
        {
          
            var data = await _repository.FILTRAR_MARK_ASYNC(searchTerm ?? "", statusId);

            return data.Select(m => new Marks_DTOS
            {
                MarkId = m.MarkId,
                MarkName = m.MarkName,
                MarkDescription = m.MarkDescription,
                MarkCreatorId = m.MarkCreatorId,
                MarkCreationDate = m.MarkCreationDate,
                MarkModificatorId = m.MarkModificatorId,
                MarkModificationDate = m.MarkModificationDate,
                MarkStatusId = m.MarkStatusId
            });
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_Mark(int markId, int markModificatorId)
        {
            return await _repository.ELIMINAR_MARK_ASYNC(markId, markModificatorId);
        }
    }
}
