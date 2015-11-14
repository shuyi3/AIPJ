using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_027 : SimTemplate //stoneskingargoyle
	{

//    stellt zu beginn eures zuges das volle leben dieses dieners wieder her.

        public override void onTurnStartTrigger(Playfield p, Minion triggerEffectMinion, bool turnStartOfOwner)
        {
            if (triggerEffectMinion.own == turnStartOfOwner)
            {
                int heal = p.getMinionHeal(triggerEffectMinion.maxHp - triggerEffectMinion.Hp, triggerEffectMinion.own);
                p.minionGetDamageOrHeal(triggerEffectMinion, -heal);
            }
        }

	}
}