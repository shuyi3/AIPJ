using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_013t : SimTemplate //excessmana
	{

//    zieht eine karte. i&gt;(ihr könnt nur 10 mana in eurer leiste haben.)/i&gt;
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}