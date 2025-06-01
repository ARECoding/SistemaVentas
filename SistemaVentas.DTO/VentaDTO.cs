using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    public class VentaDTO
    {
        public int IdVenta { get; set; }
        public string NumeroFactura { get; set; }
        public string TipoPago { get; set; }
        public string Total { get; set; }
        public string FechaRegistro { get; set; }

        public virtual ICollection<DetalleVentaDTO> DetalleVenta { get; set; }
    }
}
