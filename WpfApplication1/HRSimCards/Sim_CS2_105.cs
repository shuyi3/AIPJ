using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_105 : SimTemplate //heroicstrike
    {

        //    verleiht eurem helden +4 angriff in diesem zug.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.minionGetTempBuff(ownplay ? p.playerFirst.ownHero : p.playerSecond.ownHero, 4, 0);

        }

    }
}