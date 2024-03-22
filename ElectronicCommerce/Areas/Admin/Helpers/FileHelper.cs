using System;
namespace ElectronicCommerce.Areas.Admin.Helpers
{
    public class FileHelper
    {
        public static string GenerateFileName(string contentType)
        {
            // Xu ly viec trung file bang cach dat ten file lai bang guid va noi' duoi file vao
            var guid = Guid.NewGuid().ToString().Replace("-","");
            var extension = contentType.Split(new char[] { '/' })[1];
            return guid+"."+extension;

        }
    }
}
