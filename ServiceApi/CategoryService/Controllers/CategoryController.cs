using System;
using Microsoft.AspNetCore.Mvc;
using CategoryService.Service;
using CategoryService.Models;
using CategoryService.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;

namespace CategoryService.API.Controllers
{
    /*
    As in this assignment, we are working with creating RESTful web service to create microservices, hence annotate
    the class with [ApiController] annotation and define the controller level route as per REST Api standard.
    */
    [EnableCors("AllowMyOrigin")]
   // [Authorize]
    public class CategoryController : Controller
    {
        /*
     CategoryService should  be injected through constructor injection. Please note that we should not create service
     object using the new keyword
    */
        ICategoryService categoryService;
        /*
      * CategoryService should  be injected through constructor injection. Please note that we should not create service
      * object using the new keyword
      */
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Method to get category information by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("api/[controller]/{categoryId:int}")]
        [HttpGet]
        public IActionResult Get(int categoryId)
        {
            var o = categoryService.GetCategoryById(categoryId);
            return Ok(o);
        }

        /// <summary>
        /// Method to delete category information by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("api/[controller]/{categoryId}")]
        [HttpDelete]
        public IActionResult Delete(int categoryId)
        {
            var o = categoryService.DeleteCategory(categoryId);
            return Ok(o);
        }

        /// <summary>
        /// Method to add category information  
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if (string.IsNullOrEmpty(category.CreatedBy))
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                category.CreatedBy = claimsIdentity.Name;
            }
            var o = categoryService.CreateCategory(category);
            return StatusCode((int)HttpStatusCode.Created, o);
        }


        /// <summary>
        /// Method to get category information by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/[controller]/{userId}")]
        [HttpGet]
        public IActionResult Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                userId = claimsIdentity.Name;
            }
            var o = categoryService.GetAllCategoriesByUserId(userId);
            return Ok(o);
        }
        /// <summary>
        /// Method to update category information by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("api/[controller]/{categoryId}")]
        [HttpPut]
        public IActionResult Put(int categoryId, [FromBody] Category category)
        {
            if (string.IsNullOrEmpty(category.CreatedBy))
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                category.CreatedBy = claimsIdentity.Name;
            }
            var o = categoryService.UpdateCategory(categoryId, category);
            return Ok(o);
        }
        /*
	 * Define a handler method which will create a category by reading the
	 * Serialized category object from request body and save the category in
	 * database. This handler method should return any one of the status messages basis on
	 * different situations: 
	 * 1. 201(CREATED - In case of successful creation of the category
	 * 2. 409(CONFLICT) - In case of duplicate categoryId
	 *
	 * 
	 * This handler method should map to the URL "/api/category" using HTTP POST
	 * method".
	 */

        /*
         * Define a handler method which will delete a category from a database.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 1. 200(OK) - If the category deleted successfully from
         * database. 2. 404(NOT FOUND) - If the category with specified categoryId is
         * not found. 
         * 
         * This handler method should map to the URL "/api/category/{id}" using HTTP Delete
         * method" where "id" should be replaced by a valid categoryId without {}
         */


        /*
         * Define a handler method which will update a specific category by reading the
         * Serialized object from request body and save the updated category details in
         * database. This handler method should return any one of the status
         * messages basis on different situations: 1. 200(OK) - If the category updated
         * successfully. 2. 404(NOT FOUND) - If the category with specified categoryId
         * is not found. 
         * This handler method should map to the URL "/api/category/{id}" using HTTP PUT
         * method.
         */


        /*
         * Define a handler method which will get us the category by a userId.
         * This handler method should return any one of the status messages basis on
         * different situations: 1. 200(OK) - If the category found successfully. 
         * This handler method should map to the URL "/api/category/{userId}" using HTTP GET method
         */

        /*
     * Define a handler method which will get us the category by a categoryId.
     * This handler method should return any one of the status messages basis on
     * different situations: 1. 200(OK) - If the category found successfully. 
     * This handler method should map to the URL "/api/category/{categoryId}" using HTTP GET method. categoryId must be an integer
     */
    }
}
