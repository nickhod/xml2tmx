using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xml2Tmx.Models;
using Xml2Tmx.Writers;

namespace Xml2Tmx
{
    public class ConvertToTmxCommand : ConsoleCommand
    {
        private const int Success = 0;
        private const int Failure = 2;

        public string InputFile { get; set; }
        public string OutputFile { get; set; }

        private TmxWriter tmxWriter;

        public ConvertToTmxCommand()
        {
            // Register the actual command with a simple (optional) description.
            IsCommand("ConvertToTmx", "Convert a valid XML file to a TMD or TMC file");

            // Add a longer description for the help on that specific command.
            //HasLongDescription("This can be used to quickly read a file's contents " +
            //"while optionally stripping out the ',' character.");

            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("i|input=", "The full path of the input file.", i => InputFile = i);

            HasOption("o|output=", "The full path of the output file.", o => OutputFile = o);

            // Optional options/flags, append ':' to obtain an optional value, or null if not specified.
            //HasOption("s|strip:", "Strips ',' from the file before writing to output.",
            //    t => StripCommaCharacter = t == null ? true : Convert.ToBoolean(t));

            tmxWriter = new TmxWriter();
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (File.Exists(InputFile))
                {
                    var tmxFile = tmxWriter.WriteTmxDocument(InputFile);
                    File.WriteAllText(OutputFile, tmxFile);
                }
                else
                {
                    Console.Error.WriteLine("Input file not found");
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return 0;
        }
    }


}
