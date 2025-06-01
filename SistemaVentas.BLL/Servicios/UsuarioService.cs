using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Usuario> _userRepository;

        public UsuarioService(IGenericRepository<Usuario> repository, IMapper mapper) 
        { 
            _mapper = mapper;
            _userRepository = repository;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var userQuery = await _userRepository.Consultar();
                var userList = userQuery.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(userList);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try 
            {
                var userQuery = await _userRepository.Consultar(user =>
                user.Correo == correo &&
                user.Clave == clave);
                if(userQuery.FirstOrDefault() is null)
                    throw new TaskCanceledException("User or password not found.");
                Usuario returnedUser = userQuery.Include(role =>
                role.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(returnedUser);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var createdUser = await _userRepository.Crear(_mapper.Map<Usuario>(modelo));
                if(createdUser.IdUsuario == 0)
                    throw new TaskCanceledException("User couldnt be created");
                var userQuery = await _userRepository.Consultar(user => user.IdUsuario == createdUser.IdUsuario);
                createdUser = userQuery.Include(role => role.IdRolNavigation).FirstOrDefault();
                
                return _mapper.Map<UsuarioDTO>(createdUser);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            var modelUser = _mapper.Map<Usuario>(modelo);
            var userFound = await _userRepository.Obtener(user => user.IdUsuario == modelUser.IdUsuario);
            if (userFound.IdUsuario == 0)
                throw new TaskCanceledException("User doesnt exists");

            userFound.NombreCompleto = modelUser.NombreCompleto;
            userFound.Correo = modelUser.Correo;
            userFound.Clave = modelUser.Clave;
            userFound.IdRol = modelUser.IdRol;
            userFound.EsActivo = modelUser.EsActivo;
            
            bool hasResponse = await _userRepository.Editar(userFound);
            if (!hasResponse)
                throw new TaskCanceledException("User couldnt be edited");
            return hasResponse;
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                var userFound = await _userRepository.Obtener(user => user.IdUsuario == idUsuario);
                if (userFound.IdUsuario == 0)
                    throw new TaskCanceledException("User doesnt exists");
                bool hasResponse = await _userRepository.Eliminar(userFound);
                if(!hasResponse)
                    throw new TaskCanceledException("User couldnt be deleted");
                return hasResponse;
            }
            catch 
            {
                throw;
            }
        }
    }
}
