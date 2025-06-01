using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SistemaVentas.BLL.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoryRepository, IMapper mapper) 
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var categoryQuery = await _categoryRepository.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(categoryQuery.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}
