using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Console = Colorful.Console; //Colorful Console for better Color formatting

namespace DontPoisonMySource
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Use Unicode encoding to support the arrow symbols :)
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " file1 file2 ...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (!File.Exists(args[i]))
                {
                    WriteFormattedOutput("File not found", args[i], Color.IndianRed);
                    Console.ReadKey();
                    continue;
                }

                if (!args[i].EndsWith("proj"))
                {
                    WriteFormattedOutput("Unsupported file type", args[i], Color.IndianRed);
                    Console.ReadKey();
                    continue;
                }

                int count = 0;

                using (StreamReader reader = new StreamReader(args[i]))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match m = Regex.Match(line, "<(.*)Exec(.*?)>");
                        Match m1 = Regex.Match(line, "<PreBuildEvent>(.*?)</PreBuildEvent>");
                        Match m2 = Regex.Match(line, "<PostBuildEvent>(.*?)</PostBuildEvent>");
                        if (m.Success)
                        {
                            WriteFormattedOutput("Exec", args[i], Color.Red);
                            Console.WriteLine(" ┕► " + line);
                            count++;
                        }
                        else if (m1.Success)
                        {
                            WriteFormattedOutput("PreBuildEvent", args[i], Color.Orange);
                            Console.WriteLine(" ┕► " + m1.Groups[1].Value);
                            count++;
                        }
                        else if (m2.Success)
                        {
                            WriteFormattedOutput("PostBuildEvent", args[i], Color.Orange);
                            Console.WriteLine(" ┕► " + m2.Groups[1].Value);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        string template = "{0} {1} ► {2}";
                        Colorful.Formatter[] output = new Colorful.Formatter[]
                        {
                                new Colorful.Formatter("[i]", Color.LimeGreen),
                                new Colorful.Formatter("nothing found", Color.Green),
                                new Colorful.Formatter(args[i], Color.Yellow),
                        };
                        Console.WriteLineFormatted(template, Color.White, output);
                    }

                    Console.ReadKey();
                }
            }
        }

        private static void WriteFormattedOutput(string type, string file, Color typeColor)
        {
            string template = "{0} {1} ► {2}";
            Colorful.Formatter[] output = new Colorful.Formatter[]
            {
                                new Colorful.Formatter("[!]", Color.OrangeRed),
                                new Colorful.Formatter(type, typeColor),
                                new Colorful.Formatter(file, Color.Yellow),
            };
            Console.WriteLineFormatted(template, Color.White, output);
        }
    }
}