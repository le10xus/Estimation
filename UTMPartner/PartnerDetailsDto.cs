namespace Models.DTO.UTMPartner
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json.Serialization;

    public class PartnerDetailsDto
    {
        [JsonPropertyName("UtmPartnerPriorityID")]
        public int UtmPartnerPriorityID { get; set; }
        [JsonPropertyName("UtmPartner")]
        public string UtmPartner { get; set; }
        [JsonPropertyName("UtmPartnerShortCode")]
        public string UtmPartnerShortCode { get; set; }
        [JsonPropertyName("PartnerPriority")]
        public int PartnerPriority { get; set; }
        [JsonPropertyName("PartnerPhone")]
        public Dictionary<int,string> PartnerPhone { get; set; }
        [JsonPropertyName("PartnerName")]
        public string PartnerName { get; set; }
        [JsonPropertyName("PartnerSource")]
        public string UTMSource { get; set; }
        [JsonPropertyName("PartnerCampaign")]
        public string UTMCampaign { get; set; }
        [JsonPropertyName("PartnerPhoneId")]
        public int PartnerPhoneID { get; set; }
    }
}
