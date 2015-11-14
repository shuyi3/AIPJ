using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_FP1_001 : SimTemplate //zombiechow
    {

        //    todesröcheln:/ stellt beim feindlichen helden 5 leben wieder her.
        public override void onDeathrattle(Playfield p, Minion m)
        {
            int heal = p.getMinionHeal(5, m.own);

            p.minionGetDamageOrHeal(m.own ? p.playerSecond.ownHero : p.playerFirst.ownHero, -heal);
        }

    }
}