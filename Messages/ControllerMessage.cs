namespace APIMain.Messages {
    public static class ControllerMessage {
        /// <summary>
        /// Message when no entry with specified ID was found.
        /// </summary>
        /// <remarks>
        /// Expects the current <paramref name="controller"/> name, <paramref name="object"/> type, and searched <paramref name="id"/> passed through format methods.
        /// </remarks>
        public const string NotFoundById = "{0}: {1} with ID {2} was not found.";

        /// <summary>
        /// Message when an entry with specified ID already exists.
        /// </summary>
        /// <remarks>
        /// Expects the current <paramref name="controller"/> name, <paramref name="object"/> type, and searched <paramref name="id"/> passed through format methods.
        /// </remarks>
        public const string AlreadyExistsWithId = "{0}: {1} with ID {2} already exists.";

        /// <summary>
        /// Message when an entry tries to obtain an unauthorized access.
        /// </summary>
        /// <remarks>
        /// Expects the current <paramref name="controller"/> name passed through format methods.
        /// </remarks>
        public const string UnauthorizedAccess = "{0}: JWT token does not allow to access this resource";
    }
}
