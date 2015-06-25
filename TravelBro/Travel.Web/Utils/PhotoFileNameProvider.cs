using System;
using System.IO;
using System.Web;

namespace WebApplication1.Utils
{
    public class PhotoFileNameProvider
    {
        private readonly object _lock = new object();

        private static string _location;
        public static string FileSaveLocation
        {
            get { return _location ?? (_location = HttpContext.Current.Server.MapPath("~/App_Data/Images") + '\\'); }
        }

        public string GetNewFileName(string extension)
        {
            lock (_lock)
            {
                string fileName = Guid.NewGuid().ToString("N");
                int counter = 0;
                while (File.Exists(FileSaveLocation + "\\" + fileName + counter + extension))
                    counter++;
                return fileName + counter + extension;
            }
        }
    }
}