using DistWF.Common.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace DistWF.Controller
{
    public class DistWFControllerApp
    {
        private IHttpClientFactory _httpFactory { get; set; }

        public DistWFControllerApp(
            IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<CalculationResponse> Calculate(CalculationRequest request)
        {
            var response = new CalculationResponse();

            var client = _httpFactory.CreateClient("default");
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), UnicodeEncoding.UTF8, "application/json");
            try
            {
                var httpResponse = await client.PostAsync(client.BaseAddress + CalculationAPIMethods.Calculate, httpContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseText = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CalculationResponse>(responseText);
                    response.Success = true;
                }
                else
                {
                    response.Message = $"{Messages.ErrorInvokingService}: {httpResponse.StatusCode} - {httpResponse.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {

                response.Message = $"{Messages.ErrorInvokingService}: {ex.Message}";
            }
            finally
            {
                client.Dispose();
            }

            return response;

        }
    }
}
