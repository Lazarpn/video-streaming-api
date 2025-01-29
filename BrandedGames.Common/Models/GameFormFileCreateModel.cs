using BrandedGames.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Common.Models;

public class GameFormFileCreateModel
{
    public IFormFile File { get; set; }
    public FileType FileType { get; set; }
}
