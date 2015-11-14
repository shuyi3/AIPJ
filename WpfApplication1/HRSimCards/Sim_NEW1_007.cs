using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_007 : SimTemplate //starfall
	{

//    wählt aus:/ fügt einem diener $5 schaden zu; oder fügt allen feindlichen dienern $2 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (choice == 1)
            {
                int dmg = p.getSpellDamageDamage(5, ownplay);
                p.minionGetDamageOrHeal(target, dmg);
            }
            if (choice == 2)
            {
                int damage = p.getSpellDamageDamage(2, ownplay);
                p.allMinionOfASideGetDamage(!ownplay, damage);
            }

		}

	}
}