using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_596 : SimTemplate //demonfire
	{

//    fügt einem diener $2 schaden zu. wenn das ziel ein verbündeter dämon ist, erhält er stattdessen +2/+2.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (target.handcard.card.race == 15 && ownplay == target.own)
            {
                p.minionGetBuffed(target, 2, 2);
            }
            else
            {
                int dmg = p.getSpellDamageDamage(2, ownplay);
                p.minionGetDamageOrHeal(target, dmg);
            }
        }


	}
}