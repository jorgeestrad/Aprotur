using GeoPlus.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly DataContext _context;

        public ProyectoController(DataContext context)
        {
            _context = context;
        }

       
    }
}
