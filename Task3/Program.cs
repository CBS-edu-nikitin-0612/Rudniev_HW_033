using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input full file name: ");
            string fileName = Console.ReadLine();
            FileInfo file = new FileInfo(fileName);

            string fileContent = "";
            if (file.Exists)
            {
                using (var fileStream = File.Open(file.FullName, FileMode.Open))
                {
                    var streamReader = new StreamReader(fileStream);
                    fileContent = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            else
            {
                Console.WriteLine("File not found!");
            }
            fileContent = fileContent.ToLower();

            Regex.Replace(fileContent, @"on|for|of|to|at|in|about|against|before|concerning|including|depending|granted|alongside|outside|within|wherewith|because of|instead of|for the sake of|with regard to", "GAV!");

            using (var fileStream = File.Open(file.FullName, FileMode.Open))
            {
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(fileContent);
                streamWriter.Close();
            }
        }
    }
}
