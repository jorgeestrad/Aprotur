
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GeoPlus.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboTiposDocumento();

       
    }
}
