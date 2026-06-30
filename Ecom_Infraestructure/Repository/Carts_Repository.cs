using Ecom_Aplication.Interfaces;
using Ecom_Domain; 
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class Carts_Repository : ICartsRepository
    {
        private readonly DB_conection _conection;

        public Carts_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Carts>> LISTAR_CARTS_ASYNC()
        {
            var list = new List<Carts>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Carts_List]", con))
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

        public async Task<IEnumerable<Carts>> FILTRAR_CARTS_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<Carts>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Carts_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusId", statusId ?? (object)DBNull.Value));

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

        public async Task<IEnumerable<Carts>> OBTENER_POR_USUARIO_CARTS_ASYNC(int cartUserId)
        {
            var list = new List<Carts>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Carts_GetActiveByUserId]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cartUserId", cartUserId));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_CARTS_ASYNC(int cartUserId, int cartCreatorId, bool cartStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Carts_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cartUserId", cartUserId));
                    cmd.Parameters.Add(new SqlParameter("@cartCreatorId", cartCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@cartStatusId", cartStatusId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Carrito creado con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al crear el carrito", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_CARTS_ASYNC(int cartId, int cartUserId, int cartModificatorId, bool cartStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Carts_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cartId", cartId));
                    cmd.Parameters.Add(new SqlParameter("@cartUserId", cartUserId));
                    cmd.Parameters.Add(new SqlParameter("@cartModificatorId", cartModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@cartStatusId", cartStatusId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Carrito actualizado con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el carrito", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_CARTS_ASYNC(int cartId, int cartModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Carts_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cartId", cartId));
                    cmd.Parameters.Add(new SqlParameter("@cartModificatorId", cartModificatorId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Carrito eliminado con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el carrito", ex); }
        }

        private Carts MapearEntidadDominio(SqlDataReader dr)
        {
            return new Carts
            {
                cartId = dr["CartId"] as int?,
                cartUserId = dr["CartUserId"] as int?,
                cartCreatorId = dr["CartCreatorId"] as int?,
                cartCreationDate = dr["CartCreationDate"] as DateTime?,
                cartModificatorId = dr["CartModificatorId"] as int?,
                cartModificationDate = dr["CartModificationDate"] as DateTime?,
                cartStatusId = dr["CartStatusId"] as bool?
            };
        }
    }
}
