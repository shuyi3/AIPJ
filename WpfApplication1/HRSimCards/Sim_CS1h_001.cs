using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS1h_001 : SimTemplate //lesserheal
	{

//    heldenfähigkeit/\nstellt 2 leben wieder her.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int heal = 2;
            if (ownplay)
            {
                if (p.playerFirst.anzOwnAuchenaiSoulpriest >= 1) heal = -heal;
                if (p.playerFirst.doublepriest >= 1) heal *= (2 * p.playerFirst.doublepriest);
            }
            else
            {
                if (p.playerSecond.anzOwnAuchenaiSoulpriest >= 1) heal = -heal;
                if (p.playerSecond.doublepriest >= 1) heal *= (2 * p.playerSecond.doublepriest);
            }
            p.minionGetDamageOrHeal(target, -heal);
            
            
		}

	}
}