using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace SistemaVentas.BLL.Servicios
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }


        public async Task<List<RolDTO>> Lista()
        {
            try
            {
                var rolList = await _rolRepository.Consultar();
                return _mapper.Map<List<RolDTO>>(rolList.ToList());
            }
            catch 
            {
                throw;
            }
        }
    }
}
