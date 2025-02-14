namespace APIMain.Configuration.Objects {
    /// <summary>
    /// Maps JWT settings from appsettings.json to this type
    /// </summary>
    /// <param name="Issuer">Who issues the token</param>
    /// <param name="Audience">Who accepts the token</param>
    /// <param name="Key">Secret string to sign the token</param>
    public record JwtSettings(string Issuer, string Audience, string Key);
}
