using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;
using AutoMapper;
using SistemaVentas.Model;
using System.Globalization;


namespace SistemaVentas.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol
            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu
            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino => destino.DescripcionRol,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen =>
                origen.EsActivo == true ? 1 : 0)
                );

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                destino.DescripcionRol,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                );

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                destino.IdRolNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                );


            #endregion Usuario
            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion Categoria
            #region Producto
            CreateMap<Producto, ProductoDTO>()
                .ForMember(destino =>
                destino.DescripcionCategoria,
                opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => 
                Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR")))
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                );
            CreateMap<ProductoDTO, Producto>()
                .ForMember(destino =>
                destino.IdCategoriaNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen =>
                Convert.ToDecimal(origen.Precio, new CultureInfo("es-AR")))
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                );
            #endregion Producto
            #region Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen =>
                Convert.ToString(origen.Total.Value, new CultureInfo("es-AR")))
                )
                .ForMember(destino =>
                destino.FechaRegistro,
                opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );
            CreateMap<VentaDTO, Venta>()
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen =>
                Convert.ToDecimal(origen.Total, new CultureInfo("es-AR")))
                );
            #endregion Venta
            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen =>
                Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))
                ))
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen =>
                Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR"))
                ));
            CreateMap<DetalleVentaDTO, DetalleVenta>()
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen =>
                Convert.ToDecimal(origen.Total, new CultureInfo("es-AR")))
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen =>
                Convert.ToDecimal(origen.Precio, new CultureInfo("es-AR")))
                );
            #endregion DetalleVenta
            #region Reporte
            CreateMap<DetalleVenta, ReporteDTO>()
                .ForMember(destino =>
                destino.FechaRegistro,
                opt => opt.MapFrom(origin =>
                origin.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")
                ))
                .ForMember(destino =>
                destino.NumeroFactura,
                opt => opt.MapFrom(origin =>
                origin.IdVentaNavigation.NumeroFactura
                ))
                .ForMember(destino =>
                destino.TipoPago,
                opt => opt.MapFrom(origin =>
                origin.IdVentaNavigation.TipoPago
                ))
                .ForMember(destino =>
                destino.TotalVenta,
                opt => opt.MapFrom(origen =>
                Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-AR"))
                ))
                .ForMember(destino =>
                destino.Producto,
                opt => opt.MapFrom(origin =>
                origin.IdProductoNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen =>
                Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR"))
                ))
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen =>
                Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))
                ));
            #endregion Reporte
        }
    }
}
