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
                _context.Paises.Add((new Pais { Nombre = "Colombia" }));
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
       
        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.TiposDocumento.Any())
            {
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "PDF", Extension = "pdf" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Imagen", Extension = "png" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Audio", Extension = "mp3" });
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Vídeo" , Extension= "mp4"});
                _context.TiposDocumento.Add(new TipoDocumento { Nombre = "Base Documental", Extension = "pdf" });
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
