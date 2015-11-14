using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_053 : SimTemplate //Shieldmaiden
    {

        //   Battlecry:&lt;/b&gt; Gain 5 Armor.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (own.own)
            {
                p.minionGetArmor(p.playerFirst.ownHero, 5);
            }
            else
            {
                p.minionGetArmor(p.playerSecond.ownHero, 5);
            }
        }

    }

}