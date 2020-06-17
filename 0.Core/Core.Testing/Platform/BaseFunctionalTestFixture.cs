using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Testing.Interfaces;
using Domain.Services.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistance.EF.Extensions;
using Xunit;

namespace Core.Testing.Platform
{
    public partial class BaseFunctionalTestFixture : WebAppFactory
    {
        public string ControllerName { get; set; }
        public BaseFunctionalTestFixture()
        {
            ContextAction((context) =>
            {
                context.ResetAllIdentitiesId();
            });
        }

        //Functional helper methods
        public async Task<HttpResultData<T>> HttpCallAsync<T>(string httpVerb, string endPoint, object model = null, int id = default(int)) where T : class
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
        private async Task<HttpResultData<T>> GetHttpCallAsync<T>(string endPoint) where T : class
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
        private async Task<HttpResultData<T>> DeleteHttpCallAsync<T>(string endPoint, int id) where T : class
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
        private async Task<HttpResultData<T>> PostHttpCallAsync<T>(string endPoint, object model) where T : class
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
        private async Task<HttpResultData<T>> PutHttpCallAsync<T>(string endPoint, object model, int id) where T : class
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
        private ResponseError ParseJsonStringErrorToResponseError(string responseString)
        {
            try
            {
                return JsonConvert.DeserializeObject<ResponseError>(responseString);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private T ParseJsonStringToEntity<T>(string responseString) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Dummy
        protected static void AssertSuccess(HttpResponseMessage response, string responseString)
        {
            Assert.True(response.IsSuccessStatusCode,
                $"Response have a fail code: {response.StatusCode}. Message: {responseString}");
            Assert.NotNull(responseString);
            Assert.NotEmpty(responseString);
        }
        protected async Task<U> CreateAsync<T, U>(T model, string controllerName)
        {
            var response = await Client.PostAsync($"/api/{controllerName}/",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();
            AssertSuccess(response, responseString);

            return JsonConvert.DeserializeObject<U>(responseString);
        }
    }
}
