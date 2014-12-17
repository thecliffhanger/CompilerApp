using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("LogType")]
        [DataMember]
        public string LogType { get; set; }

        [DisplayName("Source")]
        [DataMember]
        public string Source { get; set; }

        [DisplayName("LogText")]
        [DataMember]
        public string LogText { get; set; }

        [DisplayName("Created")]
        [DataMember]
        public DateTime Created { get; set; }
    }
}
