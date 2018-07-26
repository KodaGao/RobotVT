using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FModel
{
    public class SystemMessageInfo
    {
        public string Id { get; set; }
        public string DateTime { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public SystemEnum.MessageType Type { get; set; }

        public string Message { get; set; }
    }
}
