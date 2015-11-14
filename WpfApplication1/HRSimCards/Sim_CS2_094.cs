using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_094 : SimTemplate //hammerofwrath
	{

//    verursacht $3 schaden. zieht eine karte.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}