using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_302 : SimTemplate //mortalcoil
	{

//    fügt einem diener $1 schaden zu. zieht eine karte, wenn er dadurch vernichtet wird.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            if (dmg >= target.Hp && !target.divineshild && !target.immune)
            {
                //this.owncarddraw++;
                p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            }
            p.minionGetDamageOrHeal(target, dmg);
            
		}

	}
}