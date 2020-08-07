// <copyright file="BaseFunctionalTestFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Testing.Platform
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xunit;

    public partial class BaseFunctionalTestFixture : WebAppFactory
    {
        public string ControllerName { get; set; }

        public BaseFunctionalTestFixture()
        {
            this.ContextAction((context) =>
            {
                // context.ResetAllIdentitiesId();
            });
        }

        // Functional helper methods
        public async Task<HttpResultData<T>> HttpCallAsync<T>(string httpVerb, string endPoint, object model = null, int id = default(int)) where T : class
        {
            HttpResultData<T> result;

            switch (httpVerb)
            {
                case HttpVerb.GET:
                    result = await this.GetHttpCallAsync<T>(endPoint).ConfigureAwait(false);
                    break;
                case HttpVerb.POST:
                    result = await this.PostHttpCallAsync<T>(endPoint, model).ConfigureAwait(false);
                    break;
                case HttpVerb.PUT:
                    result = await this.PutHttpCallAsync<T>(endPoint, model, id).ConfigureAwait(false);
                    break;
                case HttpVerb.DELETE:
                    result = await this.DeleteHttpCallAsync<T>(endPoint, id).ConfigureAwait(false);
                    break;
                default: throw new NotImplementedException("Http verb is not implemented");
            }

            return result;
        }

        private async Task<HttpResultData<T>> GetHttpCallAsync<T>(string endPoint) where T : class
        {
            var response = await this.Client.GetAsync($"/api/{endPoint}", HttpCompletionOption.ResponseContentRead);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = this.ParseJsonStringToEntity<T>(responseString),
                ResponseError = this.ParseJsonStringErrorToResponseError(responseString),
            };

            return result;
        }

        private async Task<HttpResultData<T>> DeleteHttpCallAsync<T>(string endPoint, int id) where T : class
        {
            var response = await this.Client.DeleteAsync($"/api/{endPoint}/{id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = this.ParseJsonStringToEntity<T>(responseString),
                ResponseError = this.ParseJsonStringErrorToResponseError(responseString),
            };

            return result;
        }

        private async Task<HttpResultData<T>> PostHttpCallAsync<T>(string endPoint, object model) where T : class
        {
            var response = await this.Client.PostAsync(
                $"/api/{endPoint}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = this.ParseJsonStringToEntity<T>(responseString),
                ResponseError = this.ParseJsonStringErrorToResponseError(responseString),
            };

            return result;
        }

        private async Task<HttpResultData<T>> PutHttpCallAsync<T>(string endPoint, object model, int id) where T : class
        {
            var response = await this.Client.PutAsync(
                $"/api/{endPoint}/{id}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            var result = new HttpResultData<T>
            {
                Response = response,
                ResponseString = responseString,
                ResponseEntity = this.ParseJsonStringToEntity<T>(responseString),
                ResponseError = this.ParseJsonStringErrorToResponseError(responseString),
            };

            return result;
        }

        private ExceptionData ParseJsonStringErrorToResponseError(string responseString)
        {
            try
            {
                return JsonConvert.DeserializeObject<ExceptionData>(responseString);
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

        // Dummy
        protected static void AssertSuccess(HttpResponseMessage response, string responseString)
        {
            Assert.True(
                response.IsSuccessStatusCode,
                $"Response have a fail code: {response.StatusCode}. Message: {responseString}");
            Assert.NotNull(responseString);
            Assert.NotEmpty(responseString);
        }

        protected async Task<TU> CreateAsync<T, TU>(T model, string controllerName)
        {
            var response = await this.Client.PostAsync(
                $"/api/{controllerName}/",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();
            AssertSuccess(response, responseString);

            return JsonConvert.DeserializeObject<TU>(responseString);
        }
    }
}
