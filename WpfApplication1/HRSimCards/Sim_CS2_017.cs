using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_017 : SimTemplate //shapeshift
	{

//    heldenfähigkeit/\n+1 angriff in diesem zug.\n+1 rüstung.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (ownplay)
            {
                p.minionGetTempBuff(p.playerFirst.ownHero, 1, 0);
                p.minionGetArmor(p.playerFirst.ownHero,1);
            }
            else
            {
                p.minionGetTempBuff(p.playerSecond.ownHero, 1, 0);
                p.minionGetArmor(p.playerSecond.ownHero,1);
            }
        }

	}
}