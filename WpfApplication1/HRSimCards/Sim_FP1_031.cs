using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_031 : SimTemplate //baronrivendare
	{

//    die todesröcheln/-effekte eurer diener werden 2-mal ausgelöst.
        public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own) p.playerFirst.ownBaronRivendare++;
            else p.playerSecond.ownBaronRivendare++;
		}

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own)
            {
                p.playerFirst.ownBaronRivendare--;
            }
            else
            {
                p.playerSecond.ownBaronRivendare--;
            }
        }

	}
}