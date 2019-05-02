using System.Threading.Tasks;
using DunnhumbyHomeWork.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace DunnhumbyHomeWork.Controllers.Interfaces
{
    public interface IProductController
    {
        /// <summary>
        ///     Get an existing product
        /// </summary>
        /// <param name="productId"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="404">Status 400: Page not found</response>
        Task<IActionResult> Get(string productId);

        /// <summary>
        ///     Create a new product
        /// </summary>
        /// <param name="dtoProduct"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Post(ProductDTO dtoProduct);

        /// <summary>
        ///     Delete an existing product
        /// </summary>
        /// <param name="productId"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Delete(string productId);

        /// <summary>
        ///     Update an existing product
        /// </summary>
        /// <param name="dtoProduct"></param>
        /// <response code="200">Status 200: Success</response>
        /// <response code="400">Status 400: Bad Request</response>
        Task<IActionResult> Put(ProductDTO dtoProduct);
    }
}
