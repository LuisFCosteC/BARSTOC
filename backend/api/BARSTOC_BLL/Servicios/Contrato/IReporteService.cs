using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IReporteService
    {
        Task<List<DTO_Reporte>> Report(string fechaInicio, string fechaFin);
        Task<List<DTO_Reporte>> ReportBySede(string fechaInicio, string fechaFin, int idSede);
        Task<List<DTO_Reporte>> ReportByProducto(string fechaInicio, string fechaFin, int idProducto);
    }
}
