using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_013 : SimTemplate //wildgrowth
	{

//    erhaltet einen leeren manakristall.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (ownplay)
            {
                if (p.playerFirst.ownMaxMana < 10)
                {
                    p.playerFirst.ownMaxMana++;
                }
                else
                {
                    p.drawACard(CardDB.cardName.excessmana, true, true);
                }

            }
            else
            {
                if (p.playerSecond.ownMaxMana < 10)
                {
                    p.playerSecond.ownMaxMana++;
                }
                else
                {
                    p.drawACard(CardDB.cardName.excessmana, false, true);
                }
            }
		}

	}
}