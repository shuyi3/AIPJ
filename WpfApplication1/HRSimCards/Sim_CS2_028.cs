using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_028 : SimTemplate //* blizzard
	{

//Deal 2 damage to all enemy minions and Freeze them.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.allMinionOfASideGetDamage(!ownplay, dmg, true);
			List<Minion> temp = (ownplay) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            foreach (Minion t in temp)
            {
                t.frozen = true;
            }
		}

	}
}