using System.Threading.Tasks;
using DunnhumbyHomeWork.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace DunnhumbyHomeWork.Controllers.Interfaces
{
    public interface ICampaignController
    {
        /// <summary>
        ///     Get all existing campaigns
        /// </summary>
        /// <param name="page"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Get(int? page);

        /// <summary>
        ///     Get a single campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Get(string campaignId);

        /// <summary>
        ///     Add new campaign
        /// </summary>
        /// <param name="dtoCampaign"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Post(CampaignDTO dtoCampaign);

        /// <summary>
        ///     Delete an existing campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Delete(string campaignId);

        /// <summary>
        ///     Update an exisiting campaign
        /// </summary>
        /// <param name="dtoCampaign"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Put(CampaignDTO dtoCampaign);
    }
}
