using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    [DataContract]
    public class Client
    {
        public Client()
        {           
        }

        [DisplayName("Client Id")]
        [DataMember]
        public Guid Id { get; set; }

        [DisplayName("Host Name")]
        [DataMember]
        public string HostName { get; set; }

        [DisplayName("IP Address")]
        [DataMember]
        public string IPAddress { get; set; }


    }
}
