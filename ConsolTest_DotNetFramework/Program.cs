using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace ConsolTest_DotNetFramework
{
  internal class Program
  {
    static void Main(string[] args)
    {
      string res = SaveTest(@"c:\temp\NewSimResults.txt");
      Console.WriteLine(res);
    }

    public static string SaveTest(string filePath)
    {
      try
      {
        using (FileStream solveStream = File.Open(filePath, FileMode.Open))
        {
          using (var httpClient = new HttpClient())
          {
            httpClient.Timeout = TimeSpan.FromHours(6);
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:7198/api/uploadfile")) 
            {
              var multipartContent = new MultipartFormDataContent();
              multipartContent.Add(new StreamContent(solveStream), "file");
              request.Content = multipartContent;

              var task = Task.Run(() => httpClient.SendAsync(request));
              var response = task.Result;

              if (response.IsSuccessStatusCode)
              {
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                //Testing
                string responseString = responseContent.ReadAsStringAsync().Result;
                return responseString;
              }
              else
              {
                return "Error" + response.StatusCode.ToString();
              }
            }
          }
        }
        return "";
      }
      catch (Exception e)
      {
        return "Exception - " + e.Message;
      }
    }
  }
}
