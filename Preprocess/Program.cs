using System;
using System.IO;

namespace Preprocess
{
    public class Program
    {
        const string folderPath = "\\gutenberg\\";
        const string outFolderPath = "\\gutenberg_out\\";

        static void Main()
        {
            var MATDPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            
            var inputPath = MATDPath + folderPath;
            var outputPath = MATDPath + outFolderPath;

            var processor = new TextProcessor(inputPath, outputPath);
            processor.Process();

            Console.ReadLine();
        }
    }
}
