using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core;

namespace ApiServer.Tests.Seed
{
    [Collection("Api collection")]
    public class BaseApiTest
    {
        protected HttpClient Client { get; }

        protected string ControllerName { get; set; }

        public BaseApiTest(ApiFixture apiFixture)
        {
            Client = apiFixture.Client;
        }

        protected static void AssertSuccess(HttpResponseMessage response, string responseString)
        {
            Assert.True(response.IsSuccessStatusCode,
                            $"Response have a fail code: {response.StatusCode}. Message: {responseString}");
            Assert.NotNull(responseString);
            Assert.NotEmpty(responseString);
        }

        protected async Task<U> Create<T, U>(T model)
        {
            var response = await Client.PostAsync($"/api/{ControllerName}/",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();
            AssertSuccess(response, responseString);

            return JsonConvert.DeserializeObject<U>(responseString);
        }

        private async Task<HttpResultData> GetHttpCall<T>(string controllerName, T model, string controllerMethod = "", string param = "")
        {
            var _response = await Client.GetAsync($"/api/{controllerName}/{controllerMethod}/{param}", HttpCompletionOption.ResponseContentRead);
            var _responseString = await _response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                response = _response,
                responseString = _responseString
            };

            return result;
        }

        private async Task<HttpResultData> DeleteHttpCall<T>(string controllerName, T model, string controllerMethod = "", string param = "")
        {
            var _response = await Client.DeleteAsync($"/api/{controllerName}/{controllerMethod}/{param}");
            var _responseString = await _response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                response = _response,
                responseString = _responseString
            };

            return result;
        }

        private async Task<HttpResultData> PostHttpCall<T>(string controllerName, T model, string controllerMethod = "", string param = "")
        {
            var _response = await Client.PostAsync($"/api/{controllerName}/{controllerMethod}/{param}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var _responseString = await _response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                response = _response,
                responseString = _responseString
            };

            return result;
        }

        private async Task<HttpResultData> PutHttpCall<T>(string controllerName, T model, string controllerMethod = "")
        {
            var _response = await Client.PutAsync($"/api/{controllerName}/{controllerMethod}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var _responseString = await _response.Content.ReadAsStringAsync();

            var result = new HttpResultData
            {
                response = _response,
                responseString = _responseString
            };

            return result;
        }

        protected async Task<HttpResultData> HttpCall<T>(string httpVerb, T model, string controllerName, string controllerMethod = "", string param = "") where T: class
        {
            HttpResultData result;

            switch (httpVerb)
            {
                case HttpVerb.GET:
                    result = await GetHttpCall(controllerName, model, controllerMethod);
                    break;
                case HttpVerb.POST:
                    result = await PostHttpCall(controllerName, model, controllerMethod);
                    break;
                case HttpVerb.PUT:
                    result = await PutHttpCall(controllerName, model, controllerMethod);
                    break;
                case HttpVerb.DELETE:
                    result = await DeleteHttpCall(controllerName, model, controllerMethod);
                    break;
                default: throw new NotImplementedException("Http verb is not implemented");
            }

            return result;
        }
    }
}
