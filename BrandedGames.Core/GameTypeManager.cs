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

public class GameTypeManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    public GameTypeManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<GameTypeModel>> GetTypes()
    {
        return await mapper.ProjectTo<GameTypeModel>(db.GameTypes).ToListAsync();
    }
}
