﻿using System;
using System.Collections.Generic;

namespace SistemaVentas.Model;

public partial class DetalleVenta
{
    public int IdDetalleVentas { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Total { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
