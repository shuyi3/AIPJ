using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_583 : SimTemplate //priestessofelune
    {

        //    kampfschrei:/ stellt bei eurem helden 4 leben wieder her.
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            int heal = p.getMinionHeal(4, own.own);
            p.minionGetDamageOrHeal(own.own ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
        }


    }
}