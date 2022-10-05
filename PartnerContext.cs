namespace Models.Contexts.Implementations
{
    using Dapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models.Configuration;
    using Models.Contexts.Interfaces;
    using Models.DTO.UTMPartner;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Linq;
    using System;
    using Models.Enums;

    public class PartnerContext : FlexApiContext, IPartnerContext
    {
        public PartnerContext(IOptions<DbConfig> config, ILogger<PartnerContext> logger) : base(config)
        {
            this.Logger = logger;
        }

        private ILogger<PartnerContext> Logger { get; }

        public async Task<int> CreatePartner(PartnerCreateDto partner)
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                var partnerResult = await connection.QueryAsync<string>(
                    "lms.usp_insert_partner",
                        new {
                            partnerName = partner.UtmPartner,
                            code = partner.UtmPartnerShortCode,
                            priority = partner.PartnerPriority,
                            createId = partner.UserId
                        },
                        commandType: CommandType.StoredProcedure);
                
                if (partnerResult == null || !partnerResult.Any())
                {
                    return -1;
                }
                var partnerId = JsonConvert.DeserializeObject<int>(string.Concat(partnerResult));
                await AddPhoneRequest(partnerId, partner, connection);

                return partnerId;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to create partner.");
                return -1;
            }
        }

        public async Task<int> CreatePartnerPhone(int partnerId, PartnerCreateDto partner)
        {
            using var connection = new SqlConnection(this.Config.AptiveCRM);
            return await AddPhoneRequest(partnerId, partner, connection);
        }

        public async Task<int> DeletePartner(int partnerId)
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                var partnerResult = await connection.QueryAsync<string>(
                    "lms.usp_delete_partner",
                        new
                        {
                            partnerId
                        },
                        commandType: CommandType.StoredProcedure);

                if (partnerResult == null || !partnerResult.Any())
                {
                    return -1;
                }

                return 1;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to create partner.");
                return -1;
            }
        }

        public async Task<IEnumerable<PartnersDto>> GetAllPartners()
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                var result = await connection.QueryAsync<string>(
                    "lms.usp_get_partners",
                        null,
                        commandType: CommandType.StoredProcedure);

                if (result == null || !result.Any())
                {
                    return null;
                }

                // map to DB model
                return JsonConvert.DeserializeObject<IEnumerable<PartnersDto>>(string.Concat(result));
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to fetch partners.");
                return null;
            }
        }

        public async Task<IEnumerable<PartnersDto>> GetPartner(int partnerId)
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                var result = await connection.QueryAsync<string>(
                    "lms.usp_get_partner_priority",
                        new { PartnerId = partnerId },
                        commandType: CommandType.StoredProcedure);

                if (result == null || !result.Any())
                {
                    return null;
                }

                // map to DB model
                return JsonConvert.DeserializeObject<IEnumerable<PartnersDto>>(string.Concat(result));
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to fetch partner. Id: {partnerId}.");
                return null;
            }
        }

        public async Task<IEnumerable<PartnerDetailsDto>> SearchPartner(object partner)
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                var result = await connection.QueryAsync<string>(
                    "lms.usp_get_partner_details",
                        partner,
                        commandType: CommandType.StoredProcedure);

                if (result == null || !result.Any())
                {
                    return null;
                }

                // map to DB model
                var res = JsonConvert.DeserializeObject<IEnumerable<PartnersDto>>(string.Concat(result))
                    .GroupBy(x => x.UtmPartnerPriorityID)
                    .Select(g => new PartnerDetailsDto()
                    {
                        UtmPartnerPriorityID = g.Key,
                        UtmPartner = g.Select(x=>x.UtmPartner).FirstOrDefault(),
                        PartnerPhone = g.ToDictionary(d => d.PartnerPhoneID, d => d.PartnerPhone ),
                        UtmPartnerShortCode = g.Select(x => x.UtmPartnerShortCode).FirstOrDefault(),
                        PartnerPriority = g.Select(x => x.PartnerPriority).FirstOrDefault(),
                        PartnerName = g.Select(x => x.PartnerName).FirstOrDefault(),
                        UTMCampaign = g.Select(x => x.UTMCampaign).FirstOrDefault(),
                        UTMSource = g.Select(x => x.UTMSource).FirstOrDefault()
                    });
                return res;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to fetch partner.");
                return null;
            }
        }

        public async Task<int> UpdatePartnerPhone(PartnerUpdateDto partner)
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                var phones = partner.PartnerPhone == null ? new Dictionary<int, string>() : partner.PartnerPhone;
                //get phones to create
                var newPhones = phones.Where(k => k.Key == -1).Select(v=>v.Value);
                if (newPhones.Any())
                {
                    var createDto = new PartnerCreateDto()
                    {
                        UserId = partner.UserId,
                        PartnerPhones = newPhones.ToList(),
                        Campaign = partner.Campaign,
                        UtmPartnerShortCode = partner.UtmPartnerShortCode,
                        Source = partner.Source,
                        PartnerPriority = partner.PartnerPriority,
                        UtmPartner = partner.UtmPartner
                    };
                    await AddPhoneRequest(partner.PartnerId, createDto, connection);
                }
                //get phones to update
                var updatePhones = phones.Where(k => k.Key != -1);
                foreach (var phone in updatePhones)
                {
                    await UpdatePartnerPhoneRequest(partner, phone, connection);
                }

                return 1;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to fetch partner.");
                return -1;
            }
        }

        public async Task<int> UpdatePartner(PartnerUpdateDto partner)
        {
            try
            {
                using var connection = new SqlConnection(this.Config.AptiveCRM);
                await UpdatePartnerRequest(partner, connection);
                var phones = partner.PartnerPhone == null ? new Dictionary<int, string>() : partner.PartnerPhone;
                
                foreach (var phone in phones)
                {
                    await UpdatePartnerPhoneRequest(partner, phone, connection);
                }

                return 1;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to fetch partner.");
                return -1;
            }
        }

        private async Task<int> UpdatePartnerRequest(PartnerUpdateDto partner, SqlConnection connection)
        {
                var result = await connection.QueryAsync<string>(
                    "lms.usp_update_partner",
                        new {
                            partnerId = partner.PartnerId,
                            partnerName = partner.UtmPartner,
                            code = partner.UtmPartnerShortCode,
                            priority = partner.PartnerPriority
                        },
                        commandType: CommandType.StoredProcedure);

                if (result == null || !result.Any())
                {
                    return -1;
                }
                return JsonConvert.DeserializeObject<int>(string.Concat(result));
        }

        private async Task<int> UpdatePartnerPhoneRequest(PartnerUpdateDto partner, KeyValuePair<int,string> phone, SqlConnection connection)
        {
            var result = await connection.QueryAsync<string>(
                "lms.usp_update_partner_phone",
                    new
                    {
                        partnerPhoneId = phone.Key,
                        partnerPhone = phone.Value,
                        partnerName = partner.UtmPartner,
                        source = partner.Source,
                        campaign = partner.Campaign,
                        createId = partner.UserId
                    },
                    commandType: CommandType.StoredProcedure);

            if (result == null || !result.Any())
            {
                return -1;
            }
            return JsonConvert.DeserializeObject<int>(string.Concat(result));
        }

        private async Task<int> AddPhoneRequest(int partnerId, PartnerCreateDto partner, SqlConnection connection)
        {
            try
            {
                var result = await connection.QueryAsync<string>(
                    "lms.usp_insert_partner_phone",
                        new
                        {
                            partnerName = partner.UtmPartner,
                            partnerPhone = string.Join(",", partner.PartnerPhones.ToArray()),
                            source = partner.Source,
                            campaign = partner.Campaign,
                            createId = partner.UserId,
                            friendlyName = partner.Source + "-" + partner.UtmPartner + "_" + partner.Campaign,
                            partnerId
                        },
                        commandType: CommandType.StoredProcedure);

                return JsonConvert.DeserializeObject<int>(string.Concat(result));
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to create partner.");
                return -1;
            }
        }
    }
}
