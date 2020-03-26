using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core;
using Domain.Services.Repositories.EF;

namespace ApiServer.Tests.Seed
{
    [Collection("Api collection")]
    public class BaseApiTest
    {
        protected HttpClient Client { get; }

        protected string ControllerName { get; set; }
        protected DataBaseContext Context {get;}

        public BaseApiTest(ApiFixture apiFixture)
        {
            Client = apiFixture.Client;
            Context = apiFixture.Server.Host.Services.GetService(typeof(DataBaseContext)) as DataBaseContext;
        }

        protected static void AssertSuccess(HttpResponseMessage response, string responseString)
        {
            Assert.True(response.IsSuccessStatusCode,
                            $"Response have a fail code: {response.StatusCode}. Message: {responseString}");
            Assert.NotNull(responseString);
            Assert.NotEmpty(responseString);
        }

        protected async Task<U> CreateAsync<T, U>(T model)
        {
            var response = await Client.PostAsync($"/api/{ControllerName}/",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();
            AssertSuccess(response, responseString);

            return JsonConvert.DeserializeObject<U>(responseString);
        }

        private async Task<HttpResultData> GetHttpCallAsync<T>(string controllerName, T model, string controllerMethod = "", string param = "")
        {
            var response = await Client.GetAsync($"/api/{controllerName}/{controllerMethod}/{param}", HttpCompletionOption.ResponseContentRead);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                Response = response,
                ResponseString = responseString
            };

            return result;
        }

        private async Task<HttpResultData> DeleteHttpCallAsync<T>(string controllerName, T model, string controllerMethod = "", string param = "")
        {
            var response = await Client.DeleteAsync($"/api/{controllerName}/{controllerMethod}/{param}");
            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                Response = response,
                ResponseString = responseString
            };

            return result;
        }

        private async Task<HttpResultData> PostHttpCallAsync<T>(string controllerName, T model, string controllerMethod = "", string param = "")
        {
            var response = await Client.PostAsync($"/api/{controllerName}/{controllerMethod}/{param}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                Response = response,
                ResponseString = responseString
            };

            return result;
        }

        private async Task<HttpResultData> PutHttpCallAsync<T>(string controllerName, T model, string controllerMethod = "")
        {
            var response = await Client.PutAsync($"/api/{controllerName}/{controllerMethod}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                Response = response,
                ResponseString = responseString
            };

            return result;
        }

        protected async Task<HttpResultData> HttpCallAsync<T>(string httpVerb, T model, string controllerName, string controllerMethod = "", string param = "") where T: class
        {
            HttpResultData result;

            switch (httpVerb)
            {
                case HttpVerb.GET:
                    result = await GetHttpCallAsync(controllerName, model, controllerMethod);
                    break;
                case HttpVerb.POST:
                    result = await PostHttpCallAsync(controllerName, model, controllerMethod);
                    break;
                case HttpVerb.PUT:
                    result = await PutHttpCallAsync(controllerName, model, controllerMethod);
                    break;
                case HttpVerb.DELETE:
                    result = await DeleteHttpCallAsync(controllerName, model, controllerMethod);
                    break;
                default: throw new NotImplementedException("Http verb is not implemented");
            }

            return result;
        }
    }
}
