using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Common.Models;

public class PlatformTypeModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconName { get; set; }
    public string Description { get; set; }
}
