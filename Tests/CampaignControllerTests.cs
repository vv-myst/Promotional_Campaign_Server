using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using DunnhumbyHomeWork;
using DunnhumbyHomeWork.Controllers;
using DunnhumbyHomeWork.DTOModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CampaignControllerTests
    {
        [SetUp]
        public void Init()
        {
            fixture = new Fixture();
            mockProductDto = new ProductDTO
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Category = fixture.Create<string>()
            };

            mockCampaignDto = new CampaignDTO
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Product = mockProductDto,
                StartDate = GetUnixDate(-30),
                EndDate = GetUnixDate(-10),
                IsActive = fixture.Create<bool>()
            };

            prManagementDbContext = SetUpDatabaseContext();
            campaignController = new CampaignController(prManagementDbContext);
        }

        [TearDown]
        public void Cleanup()
        {
            campaignController.Dispose();
            prManagementDbContext.Dispose();
        }

        private static PRManagementDbContext SetUpDatabaseContext()
        {
            var serviceProvider = new ServiceCollection()
                                  .AddEntityFrameworkInMemoryDatabase()
                                  .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<PRManagementDbContext>();
            builder.UseInMemoryDatabase("CampaignTestDatabase")
                   .UseInternalServiceProvider(serviceProvider);
            return new PRManagementDbContext(builder.Options);
        }

        private static string GetUnixDate(int addDays)
        {
            var date = DateTime.Today.AddDays(addDays);
            // var unixTime = ((DateTimeOffset) date.ToUniversalTime()).ToUnixTimeSeconds();
            return date.ConvertLocalDateTimeToUtcUnixString();
            //return unixTime.ToString();
        }

        private ProductDTO mockProductDto;
        private CampaignDTO mockCampaignDto;
        private CampaignController campaignController;
        private PRManagementDbContext prManagementDbContext;
        private Fixture fixture;

        [TestCase("dummyId")]
        public async Task Delete_NoCampaignExists_ReturnsStatus400(string inputId)
        {
            var sut = (BadRequestObjectResult) await campaignController.Delete(inputId);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        [TestCase("dummyId")]
        public async Task Put_NoCampaignExists_ReturnsStatus400(string inputId)
        {
            var dtoUpdatedCampaign = new CampaignDTO
            {
                Id = inputId,
                Name = mockCampaignDto.Name,
                StartDate = GetUnixDate(-200),
                EndDate = GetUnixDate(-150),
                IsActive = mockCampaignDto.IsActive,
                Product = mockCampaignDto.Product
            };

            var sut = (BadRequestObjectResult) await campaignController.Put(dtoUpdatedCampaign);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        [TestCase("dummyId")]
        public async Task Put_NoValidProductLinkedToCampaign_ReturnsStatus400(string inputId)
        {
            await campaignController.Post(mockCampaignDto);

            var dtoUpdatedProduct = new ProductDTO
            {
                Id = inputId, Name = mockProductDto.Name, Category = mockProductDto.Category
            };

            var dtoUpdatedCampaign = new CampaignDTO
            {
                Id = mockCampaignDto.Id,
                Name = mockCampaignDto.Name,
                StartDate = GetUnixDate(-200),
                EndDate = GetUnixDate(-150),
                IsActive = mockCampaignDto.IsActive,
                Product = dtoUpdatedProduct
            };

            var sut = (BadRequestObjectResult) await campaignController.Put(dtoUpdatedCampaign);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Delete_RemovesCampaignButNotProductFromDatabase()
        {
            await campaignController.Post(mockCampaignDto);

            var sut = (OkObjectResult) await campaignController.Delete(mockCampaignDto.Id);
            var campaign =
                await prManagementDbContext.Campaigns.FirstOrDefaultAsync(item =>
                    item.Id == mockCampaignDto.Id);
            var product =
                await prManagementDbContext.Products.FirstOrDefaultAsync(item =>
                    item.Id == mockCampaignDto.Product.Id);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(campaign, Is.Null);
            Assert.That(product, Is.Not.Null);
        }

        [Test]
        public async Task Get_GetExistingCampaign()
        {
            await campaignController.Post(mockCampaignDto);

            var sut = (OkObjectResult) await campaignController.Get(mockCampaignDto.Id);
            var actualResult = sut.Value as CampaignDTO;

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Id, Is.EqualTo(mockCampaignDto.Id));
            Assert.That(actualResult.Name, Is.EqualTo(mockCampaignDto.Name));
            Assert.That(actualResult.Product.Id, Is.EqualTo(mockCampaignDto.Product.Id));
            Assert.That(actualResult.Product.Name, Is.EqualTo(mockCampaignDto.Product.Name));
            Assert.That(actualResult.Product.Category,
                Is.EqualTo(mockCampaignDto.Product.Category));
            Assert.That(actualResult.StartDate, Is.EqualTo(mockCampaignDto.StartDate));
            Assert.That(actualResult.EndDate, Is.EqualTo(mockCampaignDto.EndDate));
            Assert.That(actualResult.IsActive, Is.EqualTo(mockCampaignDto.IsActive));
        }

        [Test]
        public async Task Get_GetsAllAvailableCampaigns()
        {
            await campaignController.Post(mockCampaignDto);

            var sut = (ObjectResult) await campaignController.Get((int?) null);
            var campaignList = sut.Value as IList<CampaignDTO>;

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(campaignList, Is.Not.Null);
            Assert.That(campaignList.Count, Is.EqualTo(1));

            var actualCampaign = campaignList[0];
            Assert.That(actualCampaign.Id, Is.EqualTo(mockCampaignDto.Id));
            Assert.That(actualCampaign.Name, Is.EqualTo(mockCampaignDto.Name));
            Assert.That(actualCampaign.Product.Id, Is.EqualTo(mockCampaignDto.Product.Id));
            Assert.That(actualCampaign.Product.Name, Is.EqualTo(mockCampaignDto.Product.Name));
            Assert.That(actualCampaign.Product.Category,
                Is.EqualTo(mockCampaignDto.Product.Category));
            Assert.That(actualCampaign.StartDate, Is.EqualTo(mockCampaignDto.StartDate));
            Assert.That(actualCampaign.EndDate, Is.EqualTo(mockCampaignDto.EndDate));
            Assert.That(actualCampaign.IsActive, Is.EqualTo(mockCampaignDto.IsActive));
        }

        [TestCase(1, 10)]
        [TestCase(2, 10)]
        [TestCase(3, 2)]
        public async Task Get_Pagination_GetsAllCampaignsInAPage(int pageNumber, int expectedResult)
        {
            // Add new campaigns to the database
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());
            await campaignController.Post(GetNewCampaign());

            var sut = (ObjectResult) await campaignController.Get(pageNumber);
            var campaignList = sut.Value as IList<CampaignDTO>;

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(campaignList, Is.Not.Null);
            Assert.That(campaignList.Count, Is.EqualTo(expectedResult));
        }

        private CampaignDTO GetNewCampaign()
        {
            var newDtoProduct = new ProductDTO
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Category = fixture.Create<string>()
            };
            
            var newDtoCampaign = new CampaignDTO
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Product = newDtoProduct,
                StartDate = GetUnixDate(-30),
                EndDate = GetUnixDate(-10),
                IsActive = fixture.Create<bool>()
            };

            return newDtoCampaign;
        }

        [Test]
        public async Task Get_NoCampaignExists_ReturnsEmptyResult()
        {
            var sut = (OkObjectResult) await campaignController.Get(mockCampaignDto.Id);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public async Task Get_ReturnsEmptyListIfNoCampaignsFound()
        {
            var sut = (OkObjectResult) await campaignController.Get((int?) null);
            var campaignList = sut.Value;

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(campaignList, Is.Null);
        }

        [Test]
        public async Task Post_CreatesANewCampaignInDatabase()
        {
            var mockProductDbo = mockProductDto.GetProductDbo();
            await prManagementDbContext.Products.AddAsync(mockProductDbo);
            await prManagementDbContext.SaveChangesAsync();

            var sut = (OkObjectResult) await campaignController.Post(mockCampaignDto);
            var returnResult = sut.Value as CampaignDTO;

            // Test the Campaign DTO returned
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(returnResult, Is.Not.Null);
            Assert.That(returnResult.Id, Is.EqualTo(mockCampaignDto.Id));
            Assert.That(returnResult.Name, Is.EqualTo(mockCampaignDto.Name));
            Assert.That(returnResult.Product.Id, Is.EqualTo(mockCampaignDto.Product.Id));
            Assert.That(returnResult.Product.Name, Is.EqualTo(mockCampaignDto.Product.Name));
            Assert.That(returnResult.Product.Category,
                Is.EqualTo(mockCampaignDto.Product.Category));
            Assert.That(returnResult.StartDate, Is.EqualTo(mockCampaignDto.StartDate));
            Assert.That(returnResult.EndDate, Is.EqualTo(mockCampaignDto.EndDate));
            Assert.That(returnResult.IsActive, Is.EqualTo(mockCampaignDto.IsActive));

            // Test the Campaign DBO inserted into database
            Assert.That(prManagementDbContext.Campaigns.Any(), Is.True);
            Assert.That(prManagementDbContext.Campaigns.ToListAsync().Result.Count, Is.EqualTo(1));

            var actualDbObject =
                await prManagementDbContext.Campaigns.FirstOrDefaultAsync(item =>
                    item.Id == mockCampaignDto.Id);

            var mockDbObject = mockCampaignDto.GetCampaignDbo();

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualDbObject, Is.Not.Null);
            Assert.That(actualDbObject.Id, Is.EqualTo(mockDbObject.Id));
            Assert.That(actualDbObject.Name, Is.EqualTo(mockDbObject.Name));
            Assert.That(actualDbObject.ProductId, Is.EqualTo(mockDbObject.ProductId));
            Assert.That(actualDbObject.StartDate, Is.EqualTo(mockDbObject.StartDate));
            Assert.That(actualDbObject.EndDate, Is.EqualTo(mockDbObject.EndDate));
            Assert.That(actualDbObject.IsActive, Is.EqualTo(mockDbObject.IsActive));
        }

        [Test]
        public async Task Post_CreatesANewProductAndCampaignInDatabase()
        {
            var sut = (OkObjectResult) await campaignController.Post(mockCampaignDto);
            var returnResult = sut.Value as CampaignDTO;

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            // Test Product DBO inserted into database
            Assert.That(prManagementDbContext.Products.Any(), Is.True);
            Assert.That(prManagementDbContext.Products.ToListAsync().Result.Count, Is.EqualTo(1));

            var actualProductDbo =
                await prManagementDbContext.Products.FirstOrDefaultAsync(item =>
                    item.Id == mockCampaignDto.Product.Id);

            Assert.That(actualProductDbo, Is.Not.Null);
            Assert.That(actualProductDbo.Id, Is.EqualTo(mockCampaignDto.Product.Id));
            Assert.That(actualProductDbo.Name, Is.EqualTo(mockCampaignDto.Product.Name));
            Assert.That(actualProductDbo.Category, Is.EqualTo(mockCampaignDto.Product.Category));

            // Test the Campaign DTO returned
            Assert.That(returnResult, Is.Not.Null);
            Assert.That(returnResult.Id, Is.EqualTo(mockCampaignDto.Id));
            Assert.That(returnResult.Name, Is.EqualTo(mockCampaignDto.Name));
            Assert.That(returnResult.Product.Id, Is.EqualTo(mockCampaignDto.Product.Id));
            Assert.That(returnResult.Product.Name, Is.EqualTo(mockCampaignDto.Product.Name));
            Assert.That(returnResult.Product.Category,
                Is.EqualTo(mockCampaignDto.Product.Category));
            Assert.That(returnResult.StartDate, Is.EqualTo(mockCampaignDto.StartDate));
            Assert.That(returnResult.EndDate, Is.EqualTo(mockCampaignDto.EndDate));
            Assert.That(returnResult.IsActive, Is.EqualTo(mockCampaignDto.IsActive));

            // Test the Campaign DBO inserted into database
            Assert.That(prManagementDbContext.Campaigns.Any(), Is.True);
            Assert.That(prManagementDbContext.Campaigns.ToListAsync().Result.Count, Is.EqualTo(1));

            var actualDbObject =
                await prManagementDbContext.Campaigns.FirstOrDefaultAsync(item =>
                    item.Id == mockCampaignDto.Id);

            var mockDbObject = mockCampaignDto.GetCampaignDbo();

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualDbObject, Is.Not.Null);
            Assert.That(actualDbObject.Id, Is.EqualTo(mockDbObject.Id));
            Assert.That(actualDbObject.Name, Is.EqualTo(mockDbObject.Name));
            Assert.That(actualDbObject.ProductId, Is.EqualTo(mockDbObject.ProductId));
            Assert.That(actualDbObject.StartDate, Is.EqualTo(mockDbObject.StartDate));
            Assert.That(actualDbObject.EndDate, Is.EqualTo(mockDbObject.EndDate));
            Assert.That(actualDbObject.IsActive, Is.EqualTo(mockDbObject.IsActive));
        }

        [Test]
        public async Task Post_StartDateAfterEndDateReturnsStatusCode400()
        {
            var startDate = DateTime.Now.ConvertLocalDateTimeToUtcUnixString();
            var endDate = DateTime.Now.AddDays(-10).ConvertLocalDateTimeToUtcUnixString();
            mockCampaignDto.StartDate = startDate;
            mockCampaignDto.EndDate = endDate;

            var sut = (BadRequestObjectResult) await campaignController.Post(mockCampaignDto);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Put_UpdatesCampaignData()
        {
            await campaignController.Post(mockCampaignDto);

            var startUnixDate = GetUnixDate(-200);
            var startDateTime = startUnixDate.ConvertUtcUnixStringToLocalDateTime();
            var endUnixDate = GetUnixDate(-150);
            var endDateTime = endUnixDate.ConvertUtcUnixStringToLocalDateTime();

            var dtoUpdatedCampaign = new CampaignDTO
            {
                Id = mockCampaignDto.Id,
                Name = mockCampaignDto.Name,
                StartDate = startUnixDate,
                EndDate = endUnixDate,
                IsActive = mockCampaignDto.IsActive,
                Product = mockCampaignDto.Product
            };

            var sut = (OkObjectResult) await campaignController.Put(dtoUpdatedCampaign);
            var actualResult =
                await prManagementDbContext.Campaigns.FirstOrDefaultAsync(item =>
                    item.Id == dtoUpdatedCampaign.Id);

            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Id, Is.EqualTo(dtoUpdatedCampaign.Id));
            Assert.That(actualResult.Name, Is.EqualTo(dtoUpdatedCampaign.Name));
            Assert.That(actualResult.Product.Id, Is.EqualTo(dtoUpdatedCampaign.Product.Id));
            Assert.That(actualResult.Product.Name, Is.EqualTo(dtoUpdatedCampaign.Product.Name));
            Assert.That(actualResult.Product.Category,
                Is.EqualTo(dtoUpdatedCampaign.Product.Category));
            Assert.That(actualResult.StartDate, Is.EqualTo(startDateTime));
            Assert.That(actualResult.EndDate, Is.EqualTo(endDateTime));
            Assert.That(actualResult.IsActive, Is.EqualTo(dtoUpdatedCampaign.IsActive));
        }
    }
}
