using Microsoft.EntityFrameworkCore;
using Solcimet.Api.Entities;

namespace Solcimet.Api.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(
            DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<Roles> Rol => Set<Roles>();
        public DbSet<Usuarios> Usuario => Set<Usuarios>();
        public DbSet<Terceros> Tercero => Set<Terceros>();
        public DbSet<Responsables> Responsable => Set<Responsables>();
        public DbSet<Obras> Obra => Set<Obras>();
        public DbSet<Facturas> Factura => Set<Facturas>();
        public DbSet<Factura_archivos> Factura_archivo => Set<Factura_archivos>();
        public DbSet<Acuerdos_pago> Acuerdo_pago => Set<Acuerdos_pago>();
        public DbSet<Pagos> Pago => Set<Pagos>();
        public DbSet<Auditoria> Auditorias => Set<Auditoria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(k => k.Id_rol);
               

                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
                .HasColumnName("actualizado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasOne(r => r.Rol)
                    .WithMany()
                    .HasForeignKey(k => k.Id_rol);

                entity.HasKey(k => k.Id_usuario);

                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
                .HasColumnName("actualizado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<Responsables>(entity =>
            {
                entity.ToTable("responsables");
                entity.HasKey(k => k.Id_responsable);
                entity.HasOne(r => r.Usuario)
                    .WithOne()
                    .HasForeignKey<Responsables>(k => k.Id_usuario);

                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
                .HasColumnName("actualizado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<Terceros>(entity =>
            {
                entity.ToTable("terceros");
                entity.HasKey(k=> k.Id_tercero);

                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
                .HasColumnName("actualizado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<Obras>(entity =>
            {
                entity.ToTable("obras");
                entity.HasKey(k => k.Id_obra);
                entity.HasOne(r => r.Cliente)
                    .WithMany()
                    .HasForeignKey(k => k.Id_cliente);
                    


                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
               .HasColumnName("actualizado_en")
               .HasColumnType("timestamp with time zone")
               .HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<Facturas>(entity =>
            {
                entity.ToTable("facturas");
                entity.HasKey(k=> k.Id_factura);
                entity.HasOne(r => r.Tercero)
                .WithMany()
                .HasForeignKey(k => k.Id_tercero);
                entity.HasOne(r => r.Obra)
                .WithMany()
                .HasForeignKey(k => k.Id_obra);
                entity.HasOne(r => r.Responsable)
                .WithMany()
                .HasForeignKey(k => k.Id_responsable);
                entity.HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(k => k.Id_usuario);


                entity.Property(p => p.Fecha_registro)
                .HasColumnName("fecha_registro")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
               .HasColumnName("actualizado_en")
               .HasColumnType("timestamp with time zone")
               .HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<Factura_archivos>(entity =>
            {
                entity.ToTable("factura_archivos");
                entity.Property(p => p.Id_archivo).UseIdentityAlwaysColumn();
                entity.HasKey(k => new
                {
                    k.Id_factura,
                    k.Id_archivo
                });
                entity.HasOne(r => r.Factura)
                .WithMany()
                .HasForeignKey(k => k.Id_factura);
     

                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

      
            });
            modelBuilder.Entity<Acuerdos_pago>(entity =>
            {
                entity.ToTable("acuerdos_pago");
                entity.HasKey(k => new
                {
                    k.Id_tercero, k.Id_obra
                });
                entity.HasOne(r => r.Obra)
                .WithMany()
                .HasForeignKey(k => k.Id_obra);

                entity.HasOne(r => r.Tercero)
                .WithMany()
                .HasForeignKey(k => k.Id_tercero);

                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

                entity.Property(p => p.Actualizado_en)
                .HasColumnName("actualizado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Pagos>(entity =>
            {
                entity.ToTable("pagos");
                entity.HasKey(k => k.Id_pago);
                entity.HasOne(r => r.Obra)
                    .WithMany()
                    .HasForeignKey(k => k.Id_obra);
                entity.HasOne(r => r.Tercero)
                    .WithMany()
                    .HasForeignKey(k => k.Id_tercero);
                
                entity.Property(p => p.Creado_en)
                .HasColumnName("creado_en")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.ToTable("auditoria");
                entity.HasKey(k => k.Id_auditoria);

                entity.Property(p => p.Fecha_hora)
                .HasColumnName("fecha_hora")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

              
            });
           
        }
    }
}
