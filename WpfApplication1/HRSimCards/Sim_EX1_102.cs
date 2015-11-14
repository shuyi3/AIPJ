using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_102 : SimTemplate //demolisher
    {

        //    fügt zu beginn eures zuges einem zufälligen feind 2 schaden zu.

        public override void onTurnStartTrigger(Playfield p, Minion triggerEffectMinion, bool turnStartOfOwner)
        {
            if (triggerEffectMinion.own == turnStartOfOwner)
            {
                List<Minion> temp2 = (turnStartOfOwner) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
                bool dmgdone = false;
                foreach (Minion mins in temp2)
                {
                    p.minionGetDamageOrHeal(mins, 2);
                    dmgdone = true;
                    break;
                }
                if (!dmgdone)
                {
                    p.minionGetDamageOrHeal(turnStartOfOwner ? p.playerSecond.ownHero : p.playerFirst.ownHero, 2);
                };
            }
        }

    }
}