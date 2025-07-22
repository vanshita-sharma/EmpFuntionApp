using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttFunc;

public static class EmployeesFunction
{
    private static readonly HttpClient client = new HttpClient();

    [FunctionName("GetAllEmployeesFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
        
        var backendUrl = "https://demoemp-hbgehsgwdygbbgh4.canadacentral-01.azurewebsites.net/api/Employee";

        // Forward request to backend API
        var response = await client.GetAsync(backendUrl);
        var content = await response.Content.ReadAsStringAsync();

        return new ContentResult
        {
            Content = content,
            ContentType = "application/json",
            StatusCode = (int)response.StatusCode
        };
    }
    [FunctionName("AddEmployeeFunction")]
    public static async Task<IActionResult> AddEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employees")] HttpRequest req)
    {
        var backendUrl = "https://demoemp-hbgehsgwdygbbgh4.canadacentral-01.azurewebsites.net/api/Employee";
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(backendUrl, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        return new ContentResult
        {
            Content = responseBody,
            ContentType = "application/json",
            StatusCode = (int)response.StatusCode
        };
    }
    [FunctionName("DeleteEmployeeFunction")]
    public static async Task<IActionResult> DeleteEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employees/{id}")] HttpRequest req, string id)
    {
        var backendUrl = $"https://demoemp-hbgehsgwdygbbgh4.canadacentral-01.azurewebsites.net/api//Employee{id}";
        var response = await client.DeleteAsync(backendUrl);
        var content = await response.Content.ReadAsStringAsync();

        return new ContentResult
        {
            Content = content,
            ContentType = "application/json",
            StatusCode = (int)response.StatusCode
        };
    }
}