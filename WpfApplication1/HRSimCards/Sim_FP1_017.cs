using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_017 : SimTemplate //nerubarweblord
	{

//    diener mit kampfschrei/ kosten (2) mehr.
        public override void onAuraStarts(Playfield p, Minion own)
		{
            p.playerFirst.nerubarweblord++;
		}

        public override void onAuraEnds(Playfield p, Minion m)
        {
            p.playerFirst.nerubarweblord--;
        }


	}
}