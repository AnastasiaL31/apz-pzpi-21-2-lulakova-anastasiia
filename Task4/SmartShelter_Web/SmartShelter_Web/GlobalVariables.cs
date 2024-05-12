using Azure.Core;
using SmartShelter_Web.Middleware;

namespace SmartShelter_Web
{
    public class GlobalVariables
    {
        public static string backendAddress = "https://localhost:7251";

        public static string role = String.Empty;

        public static bool isAdmin
        {
            get
            {
                return GlobalVariables.role != String.Empty && GlobalVariables.role == "Admin";
                //return true;
            }
        }
    }
}
