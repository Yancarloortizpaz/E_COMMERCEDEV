 DOCUMENTO MAESTRO DE DESARROLLO (ANTIGRAVITY BLUEPRINT)

Este documento es la guía de referencia absoluta para la generación de código tarea por tarea. 
La IA debe ceñirse estrictamente a las reglas, estructuras y diagramas definidos a continuación.

ACTUALMENTE EL PROGRAMA ESTA ASI ; 
C:\hector\E_COMMERCEDEV
│
└── API
    └── API_ECCOMERCEDEV
        ├── API_ECCOMERCEDEV.slnx
        │
        ├── APLICATION
        │   └── APLICATION.csproj
        │
        ├── DOMAIN
        │   └── DOMAIN.csproj
        │
        ├── INFRASTRUCTURE
        │   └── INFRASTRUCTURE.csproj
        │
        └── PRESENTACION
            ├── PRESENTACION.csproj
            ├── PRESENTACION.http
            │
            ├── Properties
            │   └── launchSettings.json
            │
            ├── Controllers
            │   └── WeatherForecastController.cs
            │
            ├── appsettings.json
            └── appsettings.Development.json
   ES LO QUE TENEMOS HASTA EL MOMENTO Y LO QUE IREMOS CONTRUYENDO SE VERA ASI :
 MAPA DEL PROYECTO Y ÁRBOL DE DIRECTORIOS DE COMO SE IRA ESTRUCTURANDO 

text
C:\hector\E_COMMERCEDEV
│
└── API
    └── API_ECCOMERCEDEV
        ├── API_ECCOMERCEDEV.slnx
        │
        ├── APLICATION
        │   ├── APLICA---TION.csproj
        │   │
        │   ├── DTOs
        │   │   ├── Catalogos
        │   │   │   ├── EstadoinsertarDTOs.cs
        │   │   │   └── EstadoEditarDTOs.cs
        │   │   │
        │   │   └── Presupuesto
        │   │       ├── AcuerdosinsertarDTOs.cs
        │   │       └── AcuerdosPagoActualizarDTOs.cs
        │   │
        │   ├── Interfaces
        │   │   ├── IAcuerdos_Pago_Repository.cs
        │   │   └── ICatalogosRepository.cs
        │   │
        │   └── Services
        │       ├── Acuerdos_Pago_Services.cs
        │       └── CatalogosServices.cs
        │
        ├── DOMAIN
        │   ├── DOMAIN.csproj
        │   │
        │   ├── VariablesSalida
        │   │   └── DBResult.cs
        │   │
        │   ├── Catalogos
        │   │   ├── DM_Estado_insertar.cs
        │   │   └── DM_Estado_listar.cs
        │   │
        │   └── Presupuesto
        │       ├── DM_Acuerdos_Pago_insertar.cs
        │       ├── DM_Acuerdos_Pago_actualizar.cs
        │       └── DM_Acuerdos_Pago_listar.cs
        │
        ├── INFRASTRUCTURE
        │   ├── INFRASTRUCTURE.csproj
        │   │
        │   ├── DB
        │   │   └── DBconexionfactory.cs
        │   │
        │   └── Repository
        │       ├── Acuerdos_Pagos_Repository.cs
        │       └── CatalogosRepository.cs
        │
        └── PRESENTACION
            ├── PRESENTACION.csproj
            ├── PRESENTACION.http
            ├── Program.cs
            │
            ├── Properties
            │   └── launchSettings.json
            │
            ├── Controllers
            │   ├── Acuerdo_PagosController.cs
            │   └── CatalogosController.cs
            │
            ├── appsettings.json
            └── appsettings.Development.json

. DIAGRAMA DE FLUJO DE DATOS Y TRADUCCIÓN DE OUTPUTS
El siguiente flujo ilustra cómo viajan los datos a través de las capas y cómo se maneja
de forma segura el mapeo de los parámetros OUTPUT de SQL sin contaminar el dominio:

[ Cliente / Web ] 
       │
       ▼ (Envía RequestDTO)
[ Presentacion.Controllers ]
       │
       ▼ (Pasa RequestDTO)
[ Aplicacion.Services ] ──(Mapea RequestDTO -> DM_DomainModel)──> [ Domain (DM_Clases) ]
       │                                                                  │
       ▼ (Envía modelo DM_ puro)                                          │ Usado como contrato
