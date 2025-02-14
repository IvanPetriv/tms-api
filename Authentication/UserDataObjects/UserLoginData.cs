namespace APIMain.Authentication.UserDataObjects {
    /// <summary>
    /// Maps an object containing user login data to a record
    /// </summary>
    /// <remarks>
    /// For example, the front-end sends a JSON object <c>{"login": "user123", "password": "qwerty"}</c>. To ensure type safety, the object is mapped to this record type
    /// </remarks>
    /// <param name="Login">User's login (username or e-mail)</param>
    /// <param name="Password">User's password</param>
    public record UserLoginData(string Login, string Password);
}
