namespace BlazorApp1.Constants
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string BusinessManager = "BusinessManager";

        public static readonly string[] All = [Admin, Customer, BusinessManager];
    }

}
