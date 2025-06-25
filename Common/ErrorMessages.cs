namespace Common
{

    public static class ErrorMessages
    {
        private static List<string> Vowels = new List<string> { "a", "A", "e", "E", "i", "I", "o", "O", "u", "U" };
        public static string AlreadyExists(string objectName, string propName) => $"{AddStartLetter(objectName.ToLower())} already exists with the same  {propName.ToLower()} !";
        public static string Alreadyreferenced(string objectName, string propName) => $"{AddStartLetter(objectName.ToLower())} already referenced in the {propName.ToLower()}!";
        public static string Alreadyreferenced(string objectName) => $"{AddStartLetter(objectName.ToLower())} already referenced!";

        public static string InvalidOperation(string propName) => $"Invalid {propName.ToLower()}!";
        public static string SomethingWentWrong(string propName) => $"Something went wrong with the {propName.ToLower()}!";
        public static string DuplicateValues(string propName) => $"The {propName.ToLower()} must be unique!";

        public static string NotFound(string objectName) => $"{objectName} is not found";
        public static string NotExist(string objectName) => $"{objectName} does not exists";
        private static string AddStartLetter(string objectName) => Vowels.Contains(objectName.ToLower()[0].ToString()) ? $"An {objectName.ToLower()}" : $"A {objectName.ToLower()}";
        public static string AtleastOneRequired(string objectName) => $" At least one {objectName} is required!";
        public static string MustHaveUniqueName() => $"Must have a unique name!";
        public static string ExceededUsersLimit() => $"Company size limit exceeded!";
        public static string MustHaveUniqueIP() => $"Must have a unique IP!";
        public static string MustHaveUniqueKey() => $"Must have a unique Key!";
        public static string MustHaveUniqueAddress() => $"Must have a unique address!";

        public static string BadRequest(string msg) => msg;

    }
}
