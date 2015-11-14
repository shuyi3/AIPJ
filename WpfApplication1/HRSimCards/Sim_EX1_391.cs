using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_391 : SimTemplate //slam
	{

//    fügt einem diener $2 schaden zu. zieht eine karte, wenn er überlebt.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{

            int dmg = p.getSpellDamageDamage(2, ownplay);
            if (target.Hp > dmg || target.immune || target.divineshild)
            {
                //this.owncarddraw++;
                p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            }
            p.minionGetDamageOrHeal(target, dmg);
            
		}

	}
}