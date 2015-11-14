using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_161 : SimTemplate //naturalize
	{

//    vernichtet einen diener. euer gegner zieht 2 karten.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.minionGetDestroyed(target);
            p.drawACard(p.getArandomCardFromDeck(!ownplay), !ownplay);
            p.drawACard(p.getArandomCardFromDeck(!ownplay), !ownplay);
		}

	}
}