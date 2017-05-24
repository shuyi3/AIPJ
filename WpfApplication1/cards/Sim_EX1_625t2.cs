using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_EX1_625t2 : SimTemplate //mindshatter
	{

//    heldenfähigkeit/\nverursacht 3 schaden.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = 3;
            if (ownplay)
            {
                if (p.doublepriest >= 1) dmg *= (2 * p.doublepriest);
            }
            else
            {
                if (p.enemydoublepriest >= 1) dmg *= (2 * p.enemydoublepriest);
            }
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}