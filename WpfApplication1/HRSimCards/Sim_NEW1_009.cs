using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_009 : SimTemplate //healingtotem
	{

//    stellt am ende eures zuges bei allen befreundeten dienern 1 leben wieder her.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (triggerEffectMinion.own == turnEndOfOwner)
            {
                int heal =  p.getMinionHeal(1, triggerEffectMinion.own);
                p.allMinionOfASideGetDamage(turnEndOfOwner, -heal);
            }
        }

	}
}