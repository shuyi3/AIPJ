using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_BRM_027p : SimTemplate //* DIE, INSECT!
	{
		// Hero PowerDeal 8 damage to a random enemy.
		
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = 8;
            if (ownplay)
            {
                if (p.doublepriest >= 1) dmg *= (2 * p.doublepriest);
				if (p.enemyMinions.Count > 0)
				{
					target = p.searchRandomMinion(p.enemyMinions, Playfield.searchmode.searchLowestHP); //damage the lowest (pessimistic variant)
				}
				else
				{
					target = p.enemyHero;
				}
            }
            else
            {
                if (p.enemydoublepriest >= 1) dmg *= (2 * p.enemydoublepriest);
				if (p.ownMinions.Count > 0)
				{
					target = p.searchRandomMinion(p.ownMinions, Playfield.searchmode.searchHighestAttack); //damage the Highest (pessimistic variant)
				}
				else
				{
					target = p.ownHero;
				}
            }			
            p.minionGetDamageOrHeal(target, dmg);
        }
	}
}