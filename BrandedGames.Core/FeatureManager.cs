using AutoMapper;
using BrandedGames.Common.Models;
using BrandedGames.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Core;

public class FeatureManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    public FeatureManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<FeatureModel>> GetFeatures()
    {
        return await mapper.ProjectTo<FeatureModel>(db.GameFeatures).ToListAsync();
    }
}
