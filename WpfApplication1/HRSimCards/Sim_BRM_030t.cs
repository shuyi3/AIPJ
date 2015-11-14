using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_BRM_030t : SimTemplate //Tail Swipe
	{

//    verursacht $4 schaden. werft eine zufällige karte ab.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(4, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            
		}

	}
}