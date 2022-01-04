using AproturWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AproturWeb.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<CoberturaDocumento> CoberturaDocumentos { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<KMLMunicipio> kMLMunicipios { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<MateriaDocumento> MateriaDocumentos { get; set; }
        public DbSet<DocumentoProyecto> DocumentoProyectos { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Objeto> Objetos { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Subregion> Subregiones { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }
        public DbSet<FormatoDocumento> FormatosDocumentos { get; set; }
        public DbSet<TipoFuenteBibliografica> TiposFuenteBibliograficas { get; set; }
        public DbSet<PermisosPorUsuario> PermisosPorUsuarios { get; set; }
        public DbSet<PermisosPorRol> PermisosPorRoles { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<RespuestaComentario> RespuestaComentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CoberturaDocumento>().HasIndex(p => new { p.MunicipioId, p.DocumentoId }).IsUnique();
            modelBuilder.Entity<Departamento>().HasIndex(p => new { p.PaisId, p.Nombre }).IsUnique();
            modelBuilder.Entity<Documento>().HasIndex(p => new { p.Nombre }).IsUnique();
            modelBuilder.Entity<KMLMunicipio>().HasIndex(p => new { p.RutaKML, p.MunicipioId }).IsUnique();
            modelBuilder.Entity<Materia>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Municipio>().HasIndex(p => new { p.Nombre, p.DepartamentoId }).IsUnique();
            modelBuilder.Entity<MateriaDocumento>().HasIndex(p => new { p.MateriaId, p.DocumentoId }).IsUnique();
            modelBuilder.Entity<DocumentoProyecto>().HasIndex(p => new { p.ProyectoId, p.DocumentoId }).IsUnique();
            modelBuilder.Entity<Objeto>().HasIndex(p => new { p.Nombre }).IsUnique();
            modelBuilder.Entity<Pais>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Proyecto>().HasIndex(p => new { p.Nombre }).IsUnique();
            modelBuilder.Entity<Subregion>().HasIndex(p => new { p.Nombre, p.DepartamentoId }).IsUnique();
            modelBuilder.Entity<TipoDocumento>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<TipoFuenteBibliografica>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<FormatoDocumento>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<User>(b => {b.ToTable("Users", "Seguridad");});
            modelBuilder.Entity<IdentityUserClaim<string>>(b => {b.ToTable("UserClaims", "Seguridad");});
            modelBuilder.Entity<IdentityUserLogin<string>>(b => {b.ToTable("UserLogins", "Seguridad");});
            modelBuilder.Entity<IdentityUserToken<string>>(b => {b.ToTable("UserTokens", "Seguridad");});
            modelBuilder.Entity<IdentityRole>(b => {b.ToTable("Roles", "Seguridad");});
            modelBuilder.Entity<IdentityRoleClaim<string>>(b => {b.ToTable("RoleClaims", "Seguridad");});
            modelBuilder.Entity<IdentityUserRole<string>>(b => {b.ToTable("UserRoles", "Seguridad");});
            modelBuilder.Entity<PermisosPorUsuario>().HasIndex(p => new { p.ObjetoId,p.UserId }).IsUnique();
            modelBuilder.Entity<PermisosPorRol>().HasIndex(p => new { p.ObjetoId, p.RolId }).IsUnique();
        }

    }
}


//
//PM> EntityFrameworkcore\Add-Migration InitialDb
//PM >EntityFrameworkcore\Update-Database

