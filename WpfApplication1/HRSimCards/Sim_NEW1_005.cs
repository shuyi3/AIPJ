using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_005 : SimTemplate //kidnapper
	{

//    combo:/ lasst einen diener auf die hand seines besitzers zurückkehren.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                if (p.playerFirst.cardsPlayedThisTurn >= 1) p.minionReturnToHand(target, target.own, 0);
            }
            else
            {
                if (p.playerSecond.cardsPlayedThisTurn >= 1) p.minionReturnToHand(target, target.own, 0);
            }

		}


	}
}