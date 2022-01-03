using GeoPlus.Common.Models;
using GeoPlus.Data;
using GeoPlus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using DbUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException;
using Response = GeoPlus.Common.Models.Response;

namespace GeoPlus.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly DataContext _context;

        public ComentarioController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetComentariosProyecto")]
        public Response GetComentariosProyecto(int id)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Message = "",
                Result = null,
            };

            try
            {
                var comentarios = _context.Comentarios
                    .Include(f=> f.RespuestasComentario)
                    .Where(f => f.ProyectoId == id)
                    .Select(s => new ComentarioEnviado
                    { 
                        IdComentario = s.Id,
                        ComentarioDado = s.Descripcion,
                        ProyectoId = s.ProyectoId,
                        Fecha = s.Fecha,
                        RespuestasEnviadas = s.RespuestasComentario
                    }).ToList();
                response.Message = "Consulta realizada correctamente!";
                response.Result = comentarios;
                return response;
            }
            catch (Exception exp)
            {
                response.IsSuccess = false;
                response.Message = exp.Message;
                response.Result = exp;
                return response;
            }
        }

        [HttpGet("GetRespuestas")]
        public Response GetRespuestas(int id)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Message = "",
                Result = null,
            };
            try
            {
                var respuestas = _context.RespuestaComentarios
                    .Where(f => f.ComentarioId == id)
                    .Select(s => new RespuestaEnvianda
                    {
                        IdRespuesta = s.Id,
                        RespuestaDada = s.Respuesta,
                        ComentarioId = s.ComentarioId,
                        Fecha = s.Fecha,
                    }).ToList();
                response.Message = "Consulta realizada correctamente!";
                response.Result = respuestas;
                return response;
            }
            catch (Exception exp)
            {
                response.IsSuccess = false;
                response.Message = exp.Message;
                response.Result = exp;
                return response;
            }
        }

        [HttpPost("AgregarComentario")]
        public Response AgregarComentario(ComentarioEnviado modelo)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Message = "",
                Result = null,
            };

            if (ModelState.IsValid)
            {
                try
                {
                    Data.Entities.Comentario comentario = new Data.Entities.Comentario()
                    {
                        ProyectoId = modelo.ProyectoId,
                        Fecha = DateTime.Now,
                        Descripcion = modelo.ComentarioDado,
                    };

                    _context.Add(comentario);
                    _context.SaveChanges();
                    response.Message = "Comentario almacenado correctamente!";
                    response.Result = comentario;
                    return response;
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        response.Message = "Ya existe el comentario!";
                        return response;
                    }
                    else
                    {
                        response.Message = dbUpdateException.InnerException.Message;
                        return response;
                    }
                }
                catch (Exception exception)
                {
                    response.IsSuccess = false;
                    response.Message = exception.Message;
                    return response;
                }
            }

            return response;
        }


        [HttpPost("AgregarRespuestaComentario")]
        public Response AgregarRespuestaComentario(RespuestaEnvianda modelo)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Message = "",
                Result = null,
            };

            if (ModelState.IsValid)
            {
                try
                {
                    Data.Entities.RespuestaComentario respuesta = new Data.Entities.RespuestaComentario()
                    {
                        ComentarioId = modelo.ComentarioId,
                        Fecha = DateTime.Now,
                        Respuesta = modelo.RespuestaDada,
                    };

                    _context.Add(respuesta);
                    _context.SaveChanges();
                    response.Message = "Comentario almacenado correctamente!";
                    response.Result = respuesta;
                    return response;
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        response.Message = "Ya existe la respuesta al comentario!";
                        return response;
                    }
                    else
                    {
                        response.Message = dbUpdateException.InnerException.Message;
                        return response;
                    }
                }
                catch (Exception exception)
                {
                    response.IsSuccess = false;
                    response.Message = exception.Message;
                    return response;
                }
            }

            return response;
        }
        
    }
}
