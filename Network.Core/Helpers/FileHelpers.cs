using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Helpers
{
    public static class FileHelpers
    {
        public static bool isImage(string path)
        {
            var ext = Path.GetExtension(path);
            switch (ext.ToLower())
            {
                case "jpg":
                case "gif":
                case "bmp":
                case "png":
                    return true;
            }
            return false;
        }
        public static async Task SaveImage(string savePath, string base64)
        {
            if(string.IsNullOrEmpty(savePath))            
                throw new Exception("SavePath is empty !");            
            if (string.IsNullOrEmpty(base64))            
                throw new Exception("Base64 is empty !");                        
            using(var stream = new FileStream(savePath, FileMode.Create))
            {
                var imgByte = Convert.FromBase64String(base64);
                await stream.WriteAsync(imgByte, 0, imgByte.Length);
            }    
        }    
        public static string GetFileSize(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            long bytes = fi.Length;
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
    
        private static string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
    }
}
