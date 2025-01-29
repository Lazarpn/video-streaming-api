using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BrandedGames.Common.Enums;

namespace BrandedGames.Entities;

public class GameFormFile
{
    public Guid Id { get; set; }
    public Guid? GameFormId { get; set; }

    [MaxLength(50)]
    public string FileName { get; set; }

    [MaxLength(1000)]
    public string FileOriginalName { get; set; }

    [MaxLength(1000)]
    public string FileUrl { get; set; }

    [MaxLength(1000)]
    public string ThumbUrl { get; set; }

    public FileType FileType { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    [ForeignKey(nameof(GameFormId))]
    [InverseProperty(nameof(Entities.GameForm.Files))]
    public virtual GameForm GameForm { get; set; }
}
