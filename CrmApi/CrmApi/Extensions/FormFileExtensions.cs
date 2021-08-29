using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace CrmApi.Extensions
{
    public static class FormFileExtensions
    {
        public static readonly List<string> ImageExtensions = new() {".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".SVG", ".JFIF"};

        public static string GetFileName(this IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
        }

        public static bool IsImage(this IFormFile file)
        {
            var fileName = GetFileName(file);
            return ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant());
        }

        public static bool ValidateImageFile(this IFormFile file, out string errorMessage)
        {
            var success = true;
            errorMessage = string.Empty;
            
            if (file.Length == 0)
            {
                errorMessage = "Invalid file provided.";
                success = false;
            }
            
            if (!file.IsImage())
            {
                errorMessage = "Invalid image provided. File must have one of the following extensions: "
                    + string.Join(',', ImageExtensions);
                success = false;
            }

            return success;
        }
    }
}
