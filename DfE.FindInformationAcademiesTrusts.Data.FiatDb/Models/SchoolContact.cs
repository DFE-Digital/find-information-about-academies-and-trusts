using System.ComponentModel.DataAnnotations;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;

public class SchoolContact : BaseEntity
{
    public int Id { get; set; }
    public required int Urn { get; set; }
    public required SchoolContactRole Role { get; set; }
    [MaxLength(500)] public required string Name { get; set; }
    [MaxLength(320)] public required string Email { get; set; }
}
