using AutoMapper;
using VideoStreaming.Common.Models;
using VideoStreaming.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreaming.Core;

public class PlatformTypeManager
{
    private readonly VideoStreamingDbContext db;
    private readonly IMapper mapper;

    public PlatformTypeManager(VideoStreamingDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

}
