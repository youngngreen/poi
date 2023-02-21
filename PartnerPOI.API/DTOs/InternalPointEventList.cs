using PartnerPOI.API.Common.Attribute;

namespace PartnerPOI.API.DTOs
{
    public class InternalPointEventList
    {
        /* ex, 100 baht per 1 point */
        [NumberLengthRange()]
        public decimal? UnitPerPoint { get; set; }

        /* Pattern of Expire. (Ex. Next 3 Month, Next 6 Month) */
        [StringLengthRange(StringLength = 5)]
        public string? ExpireDatePattern { get; set; }

        /* ex. x2 in special holiday,  */
        [NumberLengthRange()]
        public decimal? Multiplier { get; set; }

        /* yyyy-MM-dd */
        [DateTimeRangeAttribute(Format = "yyyy-MM-dd")]
        public string? EffectiveFrom { get; set; }

        /* yyyy-MM-dd */
        [DateTimeRangeAttribute(Format = "yyyy-MM-dd")]
        public string? EffectiveTo { get; set; }

    }
}
