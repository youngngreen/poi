using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PartnerPOI.API.Models
{
    [Table("TbMInternalPointD")]
    public class TbMInternalPointD
    {
        [Key]
        /* id relate to header */
        [Column("internalPointSettingID", TypeName = "int")]
        public int InternalPointSettingID { get; set; }

        [Key]
        /* index of event  */
        [Column("seqNo", TypeName = "int")]
        public int SeqNo { get; set; }

        /* Amount of Money per Point (Ex. 20 bath get 1 point) */
        [Column("unitPerPoint", TypeName = "int")]
        public int UnitPerPoint { get; set; }

        /* Multiplier (Ex. X5  for some period) */
        [Column("multiplier", TypeName = "int")]
        public int Multiplier { get; set; }

        /* Effective From  */
        [Column("effectiveFrom", TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }

        /* Effective To */
        [Column("effectiveTo", TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }

        /* Pattern of Expire Date.  */
        [Column("expireDatePattern", TypeName = "nvarchar(5)")]
        public string? ExpireDatePattern { get; set; }

        [Column("createdDate", TypeName = "timestamp")]
        public DateTime? CreatedDate { get; set; }

        [Column("createdBy", TypeName = "nvarchar(20)")]
        public string? CreatedBy { get; set; }

        [Column("updatedDate", TypeName = "timestamp")]
        public DateTime? UpdatedDate { get; set; }

        [Column("updatedBy", TypeName = "nvarchar(20)")]
        public string? UpdatedBy { get; set; }

    }


}
