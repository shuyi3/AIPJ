using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_625t : SimTemplate //mindspike
	{

//    heldenfähigkeit/\nverursacht 2 schaden.


		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = 2;
            if (ownplay)
            {
                if (p.playerFirst.doublepriest >= 1) dmg *= (2 * p.playerFirst.doublepriest);
            }
            else
            {
                if (p.playerSecond.doublepriest >= 1) dmg *= (2 * p.playerSecond.doublepriest);
            }
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}