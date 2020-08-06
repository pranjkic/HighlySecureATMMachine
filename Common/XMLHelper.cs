using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public class XMLHelper
    {
        public static void InsertAllUserToXML(List<User> users, string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
            using (StreamWriter sw = new StreamWriter(path/*, append : true*/))
            {
                xmlSerializer.Serialize(sw, users);
            }
        }

        public static List<User> ReadAllUserFromXML(/*User user*/string path)
        {
            List<User> retList = new List<User>();
            XmlSerializer xmlDeserializer = new XmlSerializer(typeof(List<User>));
            using (StreamReader sr = new StreamReader(path/*, append : true*/))
            {
                retList = (List<User>)xmlDeserializer.Deserialize(sr);
            }
            return retList;
        }
    }
}
