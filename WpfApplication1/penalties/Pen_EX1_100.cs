using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_100 : PenTemplate //lorewalkercho
	{

//    wenn ein spieler einen zauber wirkt, erhält der andere spieler eine kopie desselben auf seine hand.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}