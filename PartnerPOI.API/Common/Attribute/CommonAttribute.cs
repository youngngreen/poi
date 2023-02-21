using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PartnerPOI.API.Common.Attribute
{

    public class CommonAttribute : ValidationAttribute
    {
        public bool Required { get; set; }

        public CommonAttribute()
        {
            Required = true;
        }
    }

    public class NumberLengthRangeAttribute : CommonAttribute
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public NumberLengthRangeAttribute()
        {
            Minimum = 0;
            Maximum = double.MaxValue;
        }
    }
    public class StringLengthRangeAttribute : CommonAttribute
    {
        public int StringLength { get; set; }

        public StringLengthRangeAttribute()
        {
            StringLength = 10;
        }
    }

    public class DateTimeRangeAttribute : CommonAttribute
    {
        public string Format { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }

        public DateTimeRangeAttribute()
        {
            Format = Format;
        }
    }

    public class ListRangeAttribute : CommonAttribute
    {
        public ListRangeAttribute()
        {

        }
    }

    public class BoolRangeAttribute : CommonAttribute
    {
        public BoolRangeAttribute()
        {

        }
    }
    public class BoolTextRangeAttribute : CommonAttribute
    {
        public string Description { get; set; }
        public string[] Descriptions
        {
            get
            {
                string[] parts = Description.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return parts;
            }
        }
        public BoolTextRangeAttribute()
        {

        }
    }
}
