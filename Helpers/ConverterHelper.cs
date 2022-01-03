using GeoPlus.Data;
using GeoPlus.Data.Entities;
using GeoPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Documento = model.Document,
                TipoDocumento = await _context.TiposDocumento.FindAsync(model.TipoDocumentoId),
                Email = model.Email,
                Nombres = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                Apellidos = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserType = model.UserType,
            };
        }

        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                Document = user.Documento,
                TipoDocumentoId = user.TipoDocumento.Id,
                TiposDeDocumentos = _combosHelper.GetComboTiposDocumento(),
                Email = user.Email,
                FirstName = user.Nombres,
                Id = user.Id,
                LastName = user.Apellidos,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
            };
        }

    }
}
