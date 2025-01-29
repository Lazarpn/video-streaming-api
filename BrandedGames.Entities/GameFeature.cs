using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

public class GameFeature
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconName { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }

    [InverseProperty(nameof(GameFormFeature.GameFeature))]
    public virtual ICollection<GameFormFeature> Features { get; set; } = new HashSet<GameFormFeature>();
}
