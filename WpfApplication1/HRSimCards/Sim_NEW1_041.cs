using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_041 : SimTemplate//Stampeding Kodo
    {
        //todo list
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {

            List<Minion> temp2 = (own.own) ? new List<Minion>(p.playerSecond.ownMinions) : new List<Minion>(p.playerFirst.ownMinions);
            temp2.Sort((a, b) => a.Hp.CompareTo(b.Hp));//destroys the weakest
            foreach (Minion enemy in temp2)
            {
                if (enemy.Angr <= 2)
                {
                    p.minionGetDestroyed(enemy);
                    break;
                }
            }

        }


    }
}
