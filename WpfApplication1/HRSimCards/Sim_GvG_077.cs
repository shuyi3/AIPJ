using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_077 : SimTemplate //Anima Golem
    {

        //  At the end of each turn, destroy this minion if it's your only one. 

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (triggerEffectMinion.own)
            {
                if (p.playerFirst.ownMinions.Count == 1)
                {
                    p.minionGetDestroyed(triggerEffectMinion);
                }
            }
            else
            {
                if (p.playerSecond.ownMinions.Count == 1)
                {
                    p.minionGetDestroyed(triggerEffectMinion);
                }
            }
        }


    }

}