using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
	class Sim_BRM_016 : SimTemplate //* Axe Flinger
	{
		// Whenever this minion takes damage, deal 2 damage to the enemy hero
		
        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.anzGotDmg>=1)
            {
                for (int i = 0; i < m.anzGotDmg; i++)
                {
					p.minionGetDamageOrHeal(m.own ? p.enemyHero : p.ownHero, 2);
                }
                m.anzGotDmg = 0;
            }
        }
	}
}