using System;
using System.Linq;
using System.Xml;
using CodeWalker.GameFiles;

namespace CWExporter 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // check that arguments were supplied
            if (args.Length == 0)
            {
                Console.WriteLine("No files supplied. Please supply a file by dragging a compatible file onto the exe.");
                Console.WriteLine("Valid File Types are:");
                Console.WriteLine("ydd, ytd, yft, ydr, ybn");
                Console.WriteLine("Please ensure you are using one of the above file types or an XML of the same intended type.");
                
                // press any button to continue prompt
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                
                return;
            }
            // take filename as argument
            Console.WriteLine(args.Length);
            Array.ForEach(args, Console.WriteLine);
            string fileName = args[0];
            
            // check if file is xml
            if (fileName.EndsWith(".xml"))
            {
                Console.WriteLine("Hello World! The file type of this file is: xml");
                byte[] fileBytes = ConvertFromXml(fileName, GetFileExtensionOfXml(fileName));
                // save byte array to binary file
                File.WriteAllBytes(fileName.Replace(".xml", ""), fileBytes);
                Console.WriteLine("File saved to: " + fileName.Replace(".xml", ""));
                // press any button to continue prompt
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

            }
            // check if file is in filesTypes enum
            else if (Enum.IsDefined(typeof(FileTypes), fileName))
            {
                ConvertToXml(fileName, GetFileExtensionOfXml(fileName), fileName + ".xml");
                Console.WriteLine("File saved to: " + fileName + ".xml");
                // press any button to continue prompt
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            
            
            
        }
        
        private static FileTypes GetFileExtensionOfXml(string fileName)
        {
            // get the second file extenstion from the filename
            string[] fileStringArr = fileName.Split('.');
            string fileExtension = fileStringArr[fileStringArr.Length - 2];
            // convert to enum
            FileTypes fileType = (FileTypes)Enum.Parse(typeof(FileTypes), fileExtension);
            return fileType;
        }
        
        private enum FileTypes
        {
            ydd,
            ytd,
            yft,
            ydr,
            ybn
        }
        
        private static byte[] ConvertFromXml(string fileName, FileTypes fileType)
        {
            // load xml file
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            
            // switch based on file type
            switch (fileType)
            {
                case FileTypes.ydd:
                    YddFile yddFile = XmlYdd.GetYdd(xmlDocument);
                    return yddFile.Save();
                case FileTypes.ytd:
                    YtdFile ytdFile = XmlYtd.GetYtd(xmlDocument);
                    return ytdFile.Save();

                case FileTypes.yft:
                    YftFile yftFile = XmlYft.GetYft(xmlDocument);
                    return yftFile.Save();

                case FileTypes.ydr:
                    YdrFile ydrFile = XmlYdr.GetYdr(xmlDocument);
                    return ydrFile.Save();

                case FileTypes.ybn:
                    YbnFile ybnFile = XmlYbn.GetYbn(xmlDocument);
                    return ybnFile.Save();

                default:
                    throw new Exception(nameof(fileType) + " is not a valid file type! what the fuck did you do to fuck up this badly?!");

            }
        }
        
        private static void ConvertToXml(string fileName, FileTypes fileType, string outputFilePath)
        {
            
            byte[] rawFileBytes = File.ReadAllBytes(fileName);
            
            // switch based on file type to convert to xml
            switch (fileType)  
            {
                case FileTypes.ydd:
                    XmlDocument yddXml = new XmlDocument();
                    YddFile yddFile = new YddFile();
                    yddFile.Load(rawFileBytes);
                    yddXml.LoadXml(YddXml.GetXml(yddFile));
                    yddXml.Save(outputFilePath);
                    break;
                case FileTypes.ybn:
                    XmlDocument ybnXml = new XmlDocument();
                    YbnFile ybnFile = new YbnFile();
                    ybnFile.Load(rawFileBytes);
                    ybnXml.LoadXml(YbnXml.GetXml(ybnFile));
                    ybnXml.Save(outputFilePath);
                    break;
                case FileTypes.ydr:
                    XmlDocument ydrXml = new XmlDocument();
                    YdrFile ydrFile = new YdrFile();
                    ydrFile.Load(rawFileBytes);
                    ydrXml.LoadXml(YdrXml.GetXml(ydrFile));
                    ydrXml.Save(outputFilePath);
                    break;
                case FileTypes.yft:
                    XmlDocument yftXml = new XmlDocument();
                    YftFile yftFile = new YftFile();
                    yftFile.Load(rawFileBytes);
                    yftXml.LoadXml(YftXml.GetXml(yftFile));
                    yftXml.Save(outputFilePath);
                    break;
                case FileTypes.ytd:
                    XmlDocument ytdXml = new XmlDocument();
                    YtdFile ytdFile = new YtdFile();
                    ytdFile.Load(rawFileBytes);
                    ytdXml.LoadXml(YtdXml.GetXml(ytdFile));
                    ytdXml.Save(outputFilePath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, "You fucked it up somehow, tell me what the fuck you did to get here!!!");
            }
        }
        
    }
}