using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BARSTOC_DAL.DBContext;

public partial class DbBarstocContext : DbContext
{
    public DbBarstocContext()
    {
    }

    public DbBarstocContext(DbContextOptions<DbBarstocContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TBL_Categoria> TblCategoria { get; set; }

    public virtual DbSet<TBL_Detalle_Pedidos> TblDetallePedidos { get; set; }

    public virtual DbSet<TBL_Inventario> TblInventarios { get; set; }

    public virtual DbSet<TBL_Menu> TblMenus { get; set; }

    public virtual DbSet<TBL_MenuRol> TblMenuRols { get; set; }

    public virtual DbSet<TBL_Mesas> TblMesas { get; set; }

    public virtual DbSet<TBL_Movimientos_Inventario> TblMovimientosInventarios { get; set; }

    public virtual DbSet<TBL_Numero_Pedido> TblNumeroPedidos { get; set; }

    public virtual DbSet<TBL_Pedidos> TblPedidos { get; set; }

    public virtual DbSet<TBL_Producto> TblProductos { get; set; }

    public virtual DbSet<TBL_Roles> TblRoles { get; set; }

    public virtual DbSet<TBL_Sedes> TblSedes { get; set; }

    public virtual DbSet<TBL_Sesiones_Usuario> TblSesionesUsuarios { get; set; }

    public virtual DbSet<TBL_Usuarios> TblUsuarios { get; set; }

    public virtual DbSet<VwInventarioActual> VwInventarioActuals { get; set; }

    public virtual DbSet<VwPedidosActivo> VwPedidosActivos { get; set; }

    public virtual DbSet<VwProductosPreciosFormateado> VwProductosPreciosFormateados { get; set; }

    public virtual DbSet<VwReporteVenta> VwReporteVentas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

        modelBuilder.Entity<TBL_Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__TBL_Cate__8A3D240C760AD770");

            entity.ToTable("TBL_Categoria");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("activa")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombreCategoria");
        });

        modelBuilder.Entity<TBL_Detalle_Pedidos>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__TBL_Deta__610F0056704768AB");

            entity.ToTable("TBL_Detalle_Pedidos", tb => tb.HasTrigger("TR_After_Insert_DetallePedido"));

            entity.HasIndex(e => e.IdPedido, "IX_DetallePedidos_Pedido");

            entity.HasIndex(e => e.IdProducto, "IX_DetallePedidos_Producto");

            entity.Property(e => e.IdDetallePedido).HasColumnName("idDetallePedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioUnitario");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.TblDetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK_DetallePedidos_Pedido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.TblDetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallePedidos_Producto");
        });

        modelBuilder.Entity<TBL_Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("PK__TBL_Inve__8F145B0D11AE15C5");

            entity.ToTable("TBL_Inventario");

            entity.HasIndex(e => new { e.IdSede, e.IdProducto }, "IX_Inventario_Sede_Producto");

            entity.HasIndex(e => new { e.IdSede, e.IdProducto }, "UK_Producto_Sede").IsUnique();

            entity.Property(e => e.IdInventario).HasColumnName("idInventario");
            entity.Property(e => e.CantidadDisponible).HasColumnName("cantidadDisponible");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSede).HasColumnName("idSede");
            entity.Property(e => e.UltimaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ultimaActualizacion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.TblInventarios)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Producto");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.TblInventarios)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Sedes");
        });

        modelBuilder.Entity<TBL_Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PK__TBL_Menu__C26AF48383695F05");

            entity.ToTable("TBL_Menu");

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .HasColumnName("icono");
            entity.Property(e => e.NombreMenu)
                .HasMaxLength(50)
                .HasColumnName("nombreMenu");
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .HasColumnName("url");
        });

        modelBuilder.Entity<TBL_MenuRol>(entity =>
        {
            entity.HasKey(e => e.IdMenuRol).HasName("PK__TBL_Menu__9D6D61A489F4C68E");

            entity.ToTable("TBL_MenuRol");

            entity.Property(e => e.IdMenuRol).HasColumnName("idMenuRol");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.TblMenuRols)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuRol_Menu");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TblMenuRols)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuRol_Rol");
        });

        modelBuilder.Entity<TBL_Mesas>(entity =>
        {
            entity.HasKey(e => e.IdMesa).HasName("PK__TBL_Mesa__C26D1DFFD669D2FE");

            entity.ToTable("TBL_Mesas");

            entity.HasIndex(e => new { e.IdSede, e.NumeroMesa }, "UK_Mesa_Sede").IsUnique();

            entity.Property(e => e.IdMesa).HasColumnName("idMesa");
            entity.Property(e => e.Capacidad)
                .HasDefaultValue(4)
                .HasColumnName("capacidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("libre")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdSede).HasColumnName("idSede");
            entity.Property(e => e.NumeroMesa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numeroMesa");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.TblMesas)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mesas_Sedes");
        });

        modelBuilder.Entity<TBL_Movimientos_Inventario>(entity =>
        {
            entity.HasKey(e => e.IdMovimientoInventario).HasName("PK__TBL_Movi__E778DA2E86496A77");

            entity.ToTable("TBL_Movimientos_Inventario");

            entity.HasIndex(e => e.FechaMovimiento, "IX_Movimientos_Fecha");

            entity.Property(e => e.IdMovimientoInventario).HasColumnName("idMovimientoInventario");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CantidadAnterior).HasColumnName("cantidadAnterior");
            entity.Property(e => e.CantidadNueva).HasColumnName("cantidadNueva");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaMovimiento");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSede).HasColumnName("idSede");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipoMovimiento");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.TblMovimientosInventarios)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Productos");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.TblMovimientosInventarios)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Sedes");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TblMovimientosInventarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Usuarios");
        });

        modelBuilder.Entity<TBL_Numero_Pedido>(entity =>
        {
            entity.HasKey(e => e.IdNumeroPedido).HasName("PK__TBL_Nume__3919815499630C47");

            entity.ToTable("TBL_Numero_Pedido");

            entity.Property(e => e.IdNumeroPedido).HasColumnName("idNumeroPedido");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Prefijo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("PED-")
                .HasColumnName("prefijo");
            entity.Property(e => e.UltimoNumero).HasColumnName("ultimoNumero");
        });

        modelBuilder.Entity<TBL_Pedidos>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__TBL_Pedi__A9F619B716D50EB8");

            entity.ToTable("TBL_Pedidos", tb =>
                {
                    tb.HasTrigger("TR_After_Insert_Pedido");
                    tb.HasTrigger("TR_After_Update_Pedido_Cancelado");
                    tb.HasTrigger("TR_After_Update_Pedido_Estado");
                    tb.HasTrigger("TR_Before_Insert_Pedido");
                });

            entity.HasIndex(e => e.EstadoPedido, "IX_Pedidos_Estado");

            entity.HasIndex(e => e.IdMesa, "IX_Pedidos_Mesa");

            entity.HasIndex(e => e.IdSede, "IX_Pedidos_Sede");

            entity.HasIndex(e => e.IdUsuarioMesero, "IX_Pedidos_Usuario");

            entity.HasIndex(e => e.NumeroPedido, "UQ__TBL_Pedi__90DD6149BC259ACF").IsUnique();

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.EstadoPedido)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("activo")
                .HasColumnName("estadoPedido");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaApertura");
            entity.Property(e => e.FechaCierre).HasColumnName("fechaCierre");
            entity.Property(e => e.IdMesa).HasColumnName("idMesa");
            entity.Property(e => e.IdSede).HasColumnName("idSede");
            entity.Property(e => e.IdUsuarioMesero).HasColumnName("idUsuarioMesero");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("efectivo")
                .HasColumnName("metodoPago");
            entity.Property(e => e.NumeroPedido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroPedido");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");
            entity.Property(e => e.TotalPedido)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("totalPedido");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.TblPedidos)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Mesas");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.TblPedidos)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Sedes");

            entity.HasOne(d => d.IdUsuarioMeseroNavigation).WithMany(p => p.TblPedidos)
                .HasForeignKey(d => d.IdUsuarioMesero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Usuarios");
        });

        modelBuilder.Entity<TBL_Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__TBL_Prod__07F4A132DEE03506");

            entity.ToTable("TBL_Producto");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("activo")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioVenta");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.TblProductos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");
        });

        modelBuilder.Entity<TBL_Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__TBL_Role__3C872F7622AB7873");

            entity.ToTable("TBL_Roles");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<TBL_Sedes>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__TBL_Sede__C5AF63D0487E0EAD");

            entity.ToTable("TBL_Sedes");

            entity.Property(e => e.IdSede).HasColumnName("idSede");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("activa")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .HasColumnName("nombreSede");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<TBL_Sesiones_Usuario>(entity =>
        {
            entity.HasKey(e => e.IdSesionUsuario).HasName("PK__TBL_Sesi__D4864B637B1B8242");

            entity.ToTable("TBL_Sesiones_Usuario");

            entity.HasIndex(e => e.Estado, "IX_Sesiones_Estado");

            entity.HasIndex(e => e.IdUsuario, "IX_Sesiones_Usuario");

            entity.Property(e => e.IdSesionUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("idSesionUsuario");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("activa")
                .HasColumnName("estado");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.FechaUltimaActividad)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaUltimaActividad");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.UserAgent).HasColumnName("userAgent");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TblSesionesUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sesiones_Usuarios");
        });

        modelBuilder.Entity<TBL_Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__TBL_Usua__645723A600CF2CF7");

            entity.ToTable("TBL_Usuarios");

            entity.HasIndex(e => e.IdRol, "IX_Usuarios_Rol");

            entity.HasIndex(e => e.IdSede, "IX_Usuarios_Sede");

            entity.HasIndex(e => e.Correo, "UQ__TBL_Usua__2A586E0B16AAB4EE").IsUnique();

            entity.HasIndex(e => e.NumeroDocumento, "UQ__TBL_Usua__4CC511E41741BC09").IsUnique();

            entity.HasIndex(e => e.UsuarioLogin, "UQ__TBL_Usua__7BEBDB16FD2444C0").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(100)
                .HasColumnName("apellidoUsuario");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("activo")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaUltimoLogin).HasColumnName("fechaUltimoLogin");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.IdSede).HasColumnName("idSede");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombreUsuario");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
            entity.Property(e => e.UsuarioLogin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuarioLogin");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TblUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.TblUsuarios)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Sedes");
        });

        modelBuilder.Entity<VwInventarioActual>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_Inventario_Actual");

            entity.Property(e => e.CantidadDisponible).HasColumnName("cantidadDisponible");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombreCategoria");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .HasColumnName("nombreSede");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioVenta");
            entity.Property(e => e.UltimaActualizacion).HasColumnName("ultimaActualizacion");
        });

        modelBuilder.Entity<VwPedidosActivo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_Pedidos_Activos");

            entity.Property(e => e.FechaApertura).HasColumnName("fechaApertura");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Mesero)
                .HasMaxLength(201)
                .HasColumnName("mesero");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .HasColumnName("nombreSede");
            entity.Property(e => e.NumeroMesa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numeroMesa");
            entity.Property(e => e.NumeroPedido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroPedido");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");
            entity.Property(e => e.TotalPedido)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("totalPedido");
        });

        modelBuilder.Entity<VwProductosPreciosFormateado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_Productos_Precios_Formateados");

            entity.Property(e => e.Costo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.CostoFormateado)
                .HasMaxLength(4000)
                .HasColumnName("costoFormateado");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Margen)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("margen");
            entity.Property(e => e.MargenFormateado)
                .HasMaxLength(4000)
                .HasColumnName("margenFormateado");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombreCategoria");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioVenta");
            entity.Property(e => e.PrecioVentaFormateado)
                .HasMaxLength(4000)
                .HasColumnName("precioVentaFormateado");
        });

        modelBuilder.Entity<VwReporteVenta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_Reporte_Ventas");

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.GananciaTotal)
                .HasColumnType("decimal(24, 2)")
                .HasColumnName("gananciaTotal");
            entity.Property(e => e.MargenGanancia)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("margenGanancia");
            entity.Property(e => e.Mesero)
                .HasMaxLength(201)
                .HasColumnName("mesero");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombreCategoria");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.NumeroPedido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroPedido");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioVenta");
            entity.Property(e => e.Sede)
                .HasMaxLength(100)
                .HasColumnName("sede");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.TotalPedido)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("totalPedido");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
