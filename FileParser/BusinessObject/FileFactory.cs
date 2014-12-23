using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FileFactory
    {
        public enum FileTypeAdapter { CsvFileParser, XmlFileParser, XsltFileParser };

        public FileTypeAdapter type;

        IFileParser typeAdapter;

        public FileFactory(FileTypeAdapter type)
        {
            this.type = type;
        }
        /// <summary>
        /// This Methods decides which type parser to create by creating an Abstraction of object creation
        /// </summary>
        /// <returns>IFileParser</returns>
        public IFileParser GetFileAdapter()
        {
            switch(type)
            {
                case FileTypeAdapter.CsvFileParser:
                    typeAdapter = new CsvFileParser();
                    break;
                default:                    
                    break;
 
            }
            return typeAdapter;
        }
    }
}
