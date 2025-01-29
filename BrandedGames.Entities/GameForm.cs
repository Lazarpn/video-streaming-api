using BrandedGames.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

public class GameForm
{
    public Guid Id { get; set; }
    public Guid GameTypeId { get; set; }

    public CustomerType CustomerType { get; set; }

    public int? Price { get; set; }
    public string Items { get; set; }
    public bool RewardTopPlayers { get; set; }
    public string Rewards { get; set; }
    public string RewardPlacements { get; set; }

    [ForeignKey(nameof(GameTypeId))]
    [InverseProperty(nameof(Entities.GameType.GameForms))]
    public virtual GameType GameType { get; set; }

    [InverseProperty(nameof(GameFormFeature.GameForm))]
    public virtual ICollection<GameFormFeature> Features { get; set; } = new HashSet<GameFormFeature>();

    [InverseProperty(nameof(GameFormFile.GameForm))]
    public virtual ICollection<GameFormFile> Files { get; set; } = new HashSet<GameFormFile>();

    [InverseProperty(nameof(GameFormPlatformType.GameForm))]
    public virtual ICollection<GameFormPlatformType> GameFormPlatformTypes { get; set; } = new HashSet<GameFormPlatformType>();
}
