using Ecom_Aplication.Dtos;
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
    public class Category_Repository : ICategoryRepository
    {
        private readonly DB_conection _conection;

        public Category_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_CATEGORY_ASYNC(
            string categoryName,
            string categoryDescription,
            int categoryCreatorId,
            bool categoryStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Categories_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@categoryName", categoryName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@categoryDescription", categoryDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@categoryCreatorId", categoryCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@categoryStatusId", categoryStatusId));

                    await cmd.ExecuteNonQueryAsync();

                    
                    return (200, "Categoría creada con éxito", null);
                }
            }
            catch (SqlException ex) { throw ex; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar acuerdo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_CATEGORY_ASYNC(
            int categoryId,
            string categoryName,
            string categoryDescription,
            int categoryModificatorId,
            bool categoryStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Categories_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                    cmd.Parameters.Add(new SqlParameter("@categoryName", categoryName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@categoryDescription", categoryDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@categoryModificatorId", categoryModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@categoryStatusId", categoryStatusId));
                    

                    await cmd.ExecuteNonQueryAsync();

                    return (200, "Categoría actualizada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al editar el acuerdo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_CATEGORY_ASYNC(int categoryId, int categoryModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_Categories_Delete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                    cmd.Parameters.Add(new SqlParameter("@categoryModificatorId", categoryModificatorId));

                    await cmd.ExecuteNonQueryAsync();

                    return (200, "Categoría eliminada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el registro", ex); }
        }

        public async Task<IEnumerable<CATEGORIES>> LISTAR_CATEGORY_ASYNC()
        {
            var list = new List<CATEGORIES>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Categories_List]", con))
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

        public async Task<IEnumerable<CATEGORIES>> FILTRAR_CATEGORY_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<CATEGORIES>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Categories_Filter]", con))
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

        
        private CATEGORIES MapearEntidadDominio(SqlDataReader dr)
        {
            return new CATEGORIES
            {
                CategoryId = dr["CategoryId"] as int?,
                CategoryName = dr["CategoryName"]?.ToString() ?? string.Empty,
                CategoryDescription = dr["CategoryDescription"]?.ToString() ?? string.Empty,
                CategoryCreatorId = dr["CategoryCreatorId"] as int?,
                CategoryCreationDate = dr["CategoryCreationDate"] as DateTime?,
                CategoryModificatorId = dr["CategoryModificatorId"] as int?,
                CategoryModificationDate = dr["CategoryModificationDate"] as DateTime?,
                CategoryStatusId = dr["CategoryStatusId"] as bool?
            };
        }
    }
}