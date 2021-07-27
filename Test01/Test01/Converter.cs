using Lexipun.DotNetFramework.DataProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test01.Test01.Model;

namespace Test01.Test01
{
    public class Converter
    {
        private const char _comma = ',';
        private const char _slesh = '/';
        private const char _backSlesh = '\\';
        private const int _notFound = -1;
        public int SkipRows { get; set; }
        public Converter()
        {
            SkipRows = 1;
        }

        public void CheckOnCorrectWaayToFile(in string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path is empty eigther null");
            }

            int indexOfLastSlash = path.LastIndexOf(_slesh);

            if (indexOfLastSlash == _notFound)
            {
                indexOfLastSlash = path.LastIndexOf(_backSlesh);

                if (indexOfLastSlash == _notFound)
                {
                    throw new ArgumentException("incorrect path");
                }

            }

            if (!Directory.Exists(path.Substring(0, indexOfLastSlash)))
            {
                throw new ArgumentException("incorrect way to file");
            }

            if (!File.Exists(path))
            {
                throw new ArgumentException("incorrect name of file");
            }
        }

        public void ParceCSV(in List<Product> products, in string path)
        {
            CheckOnCorrectWaayToFile(path);

            PropertyProcessing<Product> propertyProcessing;
            bool skip;

            string[] temp;

            using(StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    skip = false;

                    Product product = new Product();
                    propertyProcessing = new PropertyProcessing<Product>(ref product);

                    temp = reader.ReadLine().Split(_comma);

                    for(int i =0; i < temp.Length; ++i)
                    {
                        propertyProcessing.MoveNext();
                        if (!propertyProcessing.SetCurrent(temp[i]))
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (!skip)
                    {
                        lock (products)
                        {
                            products.Add(product);
                        }
                    }
                }
            }


        }
    }
}
