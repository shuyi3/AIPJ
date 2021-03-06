using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_606 : SimTemplate //shieldblock
	{

//    erhaltet 5 rüstung. zieht eine karte.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (ownplay)
            {
                p.minionGetArmor(p.playerFirst.ownHero, 5);
            }
            else
            {
                p.minionGetArmor(p.playerSecond.ownHero, 5);
            }
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}