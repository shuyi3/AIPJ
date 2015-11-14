using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_164b : SimTemplate //nourish
	{

//    zieht 3 karten.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}