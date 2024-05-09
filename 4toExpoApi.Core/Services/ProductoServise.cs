using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class ProductoServise
    {
        private readonly IBaseRepository<Productos> _productoRepository;
        private ILogger<ProductoServise> _logger;


        public ProductoServise(IBaseRepository<Productos> productoRepository, ILogger<ProductoServise> logger)
        {
            _productoRepository = productoRepository;
            _logger = logger;
        }
        public async Task<GenericResponse<ProductosRequest>> AgregarProducto(ProductosRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ProductosRequest>();

                var addProducto = AppMapper.Map<ProductosRequest, Productos>(request);

                addProducto.FechaAlt = DateTime.Now;
                addProducto.UserAlt = userAlt;
                addProducto.Activo = true;

                var add = await _productoRepository.Add(addProducto, _logger);
                if (add != null && add.Id > 0)
                {
                    response.Data = request;
                    response.Message = "Se agrego correctamente el producto";
                    response.Success = true;
                    response.CreatedId = add.Id.ToString();
                }
                else
                {
                    response.Data = request;
                    response.Message = "No se pudo agregar correctamente el producto";
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
        public async Task<GenericResponse<ProductosRequest>> EditarProducto(ProductosRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ProductosRequest>();
                var productos = await _productoRepository.GetById(request.Id, _logger);
                if (productos == null)
                {
                    response.Message = "El producto no existe";
                    return response;
                }
                productos.Nombre = request.Nombre;
                productos.Marca = request.Marca;
                productos.Caracteristica = request.Caracteristica;
                productos.Precio = request.Precio;
                productos.Descripcion = request.Descripcion;
                productos.TotalArticulo = request.TotalArticulo;
                productos.UserUpd = UserUpd;
                productos.FechaUpd = DateTime.Now;
                var update = await _productoRepository.Update(productos, _logger);
                if (update != null)
                {
                    response.Message = "Se edito correctamente el producto";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se edito correctamente el producto";
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
        public async Task<List<ProductosRequest>> ObtenerProducto()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listProducto = await _productoRepository.GetAll(_logger);

                if (listProducto == null || listProducto.Count() == 0)
                {
                    return null;
                }
                var listaProductosFiltrada = listProducto.Where(x => x.Activo == true).ToList();

                var requestListProducto = listaProductosFiltrada.Select(producto => AppMapper.Map<Productos, ProductosRequest>(producto)).ToList();


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return requestListProducto;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        public async Task<GenericResponse<ProductosRequest>> EliminarProducto(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ProductosRequest>();
                var productos = await _productoRepository.GetById(id, _logger);
                if (productos == null)
                {
                    response.Message = "El producto no existe";
                    return response;
                }

                productos.Activo = false;


                var update = await _productoRepository.Update(productos, _logger);
                if (update != null)
                {
                    response.Message = "Se elimino correctamente el producto";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se elimino correctamente el producto";
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
    }
}
