using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Exceptions;
using UserService.Models;
using UserService.Service;

namespace UserService.Controllers
{
    /*
   As in this assignment, we are working with creating RESTful web service to create microservices, hence annotate
   the class with [ApiController] annotation and define the controller level route as per REST Api standard.
   */
    [EnableCors("AllowMyOrigin")]
    
    public class UserController : Controller
    {
        /*
    UserService should  be injected through constructor injection. Please note that we should not create service
    object using the new keyword
   */

        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Method to get user information by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("api/[controller]/{userId}")]
        [HttpGet]
        public IActionResult Get(string userId)
        {
            var o = userService.GetUserById(userId);
            return Ok(o);
        }
        /// <summary>
        /// Method to delete user information by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("api/[controller]/{userId}")]
        [HttpDelete]
        public IActionResult Delete(string userId)
        {
            var o = userService.DeleteUser(userId);
            return Ok(o);
        }
        /// <summary>
        /// Method to regiter user information 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var o = userService.RegisterUser(user);
            return StatusCode((int)HttpStatusCode.Created, o);
        }

        /// <summary>
        /// Method to update user information by id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [Route("api/[controller]/{userId}")]
        [HttpPut]
        public IActionResult Put(string userId, [FromBody] User user)
        {
            var o = userService.UpdateUser(userId, user);
            return Ok(user);
        }

        /*
         * Define a handler method which will create a specific user by reading the
         * Serialized object from request body and save the user details in the
         * database. This handler method should return any one of the status messages
         * basis on different situations:
         * 1. 201(CREATED) - If the user created successfully. 
         * 2. 409(CONFLICT) - If the userId conflicts with any existing user
         * 
         * This handler method should map to the URL "/api/user" using HTTP POST method
         */

        /*
         * Define a handler method which will update a specific user by reading the
         * Serialized object from request body and save the updated user details in a
         * database. This handler method should return any one of the status messages
         * basis on different situations: 
         * 1. 200(OK) - If the user updated successfully.
         * 2. 404(NOT FOUND) - If the user with specified userId is not found.
         * 
         * This handler method should map to the URL "/api/user/{id}" using HTTP PUT method.
         */

        /*
         * Define a handler method which will delete a user from a database.
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the user deleted successfully from database. 
         * 2. 404(NOT FOUND) - If the user with specified userId is not found.
         *
         * This handler method should map to the URL "/api/user/{id}" using HTTP Delete
         * method" where "id" should be replaced by a valid userId without {}
         */

        /*
         * Define a handler method which will show details of a specific user. This
         * handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the user found successfully. 
         * 2. 404(NOT FOUND) - If the user with specified userId is not found. 
         * This handler method should map to the URL "/api/user/{id}" using HTTP GET method where "id" should be
         * replaced by a valid userId without {}
         */
    }
}
