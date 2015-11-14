using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_027p : SimTemplate //* DIE, INSECT!
	{
		// Hero PowerDeal 8 damage to a random enemy.
		
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = 8;
            if (ownplay)
            {
                if (p.playerFirst.doublepriest >= 1) dmg *= (2 * p.playerFirst.doublepriest);
				if (p.playerSecond.ownMinions.Count > 0)
				{
					target = p.searchRandomMinion(p.playerSecond.ownMinions, Playfield.searchmode.searchLowestHP); //damage the lowest (pessimistic variant)
				}
				else
				{
					target = p.playerSecond.ownHero;
				}
            }
            else
            {
                if (p.playerSecond.doublepriest >= 1) dmg *= (2 * p.playerSecond.doublepriest);
				if (p.playerFirst.ownMinions.Count > 0)
				{
					target = p.searchRandomMinion(p.playerFirst.ownMinions, Playfield.searchmode.searchHighestAttack); //damage the Highest (pessimistic variant)
				}
				else
				{
					target = p.playerFirst.ownHero;
				}
            }			
            p.minionGetDamageOrHeal(target, dmg);
        }
	}
}