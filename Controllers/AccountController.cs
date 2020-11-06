using AutoMapper;
using AccountLibrary.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AccountLibrary.API.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public AccountController(
            IMapper mapper, ILogger<AccountController> logger, IConfiguration iConfig)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger;
            _configuration = iConfig;


        }

        [Route("account")]
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            string token = string.Empty;
            _logger.LogInformation("Method start account");
            if (Request.Headers.ContainsKey("Authorization"))
            {
                token = Request.Headers["Authorization"];
                string url = _configuration.GetSection("MySettings").GetSection("URL").Value;
                var jwttoken = token.Split(" ")[1];
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var jToken = (JwtSecurityToken)tokenHandler.ReadToken(jwttoken);
                var custId = jToken.Claims.ToList();
                var customerid = custId.Find(x => x.Type == "custom:CustomerId").Value;
                _logger.LogInformation("customer id passed " + customerid);
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("x-api-customerid", customerid);
                _logger.LogInformation("http call start");
                var accountsResponse = await client.GetAsync(url);
                _logger.LogInformation("http call end");

                var responseBodyAsText = await accountsResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("string response " + responseBodyAsText);

                if (accountsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return Ok(JsonConvert.DeserializeObject(responseBodyAsText));

            }
            return NotFound();

        }

        [Route("health")]
        [HttpGet]
        public ActionResult health()
        {
            return Ok();
        }

    }

}