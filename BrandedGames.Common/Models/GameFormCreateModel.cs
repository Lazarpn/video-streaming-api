using BrandedGames.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Common.Models;

public class GameFormCreateModel
{
    public Guid GameTypeId { get; set; }

    public CustomerType CustomerType { get; set; }

    public int? Price { get; set; }
    public string Items { get; set; }
    public bool RewardTopPlayers { get; set; }
    public string Rewards { get; set; }
    public string RewardPlacements { get; set; }

    public List<Guid> FeatureIds { get; set; } = new();
    public List<Guid> PlatformTypeIds { get; set; } = new();
    public List<IFormFile> Files { get; set; } = [];
}
