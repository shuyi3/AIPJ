using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_004 : SimTemplate //powerwordshield
	{

//    verleiht einem diener +2 leben.\nzieht eine karte.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.minionGetBuffed(target, 0, 2);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}