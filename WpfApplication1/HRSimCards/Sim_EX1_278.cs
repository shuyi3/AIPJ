using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_278 : SimTemplate //shiv
	{

//    verursacht $1 schaden. zieht eine karte.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}