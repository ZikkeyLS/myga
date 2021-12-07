using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MygaCross
{
    public interface IMPAddon
    {
        string error { get; set; }
        bool initialised { get; set; }
        void Intitialize(params object[] parametres);
    }
}
