
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using FuncDLL;
using System;

namespace FindCustomer
{
    public static class FindCustomer
    {
        [FunctionName("FindCustomer")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            id = id ?? data?.id;
            var customer = new CustomerDA().GetCustomerByID(Convert.ToInt32(id));
            var jsonToReturn = JsonConvert.SerializeObject(customer);

            return customer != null
                ? (ActionResult)new OkObjectResult($"Morning, {jsonToReturn}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
