using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_003 : SimTemplate //* Dragon's Breath
	{
		// Deal 4 damage. Costs (1) less for each minion that died this turn.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(4, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
        }
	}
}