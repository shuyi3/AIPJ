using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_029 : SimTemplate //dancingswords
	{

//    todesröcheln:/ euer gegner zieht eine karte.

        public override void onDeathrattle(Playfield p, Minion m)
        {
            p.drawACard(p.getArandomCardFromDeck(!m.own), !m.own);
        }

	}
}