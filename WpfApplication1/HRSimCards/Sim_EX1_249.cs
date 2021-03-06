using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_249 : SimTemplate //barongeddon
	{

//    fügt am ende eures zuges allen anderen charakteren 2 schaden zu.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (turnEndOfOwner == triggerEffectMinion.own)
            {
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (triggerEffectMinion.entitiyID != m.entitiyID) p.minionGetDamageOrHeal(m, 2);
                }
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (triggerEffectMinion.entitiyID != m.entitiyID) p.minionGetDamageOrHeal(m, 2);
                }
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, 2);
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, 2);
            }
        }

	}
}