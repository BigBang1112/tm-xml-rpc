using TmXmlRpc.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TmXmlRpc
{
    public class MasterServerTm2 : MasterServer<RequestGameManiaPlanet>
    {
        public MasterServerTm2(string title) : base(new RequestGameManiaPlanet(title))
        {

        }

        public MasterServerTm2() : this(string.Empty)
        {

        }
    }
}
