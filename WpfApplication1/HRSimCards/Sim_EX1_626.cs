using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_626 : SimTemplate //massdispel
	{

//    bringt alle feindlichen diener zum schweigen/. zieht eine karte.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.allMinionsGetSilenced(!ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}