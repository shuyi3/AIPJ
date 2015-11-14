using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_169 : SimTemplate //innervate
	{

//    erhaltet 2 manakristalle nur für diesen zug.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.playerFirst.mana = Math.Min(p.playerFirst.mana + 2, 10);
		}

	}
}