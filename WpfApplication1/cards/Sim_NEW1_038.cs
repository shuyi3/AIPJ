using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_NEW1_038 : SimTemplate//Gruul
    {
        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            p.minionGetBuffed(triggerEffectMinion, 1, 1);
        }
        

    }
}
