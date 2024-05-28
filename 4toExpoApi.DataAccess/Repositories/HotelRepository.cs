using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using _4toExpoApi.DataAccess.Response.Hotel;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _4toExpoApi.DataAccess.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly _4toExpoDbContext dbContext;

        public HotelRepository(_4toExpoDbContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<GenericResponse<Hotel>> AgregarHotel(Hotel hotel, List<Habitacion> listHabitacion, List<Distancia> listaDistancia, int userAlt, ILogger logger)
        {
            try
            {
                using var transaction = dbContext.Database.BeginTransaction();
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var itemsGuardados = 0;
                var response = new GenericResponse<Hotel>();
                var addHotel = dbContext.Add(hotel);
                var addResult = await dbContext.SaveChangesAsync();

                foreach (var item in listHabitacion)
                {
                    item.IdHotel = addHotel.Entity.Id;
                    item.Activo = true;
                    item.UserAlt = userAlt;
                    item.FechaAlt = DateTime.Now;
                }

                foreach (var item in listaDistancia)
                {
                    item.IdHotel = addHotel.Entity.Id;
                    item.Activo = true;
                    item.UserAlt = userAlt;
                    item.FechaAlt = DateTime.Now;
                }
                await dbContext.Habitacion.AddRangeAsync(listHabitacion);
                await dbContext.Distancia.AddRangeAsync(listaDistancia);
                itemsGuardados = await _context.SaveChangesAsync();

                if (itemsGuardados > 0)
                {
                    transaction.Commit();
                    response.Success = true;
                    response.CreatedId = addHotel.Entity.Id.ToString();
                }
                else
                {
                    transaction.Rollback();
                    response.Success = false;
                }


                response.Data = addHotel.Entity;

                return response;
            }
            catch (SqlException ex)
            {

                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<List<HotelResponse>>> ListaHotel(ILogger logger)
        {
            try
            {
                
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
               
                var response = new GenericResponse<List<HotelResponse>>();

                var dataListHotel = await dbContext.Hotel.Where(x => x.Activo == true).Select(hotel => new HotelResponse
                {
                    Id = hotel.Id,
                    Nombre = hotel.Nombre,
                    Tipo = hotel.Tipo,
                    Ubicacion = hotel.Ubicacion,
                    Tarifa = hotel.Tarifa,
                    Telefono = hotel.Telefono,
                    LinkWhatsapp = hotel.LinkWhatsapp,
                    Correo = hotel.Correo,
                    Imagen = hotel.Imagen,
                    CodigoReserva = hotel.CodigoReserva,
                    listaHabitacion = dbContext.Habitacion.Where(habitacion => habitacion.IdHotel == hotel.Id).Select(v => new HabitacionResponse()
                    {
                        Id = v.Id,
                        Nombre = v.Nombre,
                        Adicional = v.Adicional,
                        Impuesto = v.Impuesto,
                        incluye = v.incluye,
                        Precio = v.Precio,
                    }).ToList(),
                    listaDistancia = dbContext.Distancia.Where(distancia => distancia.IdHotel == hotel.Id).Select(v => new DistanciaResponse()
                    {
                        Id = v.Id,
                        Icono = v.Icono,
                        Tiempo = v.Tiempo,
                        Tipo = v.Tipo,


                    }).ToList(),

                }).ToListAsync();

                if (dataListHotel.Count > 0)
                {
                    response.Data = dataListHotel;
                    response.Message = "success";
                    response.Success = true;
                }
                else
                {
                    
                    response.Message = "No se encontraron datos";
                    response.Success = false;
                }


                return response;
            }
            catch (SqlException ex)
            {

                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<HotelResponse>> HotelPorID(int idHotel, ILogger logger)
        {
            try
            {

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<HotelResponse>();

                var dataListHotel = await dbContext.Hotel.Where(hotel=> hotel.Id == idHotel).Select(hotel => new HotelResponse
                {
                    Id = hotel.Id,
                    Nombre = hotel.Nombre,
                    Tipo = hotel.Tipo,
                    Ubicacion = hotel.Ubicacion,
                    Tarifa = hotel.Tarifa,
                    Telefono = hotel.Telefono,
                    LinkWhatsapp = hotel.LinkWhatsapp,
                    Correo = hotel.Correo,
                    Imagen = hotel.Imagen,
                    CodigoReserva = hotel.CodigoReserva,
                    listaHabitacion = dbContext.Habitacion.Where(habitacion => habitacion.IdHotel == hotel.Id).Select(v => new HabitacionResponse()
                    {
                        Nombre = v.Nombre,
                        Adicional = v.Adicional,
                        Impuesto = v.Impuesto,
                        incluye = v.incluye,
                        Precio = v.Precio,
                    }).ToList(),
                    listaDistancia = dbContext.Distancia.Where(distancia => distancia.IdHotel == hotel.Id).Select(v => new DistanciaResponse()
                    {
                        Id = v.Id,
                        Icono = v.Icono,
                        Tiempo = v.Tiempo,
                        Tipo = v.Tipo,


                    }).ToList(),

                }).FirstAsync();

                if (dataListHotel != null)
                {
                    response.Data = dataListHotel;
                    response.Message = "success";
                    response.Success = true;
                }
                else
                {

                    response.Message = "No se encontraron datos";
                    response.Success = false;
                }


                return response;
            }
            catch (SqlException ex)
            {

                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<Hotel>> ActualizarHotel(Hotel hotel, List<Habitacion> listHabitacion, List<Distancia> listaDistancia, int userAlt, ILogger logger)
        {
            try
            {
                using var transaction = dbContext.Database.BeginTransaction();
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var itemsGuardados = 0;
                var response = new GenericResponse<Hotel>();
                var addHotel = dbContext.Update(hotel);
                var addResult = await dbContext.SaveChangesAsync();

                foreach (var item in listHabitacion)
                {
                    
                    if(item.Id > 0)
                    {
                        item.UserUpd = userAlt;
                        item.FechaUpd = DateTime.Now;
                        dbContext.Habitacion.Update(item);
                    }
                    else
                    {
                        item.Activo = true;
                        item.UserAlt = userAlt;
                        item.FechaAlt = DateTime.Now;
                        dbContext.Habitacion.Add(item);
                    }
                }

                foreach (var item in listaDistancia)
                {
                    if (item.Id > 0)
                    {
                        item.UserUpd = userAlt;
                        item.FechaUpd = DateTime.Now;
                        dbContext.Distancia.Update(item);
                    }
                    else
                    {
                        item.Activo = true;
                        item.UserAlt = userAlt;
                        item.FechaAlt = DateTime.Now;
                        dbContext.Distancia.Add(item);
                    }
                }
                 
                
                itemsGuardados = await _context.SaveChangesAsync();

                if (itemsGuardados > 0)
                {
                    transaction.Commit();
                    response.Success = true;
                    
                }
                else
                {
                    transaction.Rollback();
                    response.Success = false;
                }


                response.Data = addHotel.Entity;

                return response;
            }
            catch (SqlException ex)
            {

                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }


    }
}
