using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_EX1_402 : SimTemplate //* armorsmith
	{
//    Whenever a friendly minion takes damage, gain 1 Armor.

        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.own)
            {
                for(int i = 0;i< p.tempTrigger.ownMinionsGotDmg;i++)
                {
					p.minionGetArmor(p.ownHero, 1);
                }
            }
            else
            {
                for (int i = 0; i < p.tempTrigger.enemyMinionsGotDmg; i++)
                {
                    p.minionGetArmor(p.enemyHero, 1);
                }
            }
        }

	}
}