#region Auto generated information. Please do not modify

// DunnhumbyHomeWork DunnhumbyHomeWork ProductController.cs
// bila007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-05-01 21:09
// 2019-05-01 20:53

#endregion

using System;
using System.Threading.Tasks;
using DunnhumbyHomeWork.Controllers.Interfaces;
using DunnhumbyHomeWork.DTOModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DunnhumbyHomeWork.Controllers
{
    [Route("api/product")]
    public class ProductController : IProductController, IDisposable
    {
        private readonly PRManagementDbContext dbContext;

        public ProductController(PRManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> Get([FromRoute] string productId)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(productId))
                    return new BadRequestObjectResult("Product Id is empty");

                var product =
                    await dbContext.Products.FirstOrDefaultAsync(item => item.Id == productId);

                if(product == null)
                    return new OkObjectResult(null);

                var dtoProduct = product.GetProductDto();
                return new OkObjectResult(dtoProduct);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO dtoProduct)
        {
            try
            {
                var dboProduct = dtoProduct.GetProductDbo();
                await dbContext.Products.AddAsync(dboProduct);
                await dbContext.SaveChangesAsync();

                return new OkObjectResult(dboProduct.GetProductDto());
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> Delete([FromRoute] string productId)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(productId))
                    return new BadRequestObjectResult("Product Id is empty");

                var product =
                    await dbContext.Products.FirstOrDefaultAsync(item => item.Id == productId);

                if(product == null)
                    return new BadRequestObjectResult($"Product not found: {productId}");

                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();

                return new OkObjectResult($"Product successfully deleted: {productId}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductDTO dtoProduct)
        {
            try
            {
                var product =
                    await dbContext.Products.FirstOrDefaultAsync(item => item.Id == dtoProduct.Id);

                if(product == null)
                    return new BadRequestObjectResult($"Product not found: {dtoProduct}");

                product.Name = dtoProduct.Name;
                product.Category = dtoProduct.Category;
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
    }
}
