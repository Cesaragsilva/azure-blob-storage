namespace BlobStorage.Api.Dto
{
    public class ConfigMapBlob
    {
        public ConfigMapBlob(string nomeConta, string chave, string container)
        {
            NomeConta = nomeConta;
            Chave = chave;
            Container = container;
        }
        public string NomeConta { get; set; }
        public string Chave { get; set; }
        public string Container { get; set; }
    }
}