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
        public static readonly List<string> ImageExtensions = new() {".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".SVG"};

        public static string GetFileName(this IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
        }

        public static bool IsImage(this IFormFile file)
        {
            var fileName = GetFileName(file);
            return ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant());
        }
    }
}
