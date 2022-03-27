using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input url address: ");
            string urlAddress = Console.ReadLine();
            string htmlCode = GetCode(urlAddress);

            List<string> mailElements = new List<string>(0);
            string pattern = @"^[0-9a-z_-]+@[\w]+\.\w{2,4}$";
            var regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(htmlCode);
            foreach (Match item in matches)
            {
                mailElements.Add(item.Value);
            }

            List<string> phoneNumberElements = new List<string>(0);
            pattern = @"[+\d-]{10, 20}";
            regex = new Regex(pattern);
            matches = regex.Matches(htmlCode);
            foreach (Match item in matches)
            {
                mailElements.Add(item.Value);
            }

            List<string> linkElements = new List<string>(0);
            pattern = @"href='(?<link>\S+)'>(?<text>\S+)</a>";
            regex = new Regex(pattern);
            for (Match m = regex.Match(htmlCode); m.Success; m = m.NextMatch())
            {
                linkElements.Add(m.Groups["link"].ToString());
            }

            var fileInfo = new FileInfo("result.txt");
            using (var fileStream = File.Open(fileInfo.FullName, FileMode.OpenOrCreate))
            {
                var streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine(new string('-', 100));
                streamWriter.WriteLine("Found emails:");
                foreach (var item in mailElements)
                {
                    streamWriter.WriteLine(item);
                }

                streamWriter.WriteLine(new string('-', 100));
                streamWriter.WriteLine("Found phone numbers:");
                foreach (var item in phoneNumberElements)
                {
                    streamWriter.WriteLine(item);
                }

                streamWriter.WriteLine(new string('-', 100));
                streamWriter.WriteLine("Found links:");
                foreach (var item in linkElements)
                {
                    streamWriter.WriteLine(item);
                }

                streamWriter.Close();
            }
        }
        static String GetCode(string urlAddress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);

            Cookie cookie = new Cookie
            {
                Name = "beget",
                Value = "begetok"
            };

            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(urlAddress), cookie);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }

            return data;
        }
    }
}
