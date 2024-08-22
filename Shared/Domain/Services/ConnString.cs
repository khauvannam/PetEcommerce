namespace Shared.Domain.Services;

public static class ConnString
{
    public static string SqlServer(string database, string server = "localhost,1433") =>
        $"Server={server};Initial Catalog={database};User ID=sa;Password=Nam09189921;TrustServerCertificate=True;Encrypt=False";
}
