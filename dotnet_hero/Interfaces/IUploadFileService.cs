using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_hero.Interfaces
{
    public interface IUploadFileService
    {
        bool IsUpload(List<IFormFile> formFiles);
        string Validation(List<IFormFile> formFiles);
        Task<List<string>> UploadImages(List<IFormFile> formFiles);
    }
}
