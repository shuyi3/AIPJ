using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_613 : SimTemplate//edwin van cleefe
    {
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {

            if(own.own) p.minionGetBuffed(own, p.playerFirst.cardsPlayedThisTurn * 2, p.playerFirst.cardsPlayedThisTurn * 2);
            else p.minionGetBuffed(own, p.playerSecond.owncards.Count * 2, p.playerSecond.owncards.Count * 2);
        }

    }
}