[ Infraestructura.Repository ] <──────────────────────────────────────────┘
       │
       ├─► [ Ejecuta Stored Procedure en SQL Server ]
       │        ├── Parámetros IN: (Campos de la clase DM_)
       │        └── Parámetros OUTPUT: (@o_code, @o_message, @o_templateId)
       │
       ▼ (Mapeo y traducción manual de variables)
   result.Code       <─── @o_code
   result.Message    <─── @o_message
   result.TemplateId <─── @o_templateId
       │
       ▼ (Retorna Instancia Limpia C#)
[ Domain.VariablesSalida.DBResult ] ───► Viaja de regreso al Service ───► Controller (JSON)


A CONTINUACION IRE DETALLANDO COMO SE VAN  A IR CONTRUYENDO LO QUE SERIA LAS DIFERENTES CLASES
DEBO DE ACLARAR QUE EATS SON PLANTILLAS LAS CUALES DEBEN DE ADAPTARSE A CADA SP , 
ESTO ES PARA QUE SE VEA LA ESTRUCTURA DE COMO SE VA A IR CONSTRUYENDO EL PROYECTO.

5. PLANTILLA DE IMPLEMENTACIÓN (MÓDULO PRESUPUESTO)
A. Modelos de Dominio (Domain/Presupuesto/)
DM_Acuerdos_Pago_insertar.cs

using System;

namespace Domain.Presupuesto
{
    public class DM_Acuerdos_Pago_insertar
    {
        public int? Id_Multa { get; set; }
        public decimal? Monto_Total_Acordado { get; set; }
        public int? Cantidad_Cuotas { get; set; }
        public decimal? Monto_Por_Cuota { get; set; }
        public int? Frecuencia_Pago { get; set; }
        public int? Id_Creador { get; set; }
    }
}

ASI MISMO LOS OTROS DOMAIN PARA CADA SP, SE DEBEN DE CREAR CLASES DOMAIN, ESTO ES PARA QUE SE VEA LA ESTRUCTURA DE COMO SE VA A IR CONSTRUYENDO EL PROYECTO.
CADA SP TENDRA SU CLASE DE MANERA INDEPENDIENTE COMO LO VIMOS AL INICIO 
using System;

namespace Domain.Presupuesto
{
    public class DM_Acuerdos_Pago_actualizar
    {
        public int? Id_Acuerdo { get; set; }
        public decimal? Monto_Total_Acordado { get; set; }
        public int? Cantidad_Cuotas { get; set; }
        public decimal? Monto_Por_Cuota { get; set; }
        public int? Frecuencia_Pago { get; set; }
        public int? Id_Modificador { get; set; }
    }
}
LOS Dtos DEBEN DE SER CON ESTA MISMA ESTRUCTRUCTRA TAMBEIEN , PARA CADA SP SE DEBE DE CREAR SU DTO DE MANERA INDEPENDIENTE, 


LOS SERVICES SE VERAN ASI 

using Aplicacion.DTOs.Presupuesto;
using Aplicacion.Interfaces;
using Domain.Presupuesto;
using Domain.VariablesSalida;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class Acuerdos_Pago_Services
    {
        private readonly IAcuerdos_Pago_Repository _repository;

        public Acuerdos_Pago_Services(IAcuerdos_Pago_Repository repository)
        {
            _repository = repository;
        }

        public async Task<DBResult> Insertar_acuerdos_pago_async(AcuerdosinsertarDTOs dto)
        {
            var modelo = new DM_Acuerdos_Pago_insertar
            {
                Id_Multa = dto.Id_Multa,
                Monto_Total_Acordado = dto.Monto_Total_Acordado,
                Cantidad_Cuotas = dto.Cantidad_Cuotas,
                Monto_Por_Cuota = dto.Monto_Por_Cuota,
                Frecuencia_Pago = dto.Frecuencia_Pago,
                Id_Creador = dto.Id_Creador
            };

            return await _repository.Insertar_Acuerdos_PagoAsync(modelo);
        }

        public async Task<DBResult> Editar_acuerdos_pago_async(AcuerdosPagoActualizarDTOs dto)
        {
            var modelo = new DM_Acuerdos_Pago_actualizar
            {
                Id_Acuerdo = dto.Id_Acuerdo,
                Monto_Total_Acordado = dto.Monto_Total_Acordado,
                Cantidad_Cuotas = dto.Cantidad_Cuotas,
                Monto_Por_Cuota = dto.Monto_Por_Cuota,
                Frecuencia_Pago = dto.Frecuencia_Pago,
                Id_Modificador = dto.Id_Modificador
            };

            return await _repository.Editar_Acuerdos_PagoAsync(modelo);
        }

        public async Task<IEnumerable<DM_Acuerdos_Pago_listar>> Listar_Acuerdos_Pago()
        {
            // Retorna directamente la colección del dominio para que el controlador la exponga o use un DTO de lectura en tareas específicas
            return await _repository.Listar_Acuerdos_PagosAsync();
        }

        public async Task<DM_Acuerdos_Pago_listar?> Obtener_Acuerdo_Pago_Por_Id(int? id)
        {
            var data = await _repository.Listar_Acuerdos_Pagos_IdAsync(id);
            return data.FirstOrDefault();
        }

        public async Task<DBResult> Eliminar_Acuerdo_Pago(int? id, int? idModificador)
        {
            return await _repository.Eliminar_Acuerdos_PagoAsync(id, idModificador);
        }
    }
}



