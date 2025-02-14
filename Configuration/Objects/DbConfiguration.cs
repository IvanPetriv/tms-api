namespace APIMain.Configuration.Objects {
    /// <summary>
    /// Maps database configuration from appsettings.json to this type
    /// </summary>
    /// <param name="ConnectionString">Address which is used to connect to the DB</param>
    public record DbConfiguration(string ConnectionString);
}
