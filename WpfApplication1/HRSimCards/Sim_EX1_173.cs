using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_173 : SimTemplate //starfire
	{

//    verursacht $5 schaden. zieht eine karte.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(5, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            //this.owncarddraw++;
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}