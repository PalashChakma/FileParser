using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace BusinessObject
{
    class CsvFileParser:IFileParser
    {
        private Hashtable hashFirstName = null;
        private Hashtable hashLastName = null;

        private Hashtable hashAddressNo = null;
        private Hashtable hashAddressString = null;

        private SortedList<String, String> sortedListAddress = null;

        String destpath = string.Empty;

        public void ParseFile(string filepath)
        {

            try
            {
                StreamReader sr = new StreamReader(filepath);

                int index = filepath.LastIndexOf(@"\");
                destpath = filepath.Substring(0, index + 1);

                //delete old output files if already exists
                DeleteOldOutputFiles();

                string line = string.Empty;
                string[] words;

                hashFirstName = new Hashtable();
                hashLastName = new Hashtable();

                hashAddressNo = new Hashtable();
                hashAddressString = new Hashtable();

                sortedListAddress = new SortedList<string, String>();
                //ignore first line
                sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
                {
                    words = line.Split(new char[] { ',' });
                    BuildHashTable(words[0].Trim(), words[1].Trim());
                    BuildAddressHashtable(words[2]);
                }


                Dictionary<String, int> dictionaryLastName = HashtableToDictionary<String, int>(hashLastName);
                IOrderedEnumerable<KeyValuePair<String, int>> orderedLastName = dictionaryLastName.OrderByDescending(v => v.Value).ThenBy(k => k.Key);

                BuildStringToWrite(orderedLastName, 1);

                Dictionary<String, int> dictionaryFirstName = HashtableToDictionary<String, int>(hashFirstName);
                IOrderedEnumerable<KeyValuePair<String, int>> orderedFirstName = dictionaryFirstName.OrderByDescending(v => v.Value).ThenBy(k => k.Key);

                BuildStringToWrite(orderedFirstName, 2);

                //Print Sorted Address
                PrintSortedAddress(sortedListAddress);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        private void BuildHashTable(string firstname, string lastname)
        {
            var obj_firstname = (object)firstname;
            var obj_lastname = (object)lastname;
            var init = 1;
            try
            {
                //Build for first name
                if (!hashFirstName.ContainsKey(obj_firstname))
                {
                    hashFirstName.Add(obj_firstname, init);
                }
                else
                {
                    Int32 count = 0;
                    count = Int32.Parse(hashFirstName[obj_firstname].ToString());
                    count = count + init;
                    hashFirstName[obj_firstname] = count;
                }
                //Build for last name
                if (!hashLastName.ContainsKey(obj_lastname))
                {
                    hashLastName.Add(obj_lastname, init);
                }
                else
                {
                    Int32 count = 0;
                    count = Int32.Parse(hashLastName[obj_lastname].ToString());
                    count = count + init;
                    hashLastName[obj_lastname] = count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
        {
            return table
                .Cast<DictionaryEntry>()
                .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
        }

        private void BuildStringToWrite(IOrderedEnumerable<KeyValuePair<String, int>> orderedName, int heading)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (heading == 1)
                {
                    sb.Append("FirstName" + "    " + "Frequency");
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("LastName" + "    " + "Frequency");
                    sb.Append(Environment.NewLine);
                }

                foreach (KeyValuePair<String, int> val in orderedName)
                {
                    int value = val.Value;
                    string key = val.Key;
                    //Build string

                    sb.Append(key + "   " + value.ToString());
                    sb.Append(Environment.NewLine);
                }
                WriteToFile(sb, 1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void BuildAddressHashtable(string val)
        {
            try
            {
                String addrString = Regex.Replace(val, @"[\d-]", string.Empty);
                sortedListAddress.Add(addrString, val);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PrintSortedAddress(SortedList<String, String> sortedListAddress)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Address");
                sb.Append(Environment.NewLine);
                foreach (KeyValuePair<String, String> val in sortedListAddress)
                {
                    sb.Append(val.Value);
                    sb.Append(Environment.NewLine);
                }
                WriteToFile(sb, 2);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private void WriteToFile(StringBuilder sb, int outputtype)
        {
            try
            {
                StreamWriter sw = null;
                if (outputtype == 1)
                {
                    sw = new StreamWriter(destpath + "output1.csv", true);
                }
                if (outputtype == 2)
                {
                    sw = new StreamWriter(destpath + "output2.csv", true);
                }
                sw.Write(sb);
                sw.Flush();
                sb.Clear();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void DeleteOldOutputFiles()
        {
            try
            {
                //if already exist delete output files
                if (File.Exists(destpath + "output1.csv"))
                {
                    File.Delete(destpath + "output1.csv");
                }
                if (File.Exists(destpath + "output2.csv"))
                {
                    File.Delete(destpath + "output2.csv");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
       
    }
}
