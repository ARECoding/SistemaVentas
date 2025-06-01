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
    public class ProductoService : IProductoService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Producto> _productRepository;

        public ProductoService(IGenericRepository<Producto> productRepository, IMapper mapper) 
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var productQuery = await _productRepository.Consultar();
                var productList = productQuery.Include(category => category.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(productList);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try 
            {
                var productCreated = await _productRepository.Crear(_mapper.Map<Producto>(modelo));
                if(productCreated.IdProducto <= 0)
                    throw new TaskCanceledException("The product couldnt be created");
                return _mapper.Map<ProductoDTO>(productCreated);

            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productModel = _mapper.Map<Producto>(modelo);
                var productFound= await _productRepository.Obtener( p => p.IdProducto == productModel.IdProducto);
                if(productFound == null)
                    throw new TaskCanceledException("The product doesnt exist");
                productFound.Nombre = productModel.Nombre;
                productFound.IdCategoria = productModel.IdCategoria;
                productFound.Stock = productModel.Stock;
                productFound.Precio = productModel.Precio;
                productFound.EsActivo = productModel.EsActivo;

                bool result = await _productRepository.Editar(productFound);

                return result ? true : throw new TaskCanceledException("couldnt edit the product.");

            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productFound = await _productRepository.Obtener(p => p.IdProducto == id);
                if (productFound == null)
                    throw new TaskCanceledException("Product not found.");
                bool result = await _productRepository.Eliminar(productFound);
                return result ? true : throw new TaskCanceledException("couldnt delete the product.");
            }
            catch 
            {
                throw;            
            }
            

        }

        
    }
}
