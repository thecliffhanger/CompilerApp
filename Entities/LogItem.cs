using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityModel
{
    [DataContract]
    public class LogItem
    {
        [DisplayName("Id")]
        [DataMember]
        public Guid Id { get; set; }        

        [DisplayName("Source")]
        [DataMember]
        public string Source { get; set; }

        [DisplayName("LogType")]
        [DataMember]
        public string LogType { get; set; }

        [DisplayName("Command")]
        [DataMember]        
        public string Command { get; set; }

        [DisplayName("Inputs")]
        [DataMember]
        public string Inputs { get; set; }

        [DisplayName("LogText")]
        [DataMember]
        public string LogText { get; set; }

        [DisplayName("Created")]
        [DataMember]
        public DateTime Created { get; set; }
    }
}
