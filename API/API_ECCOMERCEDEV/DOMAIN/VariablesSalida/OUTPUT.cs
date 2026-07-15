using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.VariablesSalida
{
    public class OUTPUT
    {
        public int? Code { get; set; }
        public string? Message { get; set; }
        public int? TemplateId { get; set; }

        //JWT Token
        public object? Data { get; set; }

        // Propiedad calculada útil para los Controladores
        public bool IsSuccess => Code >= 0;
    }
}