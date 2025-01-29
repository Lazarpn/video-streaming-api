using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

public class GameFormPlatformType
{
    public Guid GameFormId { get; set; }
    public Guid PlatformTypeId { get; set; }

    [ForeignKey(nameof(GameFormId))]
    [InverseProperty(nameof(Entities.GameForm.GameFormPlatformTypes))]
    public virtual GameForm GameForm { get; set; }

    [ForeignKey(nameof(PlatformTypeId))]
    [InverseProperty(nameof(Entities.PlatformType.GameFormPlatformTypes))]
    public virtual PlatformType PlatformType { get; set; }
}
