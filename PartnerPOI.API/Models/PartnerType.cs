//using Microsoft.EntityFrameworkCore;

namespace PartnerPOI.API.Models;

public class PartnerType
{ 
    public string? ServiceIdentifiedByPartner { get; set; }
    public int Id { get; set; }
    public string? PartnerTypeNameEN { get; set; } 
    public string? PartnerTypeNameTH { get; set; }
    public string? DisplayOrder { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
}