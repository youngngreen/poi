namespace PartnerPOI.API.Models;
public class InternalPoint
{
    public int internalPointSettingID { get; set; }
    public string? serviceIdentifiedByPartner { get; set; }
    public string? internalPointID { get; set; }
    public string? internalPointDesc { get; set; }
    public string? customerLevel { get; set; }
    public bool isDeleted { get; set; }

    public DateTime createdDate { get; set;}
    public string? createdBy { get; set; }
    public DateTime updatedDate { get; set;}
    public string? updatedBy { get; set; }

}
