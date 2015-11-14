using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_031 : SimTemplate //icelance
	{

//    friert/ einen charakter ein. wenn er bereits eingefroren/ ist, werden ihm stattdessen $4 schaden zugefügt.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(4, ownplay);

            
            if (target.frozen)
            {
                p.minionGetDamageOrHeal(target, dmg);
            }
            else
            {
                target.frozen = true;
            }

            
		}

	}
}