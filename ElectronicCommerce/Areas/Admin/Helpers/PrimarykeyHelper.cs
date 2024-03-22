using System;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Helpers
{
    public class OrderCode
    {
        public OrderCode()
        {
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "123456";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
