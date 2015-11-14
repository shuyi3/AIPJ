using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX14_04 : SimTemplate //* purecold
	{

//Deal 8 damage to the enemy hero, and and Freeze it.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(8, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
			target.frozen = true;
        }

	}
}