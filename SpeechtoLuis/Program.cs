using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

namespace SpeechtoLuis
{
    class Program
    {
        static void Main(string[] args)
        {

            // Read text from Mic
            string text = GetTextFromMicAsync().Result;

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["query"] = text;
            client.BaseAddress = new Uri("https://predict-resource.cognitiveservices.azure.com/luis/prediction/v3.0/apps/7a9d3c22-f348-4930-b7bb-dbda2fc53ad5/slots/production/predict?verbose=true&show-all-intents=true&log=true&subscription-key=a39fa7c14b4d45e7a3942f99aeb57c88&");

            var preEndpointUri = String.Format("https://predict-resource.cognitiveservices.azure.com/luis/prediction/v3.0/apps/7a9d3c22-f348-4930-b7bb-dbda2fc53ad5/slots/production/predict?verbose=true&show-all-intents=true&log=true&subscription-key=a39fa7c14b4d45e7a3942f99aeb57c88&{0}", queryString);
            HttpResponseMessage response = client.GetAsync(preEndpointUri).Result;

            // Read Intent
            //var response = GetIntents("a39fa7c14b4d45e7a3942f99aeb57c88", "https://predict-resource.cognitiveservices.azure.com/", "7a9d3c22-f348-4930-b7bb-dbda2fc53ad5", text);

            Console.WriteLine(response);
        }

        private static async Task<string> GetTextFromMicAsync()
        {
            var config = SpeechConfig.FromSubscription("0b9d77f9ac4e40bb9b90bed5696c83ef", "westus");
            using (var recognizer = new IntentRecognizer(config))
            {
                Console.WriteLine("Speak now...");
                var result = await recognizer.RecognizeOnceAsync();
                if(result.Reason == ResultReason.RecognizedSpeech)
                {
                    return result.Text;
                }
                else
                {
                    return "Speech couldn't be recognized.";
                }
            }
        }

        private static async Task<string> GetIntents(string predictionKey, string predictionEndpoint, string appId, string text)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            try
            {
                //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", predictionKey);
                queryString["query"] = text;
                //var preEndpointUri = string.Format("{0}luis/prediction/v3.0/apps/{1}/slots/production/predict?{2}", predictionEndpoint, appId, queryString);
                var preEndpointUri = String.Format("https://predict-resource.cognitiveservices.azure.com/luis/prediction/v3.0/apps/7a9d3c22-f348-4930-b7bb-dbda2fc53ad5/slots/production/predict?verbose=true&show-all-intents=true&log=true&subscription-key=a39fa7c14b4d45e7a3942f99aeb57c88&{0}", queryString);
                var response = await client.GetAsync(preEndpointUri);
                var res = await response.Content.ReadAsStringAsync();

                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
