using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BrandedGames.Common.Enums;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace BrandedGames.Core;

public class CloudinaryFileManager
{
    private readonly Cloudinary cloudinary;

    public CloudinaryFileManager(IConfiguration configuration)
    {
        cloudinary = new Cloudinary(configuration["cloudinaryApiKey"]);
        cloudinary.Api.Secure = true;
    }

    public async Task<ImageUploadResult> ProcessFileStorageUpload(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };

        return await cloudinary.UploadAsync(uploadParams);
    }
}
