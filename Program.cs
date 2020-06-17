using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Console = Colorful.Console; //Colorful Console for better Color formatting

namespace DontPoisonMySource
{
    internal static class Program
    {
        private static void Main( string[] args )
        {
            //Use Unicode encoding to support the arrow symbols :)
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            if ( args.Length == 0 )
            {
                Console.WriteLine( "Usage: " + AppDomain.CurrentDomain.FriendlyName + " file1 file2 ..." );
                Console.ReadKey();
                return;
            }

            foreach ( var t in args )
            {
                if ( !File.Exists( t ) )
                {
                    WriteFormattedOutput( "File not found", t, Color.IndianRed );
                    continue;
                }

                if ( !t.EndsWith( "proj" ) )
                {
                    WriteFormattedOutput( "Unsupported file type", t, Color.IndianRed );
                    continue;
                }

                int count = 0;
                using ( var reader = new StreamReader( t ) )
                {
                    string line;
                    while ( ( line = reader.ReadLine() ) != null )
                    {
                        var m = Regex.Match( line, "<(.*)Exec(.*?)>" );
                        var m1 = Regex.Match( line, "<PreBuildEvent>(.*?)</PreBuildEvent>" );
                        var m2 = Regex.Match( line, "<PostBuildEvent>(.*?)</PostBuildEvent>" );
                        if ( m.Success )
                        {
                            WriteFormattedOutput( "Exec", t, Color.Red );
                            Console.WriteLine( " ┕► " + line );
                            count++;
                        }
                        else if ( m1.Success )
                        {
                            WriteFormattedOutput( "PreBuildEvent", t, Color.Orange );
                            Console.WriteLine( " ┕► " + m1.Groups[1].Value );
                            count++;
                        }
                        else if ( m2.Success )
                        {
                            WriteFormattedOutput( "PostBuildEvent", t, Color.Orange );
                            Console.WriteLine( " ┕► " + m2.Groups[1].Value );
                            count++;
                        }
                    }

                    if ( count != 0 ) continue;

                    const string template = "{0} {1} ► {2}";
                    var output = new Colorful.Formatter[]
                    {
                        new Colorful.Formatter( "[i]", Color.LimeGreen ),
                        new Colorful.Formatter( "nothing found", Color.Green ),
                        new Colorful.Formatter( t, Color.Yellow ),
                    };
                    Console.WriteLineFormatted( template, Color.White, output );
                }
            }
            Console.ReadKey();
        }

        private static void WriteFormattedOutput( string type, string file, Color typeColor )
        {
            const string template = "{0} {1} ► {2}";
            var output = new Colorful.Formatter[]
            {
                new Colorful.Formatter( "[!]", Color.OrangeRed ),
                new Colorful.Formatter( type, typeColor ),
                new Colorful.Formatter( file, Color.Yellow ),
            };
            Console.WriteLineFormatted( template, Color.White, output );
        }
    }
}