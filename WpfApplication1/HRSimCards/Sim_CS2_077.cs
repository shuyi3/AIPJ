using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_077 : SimTemplate //sprint
	{

//    zieht 4 karten.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}