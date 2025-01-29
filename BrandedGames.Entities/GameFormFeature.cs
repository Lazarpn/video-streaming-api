using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

public class GameFormFeature
{
    public Guid GameFeatureId { get; set; }
    public Guid GameFormId { get; set; }

    [ForeignKey(nameof(GameFeatureId))]
    [InverseProperty(nameof(Entities.GameFeature.Features))]
    public virtual GameFeature GameFeature { get; set; }

    [ForeignKey(nameof(GameFormId))]
    [InverseProperty(nameof(Entities.GameForm.Features))]
    public virtual GameForm GameForm { get; set; }
}
