namespace APIMain.Messages {
    public static class LogMessage {
        /// <summary>
        /// Message when no entry with specified ID was found.
        /// </summary>
        /// <remarks>
        /// Expects the current <paramref name="controller"/> name, <paramref name="object"/> type, and searched <paramref name="id"/> passed through format methods.
        /// </remarks>
        public const string NotFoundById = "{controller}: {object_type} with ID {id} was not found.";

        /// <summary>
        /// Message when an entry with specified ID already exists.
        /// </summary>
        /// <remarks>
        /// Expects the current <paramref name="controller"/> name, <paramref name="object"/> type, and searched <paramref name="id"/> passed through format methods.
        /// </remarks>
        public const string AlreadyExistsWithId = "{controller}: {object_type} with ID {id} already exists.";

        /// <summary>
        /// Message when an entry tries to obtain an unauthorized access.
        /// </summary>
        /// <remarks>
        /// Expects the current <paramref name="controller"/> name passed through format methods.
        /// </remarks>
        public const string UnauthorizedAccess = "{controller}: JWT token does not allow accessing this resource";
    }
}
