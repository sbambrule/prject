using AuthenticationService.Models;
using AuthenticationService.Repository;
using AuthenticationService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Controllers
{
    /*
   As in this assignment, we are working with creating RESTful web service to create microservices, hence annotate
   the class with [ApiController] annotation and define the controller level route as per REST Api standard.
   */
    [EnableCors("AllowMyOrigin")]
    public class AuthController : Controller
    {
        /*
       AuthService should  be injected through constructor injection. Please note that we should not create service
       object using the new keyword
      */
        IAuthService authService;
        ITokenGenerator tokenGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="tokenGenerator"></param>
        public AuthController(IAuthService authService, ITokenGenerator tokenGenerator)
        {
            this.authService = authService;
            this.tokenGenerator = tokenGenerator;
        }

        /*
	     * Define a handler method which will create a specific user by reading the
	     * Serialized object from request body and save the user details in the
	     * database. This handler method should return any one of the status messages
	     * basis on different situations:
	     * 1. 201(CREATED) - If the user created successfully. 
	     * 2. 409(CONFLICT) - If the userId conflicts with any existing user
	     * 
	     * This handler method should map to the URL "/api/auth/register" using HTTP POST method
    	 */

        [Route("api/auth/register")]
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            var result =this.authService.RegisterUser(user);
            return StatusCode((int)HttpStatusCode.Created, result);
        }


        /* Define a handler method which will authenticate a user by reading the Serialized user
         * object from request body containing the username and password. The username and password should be validated 
         * before proceeding ahead with JWT token generation. The user credentials will be validated against the database entries. 
         * The error should be return if validation is not successful. If credentials are validated successfully, then JWT
         * token will be generated. The token should be returned back to the caller along with the API response.
         * This handler method should return any one of the status messages basis on different
         * situations:
         * 1. 200(OK) - If login is successful
         * 2. 401(UNAUTHORIZED) - If login is not successful
         * 
         * This handler method should map to the URL "/api/auth/login" using HTTP POST method
        */
        [Route("api/auth/login")]
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var v = this.authService.LoginUser(user);
            return Ok(tokenGenerator.GeneratorJwtToken(user.UserId));
        }

        [Route("api/auth/isAuthenticated")]
        [Authorize]
        [HttpPost]
        public IActionResult IsAuthenticated()
        {            
            //var claimsIdentity = User.Identity as ClaimsIdentity;
            return Ok(new { isAuthenticated = true });
        }
    }
}
