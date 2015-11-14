using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_154b : SimTemplate //wrath
	{

//    fügt einem diener $1 schaden zu. zieht eine karte.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int damage = p.getSpellDamageDamage(1, ownplay);
            //this.owncarddraw++;

            p.minionGetDamageOrHeal(target, damage);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
        }

	}
}