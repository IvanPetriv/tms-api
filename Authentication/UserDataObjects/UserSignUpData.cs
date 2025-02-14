namespace APIMain.Authentication.UserDataObjects {
    /// <summary>
    /// Maps an object containing user signup data to a record
    /// </summary>
    /// <remarks>
    /// For example, the front-end sends a JSON object <c>{"username": "user123", "firstName": "John", "middleName": null, ...}</c>. To ensure type safety, the object is mapped to this record type
    /// </remarks>
    public record UserSignUpData(string Username,
                                   string? FirstName,
                                   string? MiddleName,
                                   string? LastName,
                                   string? Email,
                                   byte[]? ProfilePicture);
}
