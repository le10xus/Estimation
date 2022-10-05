namespace Api.Controllers.v1
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.DTO.UTMPartner;

    [ApiVersion("1.0")]
    public class PartnerController : FlexApiController
    {
        public PartnerController(IPartnerService service)
        {
            this.Service = service;
        }

        private IPartnerService Service { get; }

        /// <summary>
        /// Fetches all partners
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPartners()
        {
            return this.Ok(await this.Service.GetAllPartners());
        }

        /// <summary>
        /// Fetches a partner details by Id
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpGet("{partnerId:int}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPartner(int partnerId)
        {
            return this.Ok(await this.Service.Find(partnerId));
        }

        /// <summary>
        /// Searches a partner by the specified criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SearchPartnerAsync([FromBody] PartnerSearchDto model)
        {
            return this.Ok(await this.Service.SearchPartner(model));
        }

        /// <summary>
        /// Creates a partner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/createPartner")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePartnerAsync([FromBody] PartnerCreateDto model)
        {
            return this.Ok(await this.Service.CreatePartner(model));
        }

        /// <summary>
        /// Creates a partners phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/createPhone/{partnerId:int}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePartnerPhoneAsync(int partnerId, PartnerCreateDto model)
        {
            return this.Ok(await this.Service.CreatePartnerPhone(partnerId, model));
        }

        /// <summary>
        /// Updates partner by the specified parameters
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartnerAsync([FromBody] PartnerUpdateDto model)
        {
            return this.Ok(await this.Service.UpdatePartner(model));
        }

        /// <summary>
        /// Updates partner phones by the id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("/updatePhones")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartnerPhoneAsync([FromBody] PartnerUpdateDto model)
        {
            return this.Ok(await this.Service.UpdatePartnerPhone(model));
        }

        /// <summary>
        /// Removes a partner by Id
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpDelete("/{partnerId:int}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePartner(int partnerId)
        {
            return this.Ok(await this.Service.DeletePartner(partnerId));
        }
    }
}
