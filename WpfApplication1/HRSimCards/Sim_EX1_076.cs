using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_076 : SimTemplate //pintsizedsummoner
	{

        //todo enemy stuff
//    der erste diener, den ihr in einem zug ausspielt, kostet (1) weniger.
        public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own) p.playerFirst.winzigebeschwoererin++;
		}

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own) p.playerFirst.winzigebeschwoererin--;
        }

	}
}