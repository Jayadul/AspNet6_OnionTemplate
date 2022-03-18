using Core.Domain.Shared.Entities;
using Core.Domain.Shared.Enum;
using Core.Domain.Shared.Wrappers;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Helper
{
    public static class HttpHelper
    {

        public static async Task<object> Post<T>(ApiPostObj apiPostObj, T contentValue)
        {
            var client = new RestClient(apiPostObj.ApiBasicUrl);
            var request = new RestRequest(apiPostObj.Url, Method.POST);

            if (apiPostObj.ApiPostCategoryType == ApiPostCategoryType.MultiPostFormData)
            {
                request.AddParameter("Data", JsonConvert.SerializeObject(contentValue));

                request.AlwaysMultipartFormData = true;
            }
            else if (apiPostObj.ApiPostCategoryType == ApiPostCategoryType.ApplicationJson)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(contentValue);
            }

            var response = await client.ExecuteAsync<Response<object>>(request);
            return response.Data != null ? response.Data.Data: null;
        }

        public static async Task<T> GetAllPost<T>(ApiPostObj apiPostObj, object contentValue)
        {
            var client = new RestClient(apiPostObj.ApiBasicUrl);
            var request = new RestRequest(apiPostObj.Url, Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(contentValue);
            var response = await client.ExecuteAsync<Response<T>>(request);
            if (response.IsSuccessful)
            {
                var result = response.Data;
                if (result != null)
                {
                    return result.Data;
                }
            }

            return await Task.FromResult<T>(default(T));
        }

        public static async Task<T> Get<T>(string apiBasicUri, string url)
        {
            var client = new RestClient(apiBasicUri);
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            var response = await client.ExecuteAsync<Response<T>>(request);
            if (response.IsSuccessful)
            {
                var result = response.Data;
                if (result != null)
                {
                    return result.Data;
                }
            }
            return await Task.FromResult<T>(default(T));
        }

    }
}
