using BlobStorage.Api.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Threading.Tasks;

namespace BlobStorage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagemController : ControllerBase
    {
        private readonly ConfigMapBlob _configMap;

        public ImagemController(IConfiguration configuration)
        {
            var nomeConta = configuration["StorageConfiguration:Nome"];
            var chave = configuration["StorageConfiguration:Chave"];
            var containerBlob = configuration["StorageConfiguration:Container"];
            _configMap = new ConfigMapBlob(nomeConta, chave, containerBlob);
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(IFormFile file, [FromForm]Imagem imagemDto)
        {
            var imageUrl = await Upload(file);
            imagemDto.UrlImagem = imageUrl;
            return Ok(imagemDto);
        }

        private async Task<string> Upload(IFormFile file)
        {
            var storageCredentials = new StorageCredentials(_configMap.NomeConta, _configMap.Chave);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            var blobAzure = storageAccount.CreateCloudBlobClient();
            var container = blobAzure.GetContainerReference(_configMap.Container);

            var blob = container.GetBlockBlobReference(file.FileName);
            blob.Properties.ContentType = file.ContentType;
            await blob.UploadFromStreamAsync(file.OpenReadStream());

            return blob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();
        }
    }
}
