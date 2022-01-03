using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GeoPlus.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Proyectos");

            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "Seguridad");

            migrationBuilder.CreateTable(
                name: "Materia",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyecto",
                schema: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KML = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RutaKML = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TipoGeograficoId = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Icono = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyecto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoObjeto",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoObjeto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaisId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamento_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "General",
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Seguridad",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                schema: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Archivo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Ruta = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Portada = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documento_TipoDocumento_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalSchema: "Seguridad",
                        principalTable: "TipoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: true),
                    Documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_TipoDocumento_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalSchema: "Seguridad",
                        principalTable: "TipoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Objeto",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoObjetoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    KeyId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objeto_TipoObjeto_TipoObjetoId",
                        column: x => x.TipoObjetoId,
                        principalSchema: "Seguridad",
                        principalTable: "TipoObjeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Municipio",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodigoDane = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipio_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalSchema: "General",
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subregion",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subregion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subregion_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalSchema: "General",
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoProyecto",
                schema: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentoId = table.Column<int>(type: "int", nullable: false),
                    ProyectoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoProyecto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoProyecto_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalSchema: "Proyectos",
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentoProyecto_Proyecto_ProyectoId",
                        column: x => x.ProyectoId,
                        principalSchema: "Proyectos",
                        principalTable: "Proyecto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MateriaDocumento",
                schema: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentoId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MateriaDocumento_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalSchema: "Proyectos",
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MateriaDocumento_Materia_MateriaId",
                        column: x => x.MateriaId,
                        principalSchema: "General",
                        principalTable: "Materia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguridad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Seguridad",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguridad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Seguridad",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Seguridad",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguridad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Seguridad",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguridad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermisosPorRol",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ObjetoId = table.Column<int>(type: "int", nullable: false),
                    Read = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Insert = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Update = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Delete = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Download = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosPorRol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermisosPorRol_Objeto_ObjetoId",
                        column: x => x.ObjetoId,
                        principalSchema: "Seguridad",
                        principalTable: "Objeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosPorRol_Roles_RolId",
                        column: x => x.RolId,
                        principalSchema: "Seguridad",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermisosPorUsuario",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ObjetoId = table.Column<int>(type: "int", nullable: false),
                    Read = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Insert = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Update = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Delete = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Download = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosPorUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermisosPorUsuario_Objeto_ObjetoId",
                        column: x => x.ObjetoId,
                        principalSchema: "Seguridad",
                        principalTable: "Objeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosPorUsuario_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguridad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoberturaDocumento",
                schema: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentoId = table.Column<int>(type: "int", nullable: false),
                    MunicipioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoberturaDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoberturaDocumento_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalSchema: "Proyectos",
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoberturaDocumento_Municipio_MunicipioId",
                        column: x => x.MunicipioId,
                        principalSchema: "General",
                        principalTable: "Municipio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KMLMunicipio",
                schema: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipioId = table.Column<int>(type: "int", nullable: false),
                    KML = table.Column<byte[]>(type: "varbinary(200)", maxLength: 200, nullable: false),
                    RutaKML = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KMLMunicipio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KMLMunicipio_Municipio_MunicipioId",
                        column: x => x.MunicipioId,
                        principalSchema: "General",
                        principalTable: "Municipio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoberturaDocumento_DocumentoId",
                schema: "Proyectos",
                table: "CoberturaDocumento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_CoberturaDocumento_MunicipioId_DocumentoId",
                schema: "Proyectos",
                table: "CoberturaDocumento",
                columns: new[] { "MunicipioId", "DocumentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departamento_PaisId_Nombre",
                schema: "General",
                table: "Departamento",
                columns: new[] { "PaisId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_Nombre",
                schema: "Proyectos",
                table: "Documento",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_TipoDocumentoId",
                schema: "Proyectos",
                table: "Documento",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoProyecto_DocumentoId",
                schema: "Proyectos",
                table: "DocumentoProyecto",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoProyecto_ProyectoId_DocumentoId",
                schema: "Proyectos",
                table: "DocumentoProyecto",
                columns: new[] { "ProyectoId", "DocumentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KMLMunicipio_MunicipioId",
                schema: "Proyectos",
                table: "KMLMunicipio",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_KMLMunicipio_RutaKML_MunicipioId",
                schema: "Proyectos",
                table: "KMLMunicipio",
                columns: new[] { "RutaKML", "MunicipioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materia_Nombre",
                schema: "General",
                table: "Materia",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MateriaDocumento_DocumentoId",
                schema: "Proyectos",
                table: "MateriaDocumento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaDocumento_MateriaId_DocumentoId",
                schema: "Proyectos",
                table: "MateriaDocumento",
                columns: new[] { "MateriaId", "DocumentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipio_DepartamentoId",
                schema: "General",
                table: "Municipio",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipio_Nombre_DepartamentoId",
                schema: "General",
                table: "Municipio",
                columns: new[] { "Nombre", "DepartamentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objeto_Nombre",
                schema: "Seguridad",
                table: "Objeto",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objeto_TipoObjetoId",
                schema: "Seguridad",
                table: "Objeto",
                column: "TipoObjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pais_Nombre",
                schema: "General",
                table: "Pais",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosPorRol_ObjetoId_RolId",
                schema: "Seguridad",
                table: "PermisosPorRol",
                columns: new[] { "ObjetoId", "RolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosPorRol_RolId",
                schema: "Seguridad",
                table: "PermisosPorRol",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosPorUsuario_ObjetoId_UserId",
                schema: "Seguridad",
                table: "PermisosPorUsuario",
                columns: new[] { "ObjetoId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosPorUsuario_UserId",
                schema: "Seguridad",
                table: "PermisosPorUsuario",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyecto_Nombre",
                schema: "Proyectos",
                table: "Proyecto",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Seguridad",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Seguridad",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subregion_DepartamentoId",
                schema: "General",
                table: "Subregion",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Subregion_Nombre_DepartamentoId",
                schema: "General",
                table: "Subregion",
                columns: new[] { "Nombre", "DepartamentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_Nombre",
                schema: "Seguridad",
                table: "TipoDocumento",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Seguridad",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Seguridad",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Seguridad",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Seguridad",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TipoDocumentoId",
                schema: "Seguridad",
                table: "Users",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Seguridad",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoberturaDocumento",
                schema: "Proyectos");

            migrationBuilder.DropTable(
                name: "DocumentoProyecto",
                schema: "Proyectos");

            migrationBuilder.DropTable(
                name: "KMLMunicipio",
                schema: "Proyectos");

            migrationBuilder.DropTable(
                name: "MateriaDocumento",
                schema: "Proyectos");

            migrationBuilder.DropTable(
                name: "PermisosPorRol",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "PermisosPorUsuario",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Subregion",
                schema: "General");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Proyecto",
                schema: "Proyectos");

            migrationBuilder.DropTable(
                name: "Municipio",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Documento",
                schema: "Proyectos");

            migrationBuilder.DropTable(
                name: "Materia",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Objeto",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Departamento",
                schema: "General");

            migrationBuilder.DropTable(
                name: "TipoObjeto",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "TipoDocumento",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "General");
        }
    }
}
