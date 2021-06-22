using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBang1112.TmXmlRpc
{
    public class MasterServerTmTurbo : MasterServer<RequestGameTmTurbo>
    {
        public MasterServerTmTurbo() : base(new RequestGameTmTurbo())
        {

        }
    }
}
