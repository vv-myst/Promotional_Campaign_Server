using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using DunnhumbyHomeWork;
using DunnhumbyHomeWork.Controllers;
using DunnhumbyHomeWork.DTOModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        [SetUp]
        public void Init()
        {
            var fixture = new Fixture();
            mockProductDto = new ProductDTO
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Category = fixture.Create<string>()
            };

            prManagementDbContext = SetUpDatabaseContext();
            productController = new ProductController(prManagementDbContext);
        }

        [TearDown]
        public void Cleanup()
        {
            productController.Dispose();
            prManagementDbContext.Dispose();
        }

        private ProductDTO mockProductDto;
        private ProductController productController;
        private PRManagementDbContext prManagementDbContext;

        private static PRManagementDbContext SetUpDatabaseContext()
        {
            var serviceProvider = new ServiceCollection()
                                  .AddEntityFrameworkInMemoryDatabase()
                                  .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<PRManagementDbContext>();
            builder.UseInMemoryDatabase("ProductTestDatabase")
                   .UseInternalServiceProvider(serviceProvider);
            return new PRManagementDbContext(builder.Options);
        }

        [Test]
        public async Task Post_CreatesANewProductInDatabase()
        {
            var sut = (OkObjectResult) await productController.Post(mockProductDto);
            var actualResult = sut.Value as ProductDTO;
            
            // Test the Product DTO returned
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Id, Is.EqualTo(mockProductDto.Id));
            Assert.That(actualResult.Name, Is.EqualTo(mockProductDto.Name));
            Assert.That(actualResult.Category, Is.EqualTo(mockProductDto.Category));
            
            // Test the Product DBO inserted into database
            Assert.That(prManagementDbContext.Products.Any(), Is.True);
            Assert.That(prManagementDbContext.Products.ToListAsync().Result.Count, Is.EqualTo(1));

            var actualDbObject =
                await prManagementDbContext.Products.FirstOrDefaultAsync(item =>
                    item.Id == mockProductDto.Id);
            
            Assert.That(actualDbObject.Id, Is.EqualTo(mockProductDto.Id));
            Assert.That(actualDbObject.Name, Is.EqualTo(mockProductDto.Name));
            Assert.That(actualDbObject.Category, Is.EqualTo(mockProductDto.Category));
        }
        
        [Test]
        public async Task Get_GetExistingProduct()
        {
            await productController.Post(mockProductDto);
            
            var sut = (OkObjectResult) await productController.Get(mockProductDto.Id);
            var actualResult = sut.Value as ProductDTO;
            
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Id, Is.EqualTo(mockProductDto.Id));
            Assert.That(actualResult.Name, Is.EqualTo(mockProductDto.Name));
            Assert.That(actualResult.Category, Is.EqualTo(mockProductDto.Category));
        }
        
        [TestCase("dummyId")]
        public async Task Get_NoProductExists_ReturnsEmptyResult(string inputId)
        {
            await productController.Post(mockProductDto);
            
            var sut = (OkObjectResult) await productController.Get(inputId);
           
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(sut.Value, Is.Null);
        }
        
        [Test]
        public async Task Get_EmptyProductId_ReturnsStatus400()
        {
            await productController.Post(mockProductDto);
            
            var sut = (BadRequestObjectResult) await productController.Get(string.Empty);
           
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Delete_RemovesProductFromDatabase()
        {
            await productController.Post(mockProductDto);

            var sut = (OkObjectResult) await productController.Delete(mockProductDto.Id);

            var product =
                await prManagementDbContext.Products.FirstOrDefaultAsync(item =>
                    item.Id == mockProductDto.Id);
            
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(product, Is.Null);
        }
        
        [TestCase("dummyId")]
        [TestCase("")]
        public async Task Delete_NoProductExists_ReturnsStatus400(string inputId)
        {
            await productController.Post(mockProductDto);
            
            var sut = (BadRequestObjectResult) await productController.Delete(inputId);
           
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Put_UpdatesProductData()
        {
            await productController.Post(mockProductDto);
            var fixture = new Fixture();

            var dtoUpdatedProduct = new ProductDTO
            {
                Id = mockProductDto.Id,
                Name = fixture.Create<string>(),
                Category = fixture.Create<string>()
            };

            var sut = (OkObjectResult) await productController.Put(dtoUpdatedProduct);

            var actualResult =
                await prManagementDbContext.Products.FirstOrDefaultAsync(item =>
                    item.Id == dtoUpdatedProduct.Id);
            
            Assert.That(sut.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Id, Is.EqualTo(dtoUpdatedProduct.Id));
            Assert.That(actualResult.Name, Is.EqualTo(dtoUpdatedProduct.Name));
            Assert.That(actualResult.Category, Is.EqualTo(dtoUpdatedProduct.Category));
        }
    }
}
