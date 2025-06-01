using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.DBContext;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.Model;


namespace SistemaVentas.DAL.Repositorios
{
    public class VentaRepositorio : GenericRepository<Venta>, IVentaRepository
    {
        private readonly VentasContext _context;
        public VentaRepositorio(VentasContext context) : base(context)
        {
            _context = context;       
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta detalle in modelo.DetalleVenta)
                    {
                        Producto productoEncontrado = _context.Productos.Where(p => p.IdProducto == detalle.IdProducto).First();
                        productoEncontrado.Stock -= detalle.Cantidad;
                        _context.Productos.Update(productoEncontrado);

                    }
                    await _context.SaveChangesAsync();
                    NumeroFactura correlativo = _context.NumeroFacturas.First();
                    correlativo.UltimoNumero += 1;
                    correlativo.FechaRegistro = DateTime.Now;
                    _context.NumeroFacturas.Update(correlativo);
                    await _context.SaveChangesAsync();
                    int cantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", cantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantidadDigitos, cantidadDigitos);
                    modelo.NumeroFactura = numeroVenta;
                    await _context.Ventas.AddAsync(modelo);
                    await _context.SaveChangesAsync();
                    ventaGenerada = modelo;
                    transaction.Commit();
                }
                catch 
                {
                    transaction.Rollback();
                }
                finally 
                {
                    await _context.DisposeAsync();
                }
            }
            return ventaGenerada;
        }
    }
}
