using System.ComponentModel.DataAnnotations;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;

public class TrustContact : BaseEntity
{
    public int Id { get; set; }
    public required int Uid { get; set; }
    public required ContactRole Role { get; set; }
    [MaxLength(500)] public required string Name { get; set; }
    [MaxLength(320)] public required string Email { get; set; }
}
