using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_096 : SimTemplate //loothoarder
	{

//    todesröcheln:/ zieht eine karte.

        public override void onDeathrattle(Playfield p, Minion m)
        {
            p.drawACard(p.getArandomCardFromDeck(m.own), m.own);
        }

	}
}