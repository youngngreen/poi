using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PartnerPOI.API.Models;

[Table("TbMInternalPointH")]
public class TbMInternalPointH
{
    [Key]
    /* running no */
    //[Column("internalPointSettingID", TypeName = "nvarchar(50)")]
    [Column("internalPointSettingID", TypeName = "int")]
    public int InternalPointSettingID { get; set; }

    /* [Owner] - Data Owner                 ex. Toyota, Daihatsu [User Type] - Business Role                ex. User, Provider [Line of Business]  - Business Type               ex. Roadside, Insurance,                    Deivery, Parking Lot [Partner] - Partner(Specific)               ex. Daihatsu, (Coffee company),                    (TowTruckCOmpany),                   Generic */
    [Column("serviceIdentifiedByPartner", TypeName = "nvarchar(50)")]
    public string ServiceIdentifiedByPartner { get; set; }

    /* Internal Point ID */
    [Column("internalPointID", TypeName = "nvarchar(10)")]
    public string InternalPointID { get; set; }

    /* Internal Point Desc */
    [Column("internalPointDesc", TypeName = "nvarchar(200)")]
    public string InternalPointDesc { get; set; }

    /* Customer Level ID (W=White, S=Silver, G=Gold, P=Platinum) */
    [Column("customerLevel", TypeName = "nvarchar(2)")]
    public string CustomerLevel { get; set; }

    /* (0 = Normal status, 1 = Deleted) */
    [Column("isDeleted", TypeName = "tinyint(1)")]
    public bool IsDeleted { get; set; }

    [Column("createdDate", TypeName = "timestamp")]
    public DateTime CreatedDate { get; set; }

    [Column("createdBy", TypeName = "nvarchar(20)")]
    public string CreatedBy { get; set; }

    [Column("updatedDate", TypeName = "timestamp")]
    public DateTime UpdatedDate { get; set; }

    [Column("updatedBy", TypeName = "nvarchar(20)")]
    public string UpdatedBy { get; set; }

}