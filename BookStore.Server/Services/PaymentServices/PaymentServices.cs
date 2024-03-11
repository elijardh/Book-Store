using System;
using BookStore.Server.Models.ResponseModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BookStore.Server.Services.PaymentServices
{
    public class PaymentServices : IPaymentServices
    {
        public PaymentServices()
        {
        }

        public Task<(int, dynamic)> InitiatePayment()
        {
            throw new NotImplementedException();
        }

        public async Task<(int, dynamic)> ValidatePayment(string reference)
        {
            try
            {
                HttpClient httpClient = new()
                {
                    BaseAddress = new Uri("https://api.flutterwave.com/v3/transactions/"),
                };

                //TODO move bearer token to appsettings.json
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "FLWSECK_TEST-d28b00460564522d46cc948b1d06f71c-X");
                HttpResponseMessage response = await httpClient.GetAsync($"{reference}/verify");
                string responseBody = await response.Content.ReadAsStringAsync();
                FlutterWaveResponse? result = JsonConvert.DeserializeObject<FlutterWaveResponse>(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    if (result == null)
                    {
                        return (0, "Unknown error");
                    }

                    if (result.Status != null && result.Status == "success")
                    {
                        return (1, result);
                    }
                    else
                    {
                        return (0, result);
                    }


                }
                else
                {
                    return (0, result!);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

