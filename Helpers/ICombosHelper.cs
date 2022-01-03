
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AproturWeb.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboTiposDocumento();

       
    }
}
