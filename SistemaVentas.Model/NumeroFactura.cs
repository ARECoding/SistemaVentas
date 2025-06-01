using System;
using System.Collections.Generic;

namespace SistemaVentas.Model;

public partial class NumeroFactura
{
    public int IdNumeroFactura { get; set; }

    public int UltimoNumero { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
