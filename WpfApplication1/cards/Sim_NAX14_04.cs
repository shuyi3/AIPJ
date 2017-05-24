using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_NAX14_04 : SimTemplate //* purecold
	{

//Deal 8 damage to the enemy hero, and and Freeze it.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = (ownplay) ? p.getSpellDamageDamage(8) : p.getEnemySpellDamageDamage(8);
            p.minionGetDamageOrHeal(target, dmg);
			target.frozen = true;
        }

	}
}