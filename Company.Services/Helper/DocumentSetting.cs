using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Company.Services.Helper
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile File , string FolderName)
        {
            // 1. Get Folder Path
            //var FolderPath = @"C:\Users\20114\OneDrive\Desktop\alaa\.Net BackEnd\MVC\MVC3\Company.Web\wwwroot\Files\Images\";

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            // 2. Get File Name

            var FileName = $"{Guid.NewGuid}-{File.FileName}";

            //3. Combine FolderPath + FilePath

            var FilePath = Path.Combine(FolderPath, FileName);

            //4. Save File

            using var FileStream = new FileStream(FilePath, FileMode.Create);

            File.CopyTo(FileStream);

            return FileName;
        }
    }
}
