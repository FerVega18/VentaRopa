using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models;

public partial class DbAa96f3VentaropaContext : DbContext
{
    public DbAa96f3VentaropaContext()
    {
    }

    public DbAa96f3VentaropaContext(DbContextOptions<DbAa96f3VentaropaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallesOrden> DetallesOrdens { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<EstadoOrden> EstadoOrdens { get; set; }

    public virtual DbSet<Orden> Ordens { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Tarjeta> Tarjeta { get; set; }

    public virtual DbSet<TokenPago> TokenPagos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SQL8006.site4now.net;Initial Catalog=db_aa96f3_ventaropa;User Id=db_aa96f3_ventaropa_admin;Password=A*nvt46Fe-;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__CATEGORI__6378C0202EA6AAC8");

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.CategoriaId).HasColumnName("categoriaID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__CLIENTE__C2FF24BD67EE5384");

            entity.ToTable("CLIENTE");

            entity.Property(e => e.ClienteId)
                .ValueGeneratedNever()
                .HasColumnName("clienteID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Nacimiento).HasColumnName("nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pais");

            entity.HasOne(d => d.NombreUsuarioNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.NombreUsuario)
                .HasConstraintName("FK__CLIENTE__NombreU__3C69FB99");
        });

        modelBuilder.Entity<DetallesOrden>(entity =>
        {
            entity.HasKey(e => new { e.OrdenId, e.ProductoId }).HasName("PK__DETALLES__5967CEB02B4A8A0A");

            entity.ToTable("DETALLES_ORDEN");

            entity.Property(e => e.OrdenId).HasColumnName("ordenID");
            entity.Property(e => e.ProductoId).HasColumnName("productoID");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.Orden).WithMany(p => p.DetallesOrdens)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLES___orden__4F7CD00D");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesOrdens)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLES___produ__5070F446");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.DireccionId).HasName("PK__Direccio__68906D44941104E0");

            entity.ToTable("Direccion");

            entity.Property(e => e.DireccionId).HasColumnName("DireccionID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Direccion__Clien__534D60F1");
        });

        modelBuilder.Entity<EstadoOrden>(entity =>
        {
            entity.HasKey(e => e.EstadoId).HasName("PK__ESTADO_O__C696F36326E5823E");

            entity.ToTable("ESTADO_ORDEN");

            entity.Property(e => e.EstadoId).HasColumnName("estadoID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Orden>(entity =>
        {
            entity.HasKey(e => e.OrdenId).HasName("PK__ORDEN__0FF9A0BBC8BF54CC");

            entity.ToTable("ORDEN");

            entity.Property(e => e.OrdenId).HasColumnName("ordenID");
            entity.Property(e => e.ClienteId).HasColumnName("clienteID");
            entity.Property(e => e.DireccionD)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccionD");
            entity.Property(e => e.EstadoId).HasColumnName("estadoID");
            entity.Property(e => e.NombreD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreD");
            entity.Property(e => e.OrdenFecha).HasColumnName("ordenFecha");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__ORDEN__clienteID__46E78A0C");

            entity.HasOne(d => d.Estado).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("FK__ORDEN__estadoID__47DBAE45");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__PRODUCTO__69E6E0B41FB1034B");

            entity.ToTable("PRODUCTO");

            entity.Property(e => e.ProductoId).HasColumnName("productoID");
            entity.Property(e => e.CategoriaId).HasColumnName("categoriaID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Imagen)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("marca");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.Talla)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("talla");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK__PRODUCTO__catego__4CA06362");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__ROL__54023654819EC1FE");

            entity.ToTable("ROL");

            entity.Property(e => e.RolId).HasColumnName("rolID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Tarjeta>(entity =>
        {
            entity.HasKey(e => e.TarjetaId).HasName("PK__TARJETA__2F3604728985AF27");

            entity.ToTable("TARJETA");

            entity.Property(e => e.TarjetaId).HasColumnName("tarjetaID");
            entity.Property(e => e.ClienteId).HasColumnName("clienteID");
            entity.Property(e => e.Cvc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CVC");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fechaVencimiento");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numero");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__TARJETA__cliente__3F466844");
        });

        modelBuilder.Entity<TokenPago>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__TOKEN_PA__AC16DAA706AC115C");

            entity.ToTable("TOKEN_PAGO");

            entity.Property(e => e.TokenId).HasColumnName("tokenID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.TarjetaId).HasColumnName("tarjetaID");

            entity.HasOne(d => d.Tarjeta).WithMany(p => p.TokenPagos)
                .HasForeignKey(d => d.TarjetaId)
                .HasConstraintName("FK__TOKEN_PAG__tarje__4222D4EF");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NombreUsuario).HasName("PK__USUARIO__6B0F5AE1DC0DA068");

            entity.ToTable("USUARIO");

            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasColumnName("rolID");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK__USUARIO__rolID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
