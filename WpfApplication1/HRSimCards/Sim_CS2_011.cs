using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_011 : SimTemplate //savageroar
    {

        //    verleiht euren charakteren +2 angriff in diesem zug.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion t in temp)
            {
                p.minionGetTempBuff(t, 2, 0);
            }
            p.minionGetTempBuff(ownplay ? p.playerFirst.ownHero : p.playerSecond.ownHero, 2, 0);
        }

    }
}