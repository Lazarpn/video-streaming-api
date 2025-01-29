using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

public class PlatformType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconName { get; set; }
    public string Description { get; set; }

    [InverseProperty(nameof(GameFormPlatformType.PlatformType))]
    public virtual ICollection<GameFormPlatformType> GameFormPlatformTypes { get; set; } = new HashSet<GameFormPlatformType>();
}
