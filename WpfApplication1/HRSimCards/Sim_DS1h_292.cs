using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_DS1h_292 : SimTemplate //steadyshot
	{

//    heldenfähigkeit/\nfügt dem feindlichen helden 2 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = 2;
            if (ownplay)
            {
                if (p.playerFirst.doublepriest >= 1) dmg *= (2 * p.playerFirst.doublepriest);
                p.minionGetDamageOrHeal(target, dmg);
            }
            else
            {
                if (p.playerSecond.doublepriest >= 1) dmg *= (2 * p.playerSecond.doublepriest);
                p.minionGetDamageOrHeal(target, dmg);
            }

		}

	}
}