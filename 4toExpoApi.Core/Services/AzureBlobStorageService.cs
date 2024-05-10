using _4toExpoApi.Core.Enums;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly string azureStorageConnectionString;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            this.azureStorageConnectionString = configuration.GetValue<string>("AzureStorageConnectionString");
        }

        public async Task DeleteAsync(ContainerEnum container, string blobFilename)
        {
            var containerName = Enum.GetName(typeof(ContainerEnum), container).ToLower();
            var blobContainerClient = new BlobContainerClient(this.azureStorageConnectionString, containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobFilename);

            try
            {
                await blobClient.DeleteAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<string> UploadAsync(IFormFile file, ContainerEnum container, string blobName = null)
        {
            if (file.Length == 0) return null;

            var containerName = Enum.GetName(typeof(ContainerEnum), container).ToLower();

            var blobContainerClient = new BlobContainerClient(this.azureStorageConnectionString, containerName);

            // Get a reference to the blob just uploaded from the API in a container from configuration settings
            if (string.IsNullOrEmpty(blobName))
            {
                blobName = Guid.NewGuid().ToString();
            }

            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

            // Open a stream for the file we want to upload
            await using (Stream stream = file.OpenReadStream())
            {
                // Upload the file async
                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }

            return blobName;
        }
    }
    public interface IAzureBlobStorageService
    {
        // El memtodo carga un archivo enviado en el request
        // "file" es el archivo a subir
        // nombre de la imagen yo se lo pongo 
        // edoxalm, carpeta = "directorio", 
        // "blobName" nombre del Blb a subir
        // Retorna el nombre del archivo guardado en Blob Container
        Task<String> UploadAsync(IFormFile file, ContainerEnum container, string blobName = null);

        // Elimina un archivo con un nombre especifico
        Task DeleteAsync(ContainerEnum container, string blobFileName);

    }
}
