namespace Common.Constants;

public static class ModelConstants
{
    public static class MaxLength
    {

        public const int Url = 2000;
        public const int GeoLocation = 100;
        public const int Comment = 800;
        public const int Description = 500;
        public const int ShortComment = 200;
        public const int Name = 100;
        public const int DisplayName = 100;
        public const int Email = 255;
        public const int Website = 100;
        public const int PhoneNumber = 15;
        public const int GUID = 50;
        public const int DescriptionWithHTML = 1500;
        public const int Last4 = 4;
        public const int Currency = 10;

        //General
        public const int ShortString = 50;
        public const int MediumString = 100;
        public const int LongString = 200;
        public const int Password = 128;
        public const int PostalCode = 15;
        public const int FileName = 255;
        public const int ContentType = 255;


    }
    public static class Regex
    {
        public const string Website = @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,20}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*).[a-z]{2,10}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
        public const string PhoneNumber = @"^[0-9]*$";
        public const string Name = @"^[a-zA-Z0-9]*[a-zA-Z0-9-_\\.\\&\\,\\-\\\s*]*[a-zA-Z0-9]+$";
        public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\\\!@#$%^&*()_+\\-=\\[\\]{}~;':\"\\|,.<>\\/?]).{6,}$";
    }

}