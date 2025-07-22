using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Text;
namespace HttFunc;

public class EmployeesFunction
{
    private readonly HttpClient _client;
    private readonly ILogger _logger;

    public EmployeesFunction(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
    {
        _client = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<EmployeesFunction>();
    }


    [Function("GetAllEmployeesFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
    {
        
        var backendUrl = "https://demoemp-hbgehsgwdygbbgh4.canadacentral-01.azurewebsites.net/api/Employee";
        var response = await _client.GetAsync(backendUrl);
        // Forward request to backend API
        var responseBody = await response.Content.ReadAsStringAsync();

        var httpResponse = req.CreateResponse((HttpStatusCode)response.StatusCode);
        httpResponse.Headers.Add("Content-Type", "application/json");
        await httpResponse.WriteStringAsync(responseBody);

        return httpResponse;
    }
    [Function("AddEmployeeFunction")]
    public async Task<HttpResponseData> AddEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employees")] HttpRequestData req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var backendUrl = "https://demoemp-hbgehsgwdygbbgh4.canadacentral-01.azurewebsites.net/api/Employee";
        var response = await _client.PostAsync(backendUrl, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        var httpResponse = req.CreateResponse((HttpStatusCode)response.StatusCode);
        httpResponse.Headers.Add("Content-Type", "application/json");
        await httpResponse.WriteStringAsync(responseBody);

        return httpResponse;
    }
    [Function("DeleteEmployeeFunction")]
    public async Task<HttpResponseData> DeleteEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employees/{id}")] HttpRequestData req,
        string id)
    {
        var backendUrl = $"https://demoemp-hbgehsgwdygbbgh4.canadacentral-01.azurewebsites.net/api/Employee/{id}";
        var response = await _client.DeleteAsync(backendUrl);
        var responseBody = await response.Content.ReadAsStringAsync();

        var httpResponse = req.CreateResponse((HttpStatusCode)response.StatusCode);
        httpResponse.Headers.Add("Content-Type", "application/json");
        await httpResponse.WriteStringAsync(responseBody);

        return httpResponse;
    }
}