using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_279 : PenTemplate //pyroblast
	{

//    verursacht $10 schaden.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}