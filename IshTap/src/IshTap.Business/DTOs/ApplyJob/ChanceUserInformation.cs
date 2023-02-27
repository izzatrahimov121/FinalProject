using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.ApplyJob;

public class ChanceUserInformation
{
    [DataType(DataType.EmailAddress)]
    public string? NewEmail { get; set; }
    public string? NewFullName { get; set; }
    public string? NewUserName { get; set; }
}
