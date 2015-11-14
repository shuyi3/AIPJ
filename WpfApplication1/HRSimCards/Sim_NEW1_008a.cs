using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_008a : SimTemplate //ancientteachings
	{

//    zieht 2 karten.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
            p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
		}
	}
}