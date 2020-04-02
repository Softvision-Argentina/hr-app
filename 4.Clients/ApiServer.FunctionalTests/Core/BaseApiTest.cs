using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core;
using Domain.Services.Repositories.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;

namespace ApiServer.FunctionalTests.Core
{
    [Collection("Api collection")]
    public class BaseApiTest
    {
        protected HttpClient Client { get; }
        protected TestServer Server { get; }
        public IServiceProvider Services { get; }
        protected DataBaseContext Context { get; }
        public IConfiguration Configuration { get; }
        protected string ControllerName { get; set; }

        public BaseApiTest(ApiFixture apiFixture)
        {
            Client = apiFixture.Client;
            Server = apiFixture.Server;
            Services = apiFixture.Services;
            Context = apiFixture.Server.Host.Services.GetService(typeof(DataBaseContext)) as DataBaseContext;
            Configuration = apiFixture.Server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            Context.Database.EnsureCreated();
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

        private T ParseJsonStringToEntity<T>(string responseString) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception)
            {
                return default;
            }
        }

        private ResponseError ParseJsonStringErrorToResponseError(string responseString)
        {
            try
            {
                return JsonConvert.DeserializeObject<ResponseError>(responseString);
            }
            catch (Exception)
            {
                return default;
            }
        }

        private async Task<HttpResultData<T>> GetHttpCallAsync<T>(string endPoint) where T: class
        {
            var response = await Client.GetAsync($"/api/{endPoint}", HttpCompletionOption.ResponseContentRead);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = ParseJsonStringToEntity<T>(responseString),
                ResponseError = ParseJsonStringErrorToResponseError(responseString)
            };

            return result;
        }

        private async Task<HttpResultData<T>> DeleteHttpCallAsync<T>(string endPoint, int id) where T: class
        {
            var response = await Client.DeleteAsync($"/api/{endPoint}/{id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = ParseJsonStringToEntity<T>(responseString),
                ResponseError = ParseJsonStringErrorToResponseError(responseString)
            };

            return result;
        }

        private async Task<HttpResultData<T>> PostHttpCallAsync<T>(string endPoint, object model) where T: class
        {
            var response = await Client.PostAsync($"/api/{endPoint}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = ParseJsonStringToEntity<T>(responseString),
                ResponseError = ParseJsonStringErrorToResponseError(responseString)
            };

            return result;
        }

        private async Task<HttpResultData<T>> PutHttpCallAsync<T>(string endPoint, object model, int id) where T: class
        {
            var response = await Client.PutAsync($"/api/{endPoint}/{id}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = ParseJsonStringToEntity<T>(responseString),
                ResponseError = ParseJsonStringErrorToResponseError(responseString)
            };

            return result;
        }

        protected async Task<HttpResultData<T>> HttpCallAsync<T>(string httpVerb, string endPoint, object model = null, int id = default) where T: class
        {
            HttpResultData<T> result;

            switch (httpVerb)
            {
                case HttpVerb.GET:
                    result = await GetHttpCallAsync<T>(endPoint);
                    break;
                case HttpVerb.POST:
                    result = await PostHttpCallAsync<T>(endPoint, model);
                    break;
                case HttpVerb.PUT:
                    result = await PutHttpCallAsync<T>(endPoint, model, id);
                    break;
                case HttpVerb.DELETE:
                    result = await DeleteHttpCallAsync<T>(endPoint, id);
                    break;
                default: throw new NotImplementedException("Http verb is not implemented");
            }

            return result;
        }
    }
}
