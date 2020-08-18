using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Helper.Converter
{
    public class Base64StringConverter
    {
        public static string getBase64String(string username, string password)
        {
            string auth = username + ":" + password;
            byte[] inarray = System.Text.UTF8Encoding.UTF8.GetBytes(auth);
            return System.Convert.ToBase64String(inarray);
        }
    }
}
