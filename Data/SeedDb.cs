using System;
using System.Linq;
using System.Threading.Tasks;
using Aprotur.Common.Enums;
using AproturWeb.Data.Entities;
using AproturWeb.Helpers;

namespace AproturWeb.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckDocumentTypesAsync();
            await CheckDocumentFormatsAsync();
            await CheckDocumentTypesFuentesDocAsync();
            await CheckRolesAsycn();
            await CheckUserAsync("1010", "Admin", "Admin", "aprotur@yopmail.com", "311 322 4620", "Aprotur", UserType.Admin);
            await CheckPaisesAsync();
            await CheckDepartamentosAsync();
            await CheckMunicipiosAsync();
            await CheckSubRegionesAsync();
            await CheckProyectosAsync();
        }

        private async Task CheckUserAsync(string document, string firstName, string lastName, string email, string phoneNumber, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Documento = document,
                    TipoDocumento = _context.TiposDocumento.FirstOrDefault(x => x.Nombre == "Cédula"),
                    Email = email,
                    Nombres = firstName,
                    Apellidos = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRolesAsycn()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }


        private async Task CheckPaisesAsync()
        {
            if (!_context.Paises.Any())
            {
                _context.Paises.Add((new Pais { Nombre = "Argentina" }));
                _context.Paises.Add((new Pais { Nombre = "Bolivia" }));
                _context.Paises.Add((new Pais { Nombre = "Brasil" }));
                _context.Paises.Add((new Pais { Nombre = "Chile" }));
                _context.Paises.Add((new Pais { Nombre = "Colombia" }));
                _context.Paises.Add((new Pais { Nombre = "Ecuador" }));
                _context.Paises.Add((new Pais { Nombre = "Paraguay" }));
                _context.Paises.Add((new Pais { Nombre = "Perú" }));
                _context.Paises.Add((new Pais { Nombre = "Uruguay" }));
                _context.Paises.Add((new Pais { Nombre = "Venezuela" }));
                await _context.SaveChangesAsync();
            }
        }


        private async Task CheckDepartamentosAsync()
        {
            if (!_context.Departamentos.Any())
            {
                var pais = this._context.Paises.Where(f => f.Nombre == "Colombia").FirstOrDefault();
                _context.Departamentos.Add(new Departamento { PaisId = pais.Id, Nombre = "Antioquia" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMunicipiosAsync()
        {
            if (!_context.Municipios.Any())
            {
                var departamento = this._context.Departamentos.Where(f => f.Nombre == "Antioquia").FirstOrDefault();
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Medellín", CodigoDane= "05001" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Barbosa", CodigoDane = "05079" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Bello", CodigoDane = "05088" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Caldas", CodigoDane = "05129" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Copacabana", CodigoDane = "05212" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Envigado", CodigoDane = "05266" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Girardota", CodigoDane = "05308" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Itagui", CodigoDane = "05360" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "La Estrella", CodigoDane = "05380" });
                _context.Municipios.Add(new Municipio { DepartamentoId = departamento.Id, Nombre = "Sabaneta", CodigoDane = "05631" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckSubRegionesAsync()
        {
            if (!_context.Subregiones.Any())
            {
                var departamento = this._context.Departamentos.Where(f => f.Nombre == "Antioquia").FirstOrDefault();
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Oriente" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Suroeste" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Urabá" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Occidente" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Bajo Cauca" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Magdalena Media" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Nordeste" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Norte" });
                _context.Subregiones.Add(new Subregion { DepartamentoId = departamento.Id, Nombre = "Valle de Aburrá" });

                await _context.SaveChangesAsync();
            }
        }
       
        private async Task CheckDocumentFormatsAsync()
        {
            if (!_context.FormatosDocumentos.Any())
            {
                _context.FormatosDocumentos.Add(new FormatoDocumento { Nombre = "PDF", Extension = "pdf" });
                _context.FormatosDocumentos.Add(new FormatoDocumento { Nombre = "Imagen", Extension = "png" });
                _context.FormatosDocumentos.Add(new FormatoDocumento { Nombre = "Audio", Extension = "mp3" });
                _context.FormatosDocumentos.Add(new FormatoDocumento { Nombre = "Vídeo" , Extension= "mp4"});
                _context.FormatosDocumentos.Add(new FormatoDocumento { Nombre = "Base Documental", Extension = "*" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.TiposDocumento.Any())
            {
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Artículo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Artículo de Investigación"});
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Artículo de Revisión" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Artículo Descriptivo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Capítulo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Decreto" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Norma" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Documento de Trabajo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Documento Técnico" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estándar" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estudio" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estudio Colaborativo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estudio Comparado" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estudio Comparativo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estudio de País" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Estudio: Publicación Digital" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Instrumento Legal" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Legislación" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Ley" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Libro" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Noticia" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Objeto de Conferencia" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Publicación Digital" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Reporte" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Revista" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Tesís Doctoral" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Tesís Especialización" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Tesís Maestría" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Tesís Pregrado" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Texto / Declaración" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Texto de Revisión" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Texto descriptivo" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Texto Digital" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Texto Normativo" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDocumentTypesFuentesDocAsync()
        {
            if (!_context.TiposFuenteBibliograficas.Any())
            {
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "ANDI" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Artículo Técnico" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Artículo" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Artículo de Revista" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Artículo de Investigación" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Biblioteca Digital de Chile" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "BID" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "CAF" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Carnegie Endowment for International Peace" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Documento Técnico" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Editorial" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Foro Economico Mundial WEF: Repositorio Digital" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Investigación" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Legislativa" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Ley" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Libro" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "MinCiencias" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "MinTic" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Norma Jurídica" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Normativas OECD" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Normatividad ETSI" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "OCDE" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "ONU" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "ONU/ CEPAL" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Publicación de la Unión Europea" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Publicación Ministerio de Tecnologías de la Información y las Comunicaciones" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Publicación Instituto Nacional de Ciberseguridad (INCIBE)" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Publicación del Instituto Nacional de Estándares y Tecnología" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Red Social de Investigación" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Repositorio UNAD" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Repositorio Universidad Complutense de Madrid" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Repositorio Universidad Francisco de Paula Santander" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Repositorio Universidad Tecnica de Ambato" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Repositorio Universidad de Ciencias y Humanidades" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Revista Shaping Europe´s Digital Future" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Revista" });
                _context.TiposFuenteBibliograficas.Add(new TipoFuenteBibliografica { Nombre = "Revista Académica" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProyectosAsync()
        {
            if (!_context.Proyectos.Any())
            {
                //_context.Proyectos.Add(new Proyecto { Nombre = "Camino Indio", RutaKML= "https://drive.google.com/uc?id=1f3h4451vw1TUEZsHnhbd_8z64QHDvKti", TipoGeograficoId=0 });
                
                //await _context.SaveChangesAsync();
            }
        }

    }
}
