using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_315 : SimTemplate //summoningportal
	{

//    eure diener kosten (2) weniger, aber nicht weniger als (1).
        public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own) p.playerFirst.beschwoerungsportal++;
		}

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own) p.playerFirst.beschwoerungsportal--;
        }


	}
}