namespace Models.DTO.UTMPartner
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json.Serialization;

    public class PartnerPriorityDto
    {
        [JsonPropertyName("UtmPartnerPriorityID")]
        public int PartnerId { get; set; }
        [JsonPropertyName("UtmPartner")]
        public string PartnerName { get; set; }
        [JsonPropertyName("UtmPartnerShortCode")]
        public string ShortCode { get; set; }
        [JsonPropertyName("PartnerPriority")]
        public int Priority { get; set; }
        [JsonPropertyName("CreateDtm")]
        public DateTime CreateDate { get; set; }
        [JsonPropertyName("CreateBID")]
        public string CreateId { get; set; }
    }
}
