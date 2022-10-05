namespace Models.Contexts.Interfaces
{
    using Models.DTO.UTMPartner;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPartnerContext
    {
        Task<IEnumerable<PartnersDto>> GetPartner(int partnerId);
        Task<IEnumerable<PartnersDto>> GetAllPartners();
        Task<IEnumerable<PartnerDetailsDto>> SearchPartner(object partner);
        Task<int> CreatePartner(PartnerCreateDto partner);
        Task<int> UpdatePartner(PartnerUpdateDto partner);
        Task<int> DeletePartner(int partnerId);
        Task<int> CreatePartnerPhone(int partnerId, PartnerCreateDto partner);
        Task<int> UpdatePartnerPhone(PartnerUpdateDto partner);

    }
}
