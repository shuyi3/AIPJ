using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_164b : PenTemplate //nourish
	{

//    zieht 3 karten.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}