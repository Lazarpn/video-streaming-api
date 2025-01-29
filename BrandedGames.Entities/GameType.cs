using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

public class GameType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconName { get; set; }

    [InverseProperty(nameof(GameForm.GameType))]
    public virtual ICollection<GameForm> GameForms { get; set; } = new HashSet<GameForm>();
}
