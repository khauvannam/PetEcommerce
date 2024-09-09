namespace Shared.Domain.Services;

public static class ConnString
{
    public const string BlobConnectionString =
        "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://172.20.0.2:10000/devstoreaccount1";

    public static string SqlServer(
        string database = "StoreDatabase",
        string server = "localhost,1433"
    )
    {
        return $"Server={server};Initial Catalog={database};User ID=sa;Password=Nam09189921;TrustServerCertificate=True;Encrypt=False";
    }
}
