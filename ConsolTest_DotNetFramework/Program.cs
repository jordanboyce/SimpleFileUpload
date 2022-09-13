using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsolTest_DotNetFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string res = SaveTest(@"C:\Users\BOYCJT\Pictures\testFile2.docx");
            Console.WriteLine(res);
        }

        public static string SaveTest(string filePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var form = new MultipartFormDataContent();

                    byte[] fileData = File.ReadAllBytes(filePath);

                    ByteArrayContent byteContent = new ByteArrayContent(fileData);

                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                    form.Add(byteContent, "file", Path.GetFileName(filePath));

                    var result = httpClient.PostAsync("https://localhost:7198/api/uploadfile", form).ConfigureAwait(false).GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;

                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        return "Result is null";
                    }
                }
            }
            catch (Exception e)
            {
                return "Exception - " + e.Message;
            }
        }
    }
}
