using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_303 : SimTemplate //shadowflame
	{

//    vernichtet einen befreundeten diener und fügt allen feindlichen dienern schaden zu, der seinem angriff entspricht.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int damage1 = p.getSpellDamageDamage(target.Angr, ownplay);

            p.minionGetDestroyed(target);

            p.allMinionOfASideGetDamage(!ownplay, damage1);

		}

	}
}