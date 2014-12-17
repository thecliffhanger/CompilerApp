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

        [DisplayName("Id")]
        [DataMember]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        [DataMember]
        public string HostName { get; set; }

        [DisplayName("IPAddress")]
        [DataMember]
        public string IPAddress { get; set; }


    }
}
