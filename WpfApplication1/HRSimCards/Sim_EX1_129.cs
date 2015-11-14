using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_129 : SimTemplate //fanofknives
	{

//    fügt allen feindlichen dienern $1 schaden zu. zieht eine karte.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            p.allMinionOfASideGetDamage(!ownplay, dmg);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}