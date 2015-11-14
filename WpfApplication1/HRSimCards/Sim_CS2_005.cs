using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_005 : SimTemplate //claw
	{

//    verleiht eurem helden +2 angriff in diesem zug und 2 rüstung.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (ownplay)
            {
                p.minionGetArmor(p.playerFirst.ownHero, 2);
                p.minionGetTempBuff(p.playerFirst.ownHero, 2, 0);
            }
            else
            {
                p.minionGetArmor(p.playerSecond.ownHero, 2);
                p.minionGetTempBuff(p.playerSecond.ownHero, 2, 0);
            }
		}

	}
}