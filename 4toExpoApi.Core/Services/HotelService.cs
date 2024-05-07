

using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace _4toExpoApi.Core.Services
{
    public class HotelService
    {
        #region <---Variables--->
        private readonly IHotelRepository _hotelRepository;
        private ILogger<HotelService> _logger;
        #endregion
        #region <---Constructor--->
        public HotelService(IHotelRepository hotelrepo, ILogger<HotelService> logger)
        {
            _hotelRepository = hotelrepo;
            _logger = logger;
        }
        #endregion
        #region <---Metodos--->

        public async Task<GenericResponse<HotelRequest>> AgregarHotel(HotelRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<HotelRequest>();


                var NuevoHotel = new Hotel()
                    {
                        Nombre = request.Nombre,
                        Tipo = request.Tipo,
                        Ubicacion= request.Ubicacion,
                        Telefono = request.Telefono,
                        LinkWhatsapp= request.LinkWhatsapp,
                        CodigoReserva= request.CodigoReserva,
                        Imagen= request.Imagen,
                        Tarifa= request.Tarifa,
                    };

                NuevoHotel.FechaAlt = DateTime.Now;
                NuevoHotel.UserAlt = userAlt;
                NuevoHotel.Activo = true;

                var add = await _hotelRepository.AgregarHotel(NuevoHotel, _logger);
                if (add.Success)
                {
                    response.Data = request;
                    response.Message = "Se agrego Hotel";
                    response.Success = true;
                    response.CreatedId = add.CreatedId;
                }
                else
                {
                    response.Data = request;
                    response.Message = "No se pudo agregar el hotel";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }


        #endregion

    }
}
