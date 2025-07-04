﻿using System;
using System.Collections.Generic;

namespace SistemaVentas.Model;

public partial class Venta
{
    public int IdVenta { get; set; }

    public string? NumeroFactura { get; set; }

    public string? TipoPago { get; set; }

    public decimal? Total { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