las interfaces asi ;     public interface IAcuerdos_Pago_Repository
    {
        Task<IEnumerable<Acuerdos_Pago_DTOs>> Listar_Acuerdos_PagosAsync();
        Task<IEnumerable<Acuerdos_Pago_DTOs>> Listar_Acuerdos_Pagos_IdAsync(int id);
        Task Insertar_Acuerdos_PagoAsync(Acuerdos_Pago_DTOs acuerdos_Pago);
        Task Editar_Acuerdos_PagoAsync(Acuerdos_Pago_DTOs acuerdos_Pago);
        Task Eliminar_Acuerdos_PagoAsync(int id, int Id_modificador);
    }
}  clasro que hay que coisderar que esats van conectadas con las otras como por ejemplo using application.DTOs; 
 los reposity se veran asi ; 
 using Aplicacion.Interfaces;
using Domain.Presupuesto;
using Domain.VariablesSalida;
using infrastructure.DB;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class Acuerdos_Pagos_Repository : IAcuerdos_Pago_Repository
    {
        private readonly DBconexionfactory _conection;

        public Acuerdos_Pagos_Repository(DBconexionfactory conection)
        {
            _conection = conection;
        }

        #region Métodos de Escritura (Comandos Transaccionales)

        public async Task<DBResult> Insertar_Acuerdos_PagoAsync(DM_Acuerdos_Pago_insertar modelo)
        {
            var result = new DBResult();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("Sp_Acuerdos_Pago_Insertar", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de Entrada (IN)
                    cmd.Parameters.Add(new SqlParameter("@Id_Multa", modelo.Id_Multa ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Monto_Total_Acordado", modelo.Monto_Total_Acordado ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Cantidad_Cuotas", modelo.Cantidad_Cuotas ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Monto_Por_Cuota", modelo.Monto_Por_Cuota ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Frecuencia_Pago", modelo.Frecuencia_Pago ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Id_Creador", modelo.Id_Creador ?? (object)DBNull.Value));

                    // Definición de Parámetros de Salida Universales (OUTPUT)
                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter pTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pCode);
                    cmd.Parameters.Add(pMessage);
                    cmd.Parameters.Add(pTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    // Traducción manual y segura de las variables de salida al Objeto del Dominio
                    result.Code = pCode.Value != DBNull.Value ? (int?)pCode.Value : null;
                    result.Message = pMessage.Value != DBNull.Value ? pMessage.Value.ToString() : null;
                    result.TemplateId = pTemplate.Value != DBNull.Value ? (int?)pTemplate.Value : null;
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en el motor SQL al insertar el acuerdo de pago.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico de infraestructura al insertar el acuerdo.", ex);
            }
        }

        public async Task<DBResult> Editar_Acuerdos_PagoAsync(DM_Acuerdos_Pago_actualizar modelo)
        {
            var result = new DBResult();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("Sp_Editar_Acuerdos_Pago", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de Entrada (IN)
                    cmd.Parameters.Add(new SqlParameter("@Id_Acuerdo", modelo.Id_Acuerdo ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Monto_Total_Acordado", modelo.Monto_Total_Acordado ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Cantidad_Cuotas", modelo.Cantidad_Cuotas ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Monto_Por_Cuota", modelo.Monto_Por_Cuota ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Frecuencia_Pago", modelo.Frecuencia_Pago ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Id_Modificador", modelo.Id_Modificador ?? (object)DBNull.Value));

                    // Definición de Parámetros de Salida Universales (OUTPUT)
                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter pTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pCode);
                    cmd.Parameters.Add(pMessage);
                    cmd.Parameters.Add(pTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    // Traducción manual y segura de las variables de salida
                    result.Code = pCode.Value != DBNull.Value ? (int?)pCode.Value : null;
                    result.Message = pMessage.Value != DBNull.Value ? pMessage.Value.ToString() : null;
                    result.TemplateId = pTemplate.Value != DBNull.Value ? (int?)pTemplate.Value : null;
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en el motor SQL al actualizar el acuerdo de pago.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico de infraestructura al editar el acuerdo de pago.", ex);
            }
        }

        public async Task<DBResult> Eliminar_Acuerdos_PagoAsync(int? id, int? idModificador)
        {
            var result = new DBResult();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("Sp_Acuerdos_Pago_Eliminar", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de Entrada (IN)
                    cmd.Parameters.Add(new SqlParameter("@Id_Acuerdo", id ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Id_Modificador", idModificador ?? (object)DBNull.Value));

                    // Definición de Parámetros de Salida Universales (OUTPUT)
                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter pTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pCode);
                    cmd.Parameters.Add(pMessage);
                    cmd.Parameters.Add(pTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    // Traducción manual y segura de las variables de salida
                    result.Code = pCode.Value != DBNull.Value ? (int?)pCode.Value : null;
                    result.Message = pMessage.Value != DBNull.Value ? pMessage.Value.ToString() : null;
                    result.TemplateId = pTemplate.Value != DBNull.Value ? (int?)pTemplate.Value : null;
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en el motor SQL al eliminar de forma lógica el acuerdo.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico de infraestructura al intentar eliminar el acuerdo.", ex);
            }
        }

        #endregion

        #region Métodos de Lectura (Consultas / Queries)

        public async Task<IEnumerable<DM_Acuerdos_Pago_listar>> Listar_Acuerdos_PagosAsync()
        {
            var list = new List<DM_Acuerdos_Pago_listar>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("Sp_Acuerdos_Pago_Listar", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearDataReaderADominio(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar el listado completo de acuerdos en la base de datos.", ex);
            }
        }

        public async Task<IEnumerable<DM_Acuerdos_Pago_listar>> Listar_Acuerdos_Pagos_IdAsync(int? id)
        {
            var list = new List<DM_Acuerdos_Pago_listar>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("Sp_Filtrar_Acuerdos_Pago", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id_Usuario", id ?? (object)DBNull.Value));

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(new DM_Acuerdos_Pago_listar
                            {
                                Id_Acuerdo = dr.IsDBNull(dr.GetOrdinal("Id_Acuerdo")) ? null : (int?)dr.GetInt32(dr.GetOrdinal("Id_Acuerdo")),
                                Id_Multa = dr.IsDBNull(dr.GetOrdinal("Id_Multa")) ? null : (int?)dr.GetInt32(dr.GetOrdinal("Id_Multa")),
                                Monto_Total_Acordado = dr.IsDBNull(dr.GetOrdinal("Monto_Total_Acordado")) ? null : (decimal?)dr.GetDecimal(dr.GetOrdinal("Monto_Total_Acordado")),
                                Cantidad_Cuotas = dr.IsDBNull(dr.GetOrdinal("Cantidad_Cuotas")) ? null : (int?)dr.GetInt32(dr.GetOrdinal("Cantidad_Cuotas")),
                                Monto_Por_Cuota = dr.IsDBNull(dr.GetOrdinal("Monto_Por_Cuota")) ? null : (decimal?)dr.GetDecimal(dr.GetOrdinal("Monto_Por_Cuota")),
                                Frecuencia_Descripcion = dr.IsDBNull(dr.GetOrdinal("Frecuencia_Pago_Nombre")) ? null : dr.GetString(dr.GetOrdinal("Frecuencia_Pago_Nombre")),
                                Fecha_Creacion = dr.IsDBNull(dr.GetOrdinal("Fecha_Creacion")) ? null : (DateTime?)dr.GetDateTime(dr.GetOrdinal("Fecha_Creacion")),
                                Estado = dr.IsDBNull(dr.GetOrdinal("Estado_Descripcion")) ? null : dr.GetString(dr.GetOrdinal("Estado_Descripcion")),
                                Usuario_Login = dr.IsDBNull(dr.GetOrdinal("Login_Usuario")) ? null : dr.GetString(dr.GetOrdinal("Login_Usuario"))
                            });
                        }
                    }
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al filtrar el acuerdo por el identificador de usuario: {id}.", ex);
            }
        }

        #endregion

        #region Mapeadores Internos Primitivos (Helpers)

        private DM_Acuerdos_Pago_listar MapearDataReaderADominio(SqlDataReader dr)
        {
            return new DM_Acuerdos_Pago_listar
            {
                Id_Acuerdo = dr["Id_Acuerdo"] as int?,
                Id_Multa = dr["Id_Multa"] as int?,
                Monto_Total_Acordado = dr["Monto_Total_Acordado"] as decimal?,
                Cantidad_Cuotas = dr["Cantidad_Cuotas"] as int?,
                Monto_Por_Cuota = dr["Monto_Por_Cuota"] as decimal?,
                Frecuencia_Pago = dr["Frecuencia_Pago"] as int?,
                Frecuencia_Descripcion = dr["Frecuencia_Pago_Nombre"]?.ToString(),
                Fecha_Creacion = dr["Fecha_Creacion"] as DateTime?,
                Fecha_Modificacion = dr["Fecha_Modificacion"] as DateTime?,
                Id_Creador = dr["Id_Creador"] as int?,
                Id_Modificador = dr["Id_Modificador"] as int?,
                Id_Estado = dr["Id_Estado"] as int?,
                Estado = dr["Estado"]?.ToString()
            };
        }

        #endregion
    }
}  los nombre de las regiones pueden ser de una palabra nada mas como listar_catalogo  y asi sucesivamnete una sola palabra osea 2 pero unida con _


los controller se veran asi 

}using System;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.DTOs.Presupuesto;
using Aplicacion.Services;
using Domain.VariablesSalida;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Acuerdo_PagosController : ControllerBase
    {
        private readonly Acuerdos_Pago_Services _service;

        public Acuerdo_PagosController(Acuerdos_Pago_Services service)
        {
            _service = service;
        }

        #region Métodos de Lectura (GET)

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar_Acuerdo_Pagos()
        {
            try
            {
                var lista = await _service.Listar_Acuerdos_Pago();
                if (lista == null || !lista.Any())
                {
                    return NotFound(new { codigo = 404, msj = "No se encontraron acuerdos de pago." });
                }
                return Ok(new { codigo = 200, msj = "Consulta exitosa", data = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        [HttpGet("listar_por_id/{id}")]
        public async Task<IActionResult> Listar_Acuerdo_Pagos_Por_Id(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return BadRequest(new { codigo = 400, msj = "El identificador de usuario es requerido." });
                }

                var elemento = await _service.Obtener_Acuerdo_Pago_Por_Id(id);
                if (elemento == null)
                {
                    return NotFound(new { codigo = 404, msj = "Acuerdo no encontrado." });
                }
                return Ok(new { codigo = 200, msj = "Consulta exitosa", data = elemento });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        #endregion

        #region Métodos de Escritura (POST, PUT, DELETE) - Controlados por Base de Datos

        [HttpPost("insertar")]
        public async Task<IActionResult> Ingresar_Datos_Acuerdos_Pagos([FromBody] AcuerdosinsertarDTOs acuerdos)
        {
            try
            {
                if (!ModelState.IsValid || acuerdos == null)
                {
                    return BadRequest(new { codigo = 400, msj = "Datos enviados no válidos." });
                }

                // Ejecutamos el flujo y capturamos el DBResult del SP
                DBResult resultado = await _service.Insertar_acuerdos_pago_async(acuerdos);

                // Si IsSuccess es falso (Code < 0), regresamos lo que la BD dictaminó
                if (!resultado.IsSuccess)
                {
                    return BadRequest(new { codigo = resultado.Code, msj = resultado.Message });
                }

                // Si todo sale bien, también mostramos el mensaje exacto generado por el SP
                return Ok(new { codigo = resultado.Code, msj = resultado.Message, templateId = resultado.TemplateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Editar_Acuerdos_Pagos([FromBody] AcuerdosPagoActualizarDTOs acuerdos)
        {
            try
            {
                if (acuerdos == null || !acuerdos.Id_Acuerdo.HasValue)
                {
                    return BadRequest(new { codigo = 400, msj = "El identificador del acuerdo es obligatorio." });
                }

                DBResult resultado = await _service.Editar_acuerdos_pago_async(acuerdos);

                if (!resultado.IsSuccess)
                {
                    return BadRequest(new { codigo = resultado.Code, msj = resultado.Message });
                }

                return Ok(new { codigo = resultado.Code, msj = resultado.Message, templateId = resultado.TemplateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        [HttpDelete("{id}/{idModificador}")]
        public async Task<IActionResult> Eliminar_Acuerdos_Pagos(int? id, int? idModificador)
        {
            try
            {
                if (!id.HasValue || !idModificador.HasValue)
                {
                    return BadRequest(new { codigo = 400, msj = "El ID del acuerdo y el ID del modificador son requeridos." });
                }

                DBResult resultado = await _service.Eliminar_Acuerdo_Pago(id, idModificador);

                if (!resultado.IsSuccess)
                {
                    return BadRequest(new { codigo = resultado.Code, msj = resultado.Message });
                }

                return Ok(new { codigo = resultado.Code, msj = resultado.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        #endregion
    }
}  igual las regiones como en el caso anterior 
y por ultimo conectar en el progrma de  presentacion ; 
builder.Services.AddScoped<IAcuerdos_Pago_Repository, Acuerdos_Pagos_Repository>();
builder.Services.AddScoped<Acuerdos_Pago_Services>();  para que ya quede concetado 

iremos contruyendo archivo por archivo adaptado segun cada sp que ire dejando en  
se usara de manara adecuanda la conexion como ya se vio en los ejemplos pero usando las rutas correcta para este proyecto
}

la ruta del cual se encuentra los sp de los que trabajaremos es C:\hector\E_COMMERCEDEV\API\API_ECCOMERCEDEV\nombres_y_sp_solo_los_paramtros.md
es importante tener en cuneta que los parametoros de salida   @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT  se usaran de forama global los cuales ya estan creados 
    C:\hector\E_COMMERCEDEV\API\API_ECCOMERCEDEV\DOMAIN\VariablesSalida\OUTPUT.cs

## 6. REGLAS DE DISEÑO Y DESARROLLO ESTABLECIDAS EN LA SESIÓN

A partir del desarrollo de los módulos de la aplicación (Providers, Users, PaymentMethodTypes, Segments), se establecen de forma definitiva las siguientes reglas de arquitectura y flujo que deben respetarse en todos los próximos desarrollos:

1. **Estructura de Directorios por Tabla/Entidad**:
   - Todo código (modelos de dominio y DTOs) debe organizarse en subcarpetas con el nombre de la tabla correspondiente (ej. `DOMAIN/Providers/`, `DOMAIN/Users/`, `APLICATION/DTOs/Providers/`). Se prohíbe agrupar catálogos distintos en una carpeta genérica como `Catalogos`.

2. **Política Estricta de Cero Comentarios**:
   - Queda estrictamente prohibido añadir cualquier tipo de comentarios (línea, bloque, XML) en los archivos de código generados en los bloques de desarrollo.

3. **Desarrollo en Dos Bloques con Pausa de Revisión**:
   - **Bloque 1**: Crear las entidades de Dominio (`DM_...`) y los DTOs de entrada/salida. Detenerse inmediatamente y esperar la aprobación del usuario antes de continuar.
   - **Bloque 2**: Crear interfaces, repositorios, servicios, controladores y registros de inyección de dependencias en `Program.cs`.

4. **Diferenciación de Listados y Filtros (Consultas)**:
   - Dado que los procedimientos de listado completo (`List`) y filtrado (`Filter`) suelen devolver estructuras de columnas distintas en base de datos, se deben separar estrictamente en entidades de dominio independientes (ej. `DM_Table_listar` y `DM_Table_filtrar`) y DTOs de respuesta independientes (ej. `TableListarDTOs` y `TableFiltrarDTOs`).

5. **Manejo de Parámetros de Filtro Únicos**:
   - Si un procedimiento de filtrado tiene un único parámetro de entrada (ej. `@SearchTerm`), este debe pasarse directamente como un parámetro en las firmas de los métodos (`string? searchTerm`) del controlador, servicio y repositorio, en lugar de crear una clase DTO de entrada. Si el filtro tiene múltiples parámetros, se utilizará una clase DTO agrupada (ej. `TableFilterDTOs`).

6. **Diseño de Endpoints de Eliminación (Delete)**:
   - Los stored procedures de eliminación lógica (`Delete`) no requieren de clases de dominio ni de DTOs específicos. Se deben manejar pasando directamente los parámetros primitivos (ej. `id` y `idModificador`) como parámetros de ruta en el endpoint `[HttpDelete("{id}/{idModificador}")]`.

7. **Variables Globales de Salida**:
   - Todos los parámetros de salida de base de datos (`@o_code`, `@o_message`, `@o_templateId`) se reciben de forma global en la clase `DOMAIN.VariablesSalida.OUTPUT`.