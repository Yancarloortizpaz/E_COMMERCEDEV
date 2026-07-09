using DOMAIN.PaymentOrderDetails;
using DOMAIN.VariablesSalida;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APLICATION.Interfaces
{
    public interface IPaymentOrderDetailsRepository
    {
        Task<IEnumerable<DM_PaymentOrderDetails_listar>> Listar_PaymentOrderDetailsAsync();
        Task<IEnumerable<DM_PaymentOrderDetails_filtrar>> Filtrar_PaymentOrderDetailsAsync(int? orderId, string? searchTerm);
        Task<OUTPUT> Insertar_PaymentOrderDetailsAsync(DM_PaymentOrderDetails_insertar modelo);
    }
}
