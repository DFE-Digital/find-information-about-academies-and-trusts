using System.ComponentModel.DataAnnotations;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;

public class BaseEntity
{
    [MaxLength(500)]
    public string LastModifiedByName { get; set; } = null!;
    [MaxLength(320)]
    public string LastModifiedByEmail { get; set; } = null!;
    public DateTime LastModifiedAtTime { get; set; }
}
