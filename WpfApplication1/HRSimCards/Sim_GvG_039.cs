using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_039 : SimTemplate //Vitality Totem
    {

        //    At the end of your turn, restore 4 Health to your hero.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (triggerEffectMinion.own == turnEndOfOwner)
            {
                
                if (triggerEffectMinion.own)
                {
                    int heal = p.getMinionHeal(4, triggerEffectMinion.own);
                    p.minionGetDamageOrHeal(p.playerFirst.ownHero, -heal, true);
                }
                else
                {
                    int heal = p.getMinionHeal(4, triggerEffectMinion.own);
                    p.minionGetDamageOrHeal(p.playerSecond.ownHero, -heal, true);
                }

            }
        }


    }

}