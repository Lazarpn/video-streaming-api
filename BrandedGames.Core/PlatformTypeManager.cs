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

public class PlatformTypeManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    public PlatformTypeManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<PlatformTypeModel>> GetPlatforms()
    {
        return await mapper.ProjectTo<PlatformTypeModel>(db.PlatformTypes).ToListAsync();
    }
}
