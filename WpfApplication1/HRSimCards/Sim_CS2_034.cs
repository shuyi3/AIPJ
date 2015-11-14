using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_034 : SimTemplate //fireblast
	{

//    heldenfähigkeit/\nverursacht 1 schaden.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = 1;
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