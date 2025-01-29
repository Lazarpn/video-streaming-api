using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Common.Models;

public class FeatureModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconName { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
}
