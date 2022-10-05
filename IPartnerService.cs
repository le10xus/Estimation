namespace Services.Interfaces
{
    using Models.DTO.UTMPartner;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPartnerService
    {
        Task<IEnumerable<PartnersDto>> Find(int partnerId);
        Task<IEnumerable<PartnersDto>> GetAllPartners();
        Task<IEnumerable<PartnerDetailsDto>> SearchPartner(PartnerSearchDto model);
        Task<int> CreatePartner(PartnerCreateDto partner);
        Task<int> UpdatePartner(PartnerUpdateDto partner);
        Task<int> UpdatePartnerPhone(PartnerUpdateDto partner);
        Task<int> DeletePartner(int partnerId);
        Task<int> CreatePartnerPhone(int partnerId, PartnerCreateDto partner);
    }
}
