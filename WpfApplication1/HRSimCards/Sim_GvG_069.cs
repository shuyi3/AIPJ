using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_069 : SimTemplate //Antique Healbot
    {

        //   Battlecry: Restore 8 Health to your hero.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (own.own)
            {
                int heal = p.getMinionHeal(8, own.own);
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, -heal, true);
            }
            else
            {
                int heal = p.getMinionHeal(8, own.own);
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, -heal, true);
            }
        }

    }

}