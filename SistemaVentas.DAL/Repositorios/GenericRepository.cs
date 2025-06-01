using SistemaVentas.DAL.Repositorios.Contrato;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.DBContext;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SistemaVentas.DAL.Repositorios
{
    public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
    {
        private readonly VentasContext _context;

        public GenericRepository(VentasContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                TModelo modelo = await _context.Set<TModelo>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _context.Set<TModelo>().Add(modelo);
                await _context.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _context.Set<TModelo>().Update(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> queryModel = filtro == null
                    ? _context.Set<TModelo>()
                    : _context.Set<TModelo>().Where(filtro);
                return queryModel;
            }
            catch
            {
                throw;
            }
        }

        

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _context.Set<TModelo>().Remove(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
            
        }

        
    }
}
