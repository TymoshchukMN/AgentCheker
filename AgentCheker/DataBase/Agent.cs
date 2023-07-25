using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentCheker.DataBase
{
    public class Agent
    {
        public string PcName { get; set; }

        public bool IsActiveDC { get; set; }

        public bool IsActiveEset { get; set; }
    }
}
