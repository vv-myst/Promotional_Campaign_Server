#region Auto generated information. Please do not modify

// DunnhumbyHomeWork DunnhumbyHomeWork CampaignController.cs
// bila007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-05-01 22:27
// 2019-04-28 22:35

#endregion

using System;
using System.Linq;
using System.Threading.Tasks;
using DunnhumbyHomeWork.Controllers.Interfaces;
using DunnhumbyHomeWork.DbModel;
using DunnhumbyHomeWork.DTOModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DunnhumbyHomeWork.Controllers
{
    [Route("api/campaign")]
    public class CampaignController : ICampaignController, IDisposable
    {
        private readonly PRManagementDbContext dbContext;

        public CampaignController(PRManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get([FromQuery] int? page)
        {
            try
            {
                var pageNumber = page ?? 1;
                var itemsToSkip = (pageNumber - 1) * 10;

                var dboCampaignList = await dbContext.Campaigns.Include(item => item.Product)
                                                     .OrderBy(item => item.CreationTime)
                                                     .Skip(itemsToSkip)
                                                     .Take(10)
                                                     .ToListAsync();

                if(!dboCampaignList.Any())
                    return new OkObjectResult(null);

                var dtoCampaignsList = dboCampaignList.GetCampaignDtoList();
                return new OkObjectResult(dtoCampaignsList);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpGet]
        [Route("{campaignId}")]
        public async Task<IActionResult> Get([FromRoute] string campaignId)
        {
            try
            {
                var campaign = await dbContext.Campaigns.Include(item => item.Product)
                                              .FirstOrDefaultAsync(item => item.Id == campaignId);

                if(campaign == null)
                    return new OkObjectResult(null);

                var dtoCampaign = campaign.GetCampaignDto();
                return new OkObjectResult(dtoCampaign);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CampaignDTO dtoCampaign)
        {
            try
            {
                var product =
                    await dbContext.Products.FirstOrDefaultAsync(item =>
                        item.Id == dtoCampaign.Product.Id);

                if(product == null)
                    await CreateNewProduct(dtoCampaign.Product);

                var dboCampaign = await CreateNewCampaign(dtoCampaign);
                return new OkObjectResult(dboCampaign.GetCampaignDto());
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpDelete]
        [Route("{campaignId}")]
        public async Task<IActionResult> Delete([FromRoute] string campaignId)
        {
            try
            {
                var campaign =
                    await dbContext.Campaigns.FirstOrDefaultAsync(item => item.Id == campaignId);

                if(campaign == null)
                    return new BadRequestObjectResult(
                        $"No campaign found for the given campaign Id: {campaignId}");

                dbContext.Campaigns.Remove(campaign);
                await dbContext.SaveChangesAsync();

                return new OkObjectResult($"Campaign successfully deleted: {campaignId}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CampaignDTO dtoCampaign)
        {
            try
            {
                var campaign = await dbContext
                                     .Campaigns.Include(item => item.Product)
                                     .FirstOrDefaultAsync(item => item.Id == dtoCampaign.Id);

                if(campaign == null)
                    return new BadRequestObjectResult($"Campaign not found: {dtoCampaign}");

                var product =
                    await dbContext.Products.FirstOrDefaultAsync(item =>
                        item.Id == dtoCampaign.Product.Id);

                if(product == null)
                    return new BadRequestObjectResult(
                        $"Product linked with campaign not found: {dtoCampaign.Product}");

                var dbCampaign = dtoCampaign.GetCampaignDbo();
                campaign.Name = dbCampaign.Name;
                campaign.StartDate = dbCampaign.StartDate;
                campaign.EndDate = dbCampaign.EndDate;
                campaign.IsActive = dbCampaign.IsActive;
                campaign.ProductId = dbCampaign.ProductId;

                await dbContext.SaveChangesAsync();
                return new OkObjectResult("Changes successfully saved");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        private async Task CreateNewProduct(ProductDTO dtoProduct)
        {
            try
            {
                var dboProduct = dtoProduct.GetProductDbo();
                await dbContext.Products.AddAsync(dboProduct);
                await dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.StackTrace);
            }
        }

        private async Task<Campaign> CreateNewCampaign(CampaignDTO dtoCampaign)
        {
            try
            {
                var dboProduct = dtoCampaign.GetCampaignDbo();
                await dbContext.Campaigns.AddAsync(dboProduct);
                await dbContext.SaveChangesAsync();
                return dboProduct;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.StackTrace);
            }
        }
    }
}
