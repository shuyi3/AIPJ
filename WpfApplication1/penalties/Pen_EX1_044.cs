using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_044 : PenTemplate //questingadventurer
	{

//    erhält jedes mal +1/+1, wenn ihr eine karte ausspielt.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}