using Ecom_Aplication.Interfaces;
using Ecom_Domain.Entities;
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class CartDetail_Repository : ICartDetails
    {
        private readonly DB_conection _conection;

        public CartDetail_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<CartDetail>> LISTAR_CARTDETAILS_ASYNC()
        {
            var list = new List<CartDetail>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_CartDetails_List]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearEntidadDominio(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_CARTDETAILS_ASYNC(
            int cartDetailCartId,
            int cartDetailProductVariableId,
            decimal cartDetailPrice,
            int cartDetailQuantity,
            decimal cartDetailDiscount,
            decimal cartDetailSubTotal,
            decimal cartDetailTAX,
            decimal cartDetailTotal,
            int cartDetailCurrencyId,
            int cartDetailCreatorId,
            bool cartDetailStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_CartDetails_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@cartDetailCartId", cartDetailCartId));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailProductVariableId", cartDetailProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailPrice", cartDetailPrice));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailQuantity", cartDetailQuantity));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailDiscount", cartDetailDiscount));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailSubTotal", cartDetailSubTotal));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailTAX", cartDetailTAX));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailTotal", cartDetailTotal));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailCurrencyId", cartDetailCurrencyId));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailCreatorId", cartDetailCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailStatusId", cartDetailStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Detalle de carrito insertado correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el detalle de carrito", ex); }
        }

        public async Task<(int code, string message)> ACTUALIZAR_CANTIDAD_CARTDETAILS_ASYNC(
            int cartDetailId,
            int newQuantity,
            int modificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_CartDetails_UpdateQuantity]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@cartDetailId", cartDetailId));
                    cmd.Parameters.Add(new SqlParameter("@newQuantity", newQuantity));
                    cmd.Parameters.Add(new SqlParameter("@modificatorId", modificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Cantidad y cálculos del carrito actualizados dinámicamente con éxito.";

                    return (code, message);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la cantidad del detalle de carrito", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_CARTDETAILS_ASYNC(
            int cartDetailId,
            int cartDetailModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_CartDetails_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cartDetailId", cartDetailId));
                    cmd.Parameters.Add(new SqlParameter("@cartDetailModificatorId", cartDetailModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Detalle de carrito inactivado (eliminado lógicamente) correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el detalle de carrito", ex); }
        }

        private CartDetail MapearEntidadDominio(SqlDataReader dr)
        {
            return new CartDetail
            {
                Id = dr["cartDetailId"] as int?,
                CartId = dr["cartDetailCartId"] as int?,
                ProductVariableId = dr["cartDetailProductVariableId"] as int?,
                Price = dr["cartDetailPrice"] as decimal?,
                Quantity = dr["cartDetailQuantity"] as int?,
                Discount = dr["cartDetailDiscount"] as decimal?,
                SubTotal = dr["cartDetailSubTotal"] as decimal?,
                Tax = dr["cartDetailTAX"] as decimal?,
                Total = dr["cartDetailTotal"] as decimal?,
                CurrencyId = dr["cartDetailCurrencyId"] as int?,
                CreatorId = dr["cartDetailCreatorId"] as int?,
                CreationDate = dr["cartDetailCreationDate"] as DateTime?,
                ModificatorId = dr["cartDetailModificatorId"] as int?,
                ModificationDate = dr["cartDetailModificationDate"] as DateTime?,
                StatusId = dr["cartDetailStatusId"] as bool?
            };
        }
    }
}