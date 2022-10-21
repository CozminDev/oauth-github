using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Models;
using System.Net.Http.Headers;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SignInController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery] string code)
        {
            HttpClient authorizationClient = httpClientFactory.CreateClient("github-oauth");

            string client_id = "fb946a02836c5a4c7416";
            string client_secret = "3bda20ff19f8ad9fc46429f5f4ae8235ad9f93a2";

            HttpResponseMessage accessTokenResponse = await authorizationClient.PostAsync($"access_token?client_id={client_id}&client_secret={client_secret}&code={code}", null);

            if (accessTokenResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return Unauthorized();

            AccessTokenResponse? token = JsonConvert.DeserializeObject<AccessTokenResponse>(await accessTokenResponse.Content.ReadAsStringAsync());

            HttpClient githubApiClient = httpClientFactory.CreateClient("github-api");

            githubApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token.access_token}");

            HttpResponseMessage userResponse = await githubApiClient.GetAsync("user");

            if (userResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return Unauthorized();

            User? user = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync());

            return Ok(user);
        }
    }
}
