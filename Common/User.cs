using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public class User
    {
        [XmlIgnore]
        public X509Certificate2 Certificate { get; set; }
        public string Name { get; set; }
        public byte[] PIN { get; set; }
        public double Cash { get; set; }
        public string OrganizationalUnit { get; set; }
        public DateTime TimeOfAdding { get; set; }
    }
}
