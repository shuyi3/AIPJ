using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_339 : SimTemplate //thoughtsteal
	{

//    kopiert 2 karten aus dem deck eures gegners und fügt sie eurer hand hinzu.
        //TODO: steal instead of draw
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay, true);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay, true);
		}

	}
}