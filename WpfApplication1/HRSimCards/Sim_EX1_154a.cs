using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_154a : SimTemplate //wrath
	{

//    fügt einem diener $3 schaden zu.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int damage = 0;
            damage = p.getSpellDamageDamage(3, ownplay);
            p.minionGetDamageOrHeal(target, damage);
        }

	}
}