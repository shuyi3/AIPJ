using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_539 : SimTemplate //killcommand
	{

//    verursacht $3 schaden. verursacht stattdessen $5 schaden, wenn ihr ein wildtier besitzt.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (ownplay)
            {
                bool haspet = false;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET)
                    {
                        haspet = true;
                        break;
                    }
                }

                int dmg = p.getSpellDamageDamage(3, ownplay);
                if (haspet) dmg = p.getSpellDamageDamage(5, ownplay);
                p.minionGetDamageOrHeal(target, dmg);
            }
		}

	}
}