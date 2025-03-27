using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Text;



namespace ContactDetails.Test
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        
             public async Task GetAllContactTest()
            {
            //Arrange
            var token = await GetToken();
            //Act
            var response = await GetDataWithTokenAsync("https://localhost:7230/api/Contact", token);
            var contactList = JsonConvert.DeserializeObject<List<Details>>(response);
            //Assert
            Assert.IsTrue(contactList.Count > 0, "Contact list is empty.");
        }

        
        private async Task<string> GetToken()
        {
            TokenPop tok = new TokenPop();
            var data = new { firstName = "Adrick", email = "Aji@gmail.com" };
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7133/api/ContactDat_/Login", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                tok = JsonConvert.DeserializeObject<TokenPop>(result);
            }
            return tok.Token;
        }

        private async Task<string> GetDataWithTokenAsync(string url, string token)
        {
            var httpClient = new HttpClient();
            // Add Authorization header with Bearer token
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Make the GET request
            var response = await httpClient.GetAsync(url);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read and return the response content as string
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the case when the response is not successful
                return $"Error: {response.StatusCode}";
            }
        }
    }
}