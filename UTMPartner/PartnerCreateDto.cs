namespace Models.DTO.UTMPartner
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PartnerCreateDto
    {
        public string UtmPartner { get; set; }
        public string UtmPartnerShortCode { get; set; }
        public int PartnerPriority { get; set; }
        public string UserId { get; set; }
        public List<string> PartnerPhones { get; set; }
        public string Source { get; set; }
        public string Campaign { get; set; }
    }
}
