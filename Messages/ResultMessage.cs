namespace APIMain.Messages {
    public static class ResultMessage {
        /// <summary>
        /// Message when no entry with specified ID was found.
        /// </summary>
        /// <param name="object_type">Table object type</param>
        /// <param name="id">ID of the object</param>
        /// <returns>Formatted string containing <paramref name="object_type"/> and <paramref name="id"/></returns>
        public static string NotFoundById(string object_type, long id) =>
            string.Format("{0} with ID {1} was not found.", object_type, id);

        /// <summary>
        /// Message when an entry with specified ID already exists.
        /// </summary>
        /// <param name="object_type">Table object type</param>
        /// <param name="id">ID of the object</param>
        /// <returns>Formatted string containing <paramref name="object_type"/> and <paramref name="id"/></returns>
        public static string AlreadyExistsWithId(string object_type, long id) =>
            string.Format("{0} with ID {1} already exists.", object_type, id);

        /// <summary>
        /// Message when an entry tries to obtain an unauthorized access.
        /// </summary>
        public static string UnauthorizedAccess() =>
            string.Format("You don't have access to this resource");
    }
}
