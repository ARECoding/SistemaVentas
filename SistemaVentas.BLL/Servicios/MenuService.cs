using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.BLL.Servicios.Contrato;


namespace SistemaVentas.BLL.Servicios
{
    public class MenuService : IMenuService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Usuario> _userRepository;
        private readonly IGenericRepository<MenuRol> _menuRoleRepository;
        private readonly IGenericRepository<Menu> _menuRepository;

        public MenuService(IMapper mapper, IGenericRepository<Usuario> userRepository, IGenericRepository<MenuRol> menuRoleRepository, IGenericRepository<Menu> menuRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _menuRoleRepository = menuRoleRepository;
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> userTable = await _userRepository.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> menuRoleTable = await _menuRoleRepository.Consultar();
            IQueryable<Menu> menuTable = await _menuRepository.Consultar();

            try
            {
                IQueryable<Menu> resultTable = (from user in userTable
                                                join menuRole in menuRoleTable on user.IdRol equals menuRole.IdRol
                                                join menu in menuTable on menuRole.IdMenu equals menu.IdMenu
                                                select menu).AsQueryable();
                var menuList = resultTable.ToList();
                return _mapper.Map<List<MenuDTO>>(menuList);
            }
            catch 
            {
                throw;
            }
            
        }
    }
}
