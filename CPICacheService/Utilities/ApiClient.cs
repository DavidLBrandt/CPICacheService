using Newtonsoft.Json;

namespace CPICacheService.Utilities
{
    public class ApiClient
    {
        private readonly HttpClient httpClient;

        public ApiClient()
        {
            httpClient = new HttpClient();
        }

        //public async Task<List<MyObject>> GetMyObjectsAsync(string url)
        //{
        //    HttpResponseMessage response = await httpClient.GetAsync(url);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string json = await response.Content.ReadAsStringAsync();
        //        List<MyObject> myObjects = JsonConvert.DeserializeObject<List<MyObject>>(json);
        //        return myObjects;
        //    }

        //    // Handle error cases
        //    response.EnsureSuccessStatusCode();
        //    return null;
        //}
    }
}
