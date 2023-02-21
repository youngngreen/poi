using PartnerPOI.API.Common.Attribute;

namespace PartnerPOI.API.DTOs
{
    public class InternalPointDeleteRequest
    {
        [StringLengthRange(StringLength = 50)]
        public string? RequestUserID { get; set; }

        /* (Key) */
        [StringLengthRange(StringLength = 20)]
        public string? InternalPointID { get; set; }

        /* (Key) */
        [StringLengthRange(StringLength = 2)]
        public string? CustomerLevel { get; set; }

    }

}
