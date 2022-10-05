namespace Models.DTO.UTMPartner
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PartnerUpdateDto
    {
        public int PartnerId { get; set; }
        public string UtmPartner { get; set; }
        public string UtmPartnerShortCode { get; set; }
        public int PartnerPriority { get; set; }
        public string FriendlyName { get; set; }
        public string UserId { get; set; }
        public Dictionary<int, string> PartnerPhone { get; set; }
        public string Source { get; set; }
        public string Campaign { get; set; }
    }
}
