using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_088 : SimTemplate //guardianofkings
    {

        //    kampfschrei:/ stellt bei eurem helden 6 leben wieder her.
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            int heal = p.getMinionHeal(6, own.own);

            p.minionGetDamageOrHeal(own.own ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
        }


    }
}