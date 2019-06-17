using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMB.FileFormats;

namespace OpenMB.Utilties.MapExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OpenMB Map exporter v1.0.0");
            Console.WriteLine("Export M&B Map to xml format");
            Console.WriteLine("Usage:");
            Console.WriteLine("\t<M&B map.txt full path> <Save full path>");
            if (args.Length == 2)
            {
                string mapTxt = args[0];
                string mapXml = args[1];

                MBWorldMap worldmap = new MBWorldMap();
                worldmap.ParseTxt(mapTxt);
                worldmap.SaveAsXml(mapXml);
                Console.WriteLine("Finish!");
            }
            else
            {
                Console.WriteLine("Invalid parameter number! Exiting.......");
            }
            Console.Read();
        }
    }
}
