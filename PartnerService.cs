namespace Services.Implementations
{
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Models.DTO;
    using Models.DTO.UTMPartner;
    using Models.Contexts.Interfaces;
    using Models.Contexts.Implementations;
    using System.Dynamic;

    public class PartnerService : IPartnerService
    {
        public PartnerService(IPartnerContext context)
        {
            this.Db = context;
        }
        private IPartnerContext Db { get; }

        public async Task<int> CreatePartner(PartnerCreateDto partner)
        {
            return await this.Db.CreatePartner(partner);
        }

        public async Task<int> CreatePartnerPhone(int partnerId, PartnerCreateDto partner)
        {
            return await this.Db.CreatePartnerPhone(partnerId, partner);
        }

        public async Task<int> DeletePartner(int partnerId)
        {
            return await this.Db.DeletePartner(partnerId);
        }

        public async Task<IEnumerable<PartnersDto>> Find(int partnerId)
        {
            return await this.Db.GetPartner(partnerId);
        }

        public async Task<IEnumerable<PartnersDto>> GetAllPartners()
        {
            return await this.Db.GetAllPartners();
        }

        public async Task<IEnumerable<PartnerDetailsDto>> SearchPartner(PartnerSearchDto model)
        {
            dynamic dbModel = new ExpandoObject();
            if (!string.IsNullOrEmpty(model.PartnerPhone)) dbModel.phone= model.PartnerPhone;
            if (!string.IsNullOrEmpty(model.UtmPartner)) dbModel.partnerName = model.UtmPartner;
            if (!string.IsNullOrEmpty(model.UtmPartnerShortCode)) dbModel.code = model.UtmPartnerShortCode;
            if (model.PartnerPriority != -1) dbModel.priority = model.PartnerPriority;

            return await this.Db.SearchPartner(dbModel);
        }

        public async Task<int> UpdatePartner(PartnerUpdateDto partner)
        {
            return await this.Db.UpdatePartner(partner);
        }

        public async Task<int> UpdatePartnerPhone(PartnerUpdateDto partner)
        {
            return await this.Db.UpdatePartnerPhone(partner);
        }
    }
}
