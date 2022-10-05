namespace Models.DTO.UTMPartner
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PartnerSearchDto
    {
        public string UtmPartner { get; set; }
        public string UtmPartnerShortCode { get; set; }
        public int PartnerPriority { get; set; }
        public string PartnerPhone { get; set; }
    }
}
