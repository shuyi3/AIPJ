using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_089 : SimTemplate //Illuminator
    {

        //  if you control a Secret at the end of your turn, restore 4 health to your hero. 

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (turnEndOfOwner == triggerEffectMinion.own)
            {
                if (((turnEndOfOwner) ? p.playerFirst.ownSecretsIDList.Count : p.playerSecond.ownSecretsIDList.Count) >= 1)
                {
                    int heal = p.getMinionHeal(4, turnEndOfOwner);
                    p.minionGetDamageOrHeal(((turnEndOfOwner) ? p.playerFirst.ownHero : p.playerSecond.ownHero), -heal, true);
                }
            }
        }

    }

}