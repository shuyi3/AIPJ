using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_CS2_094 : PenTemplate //hammerofwrath
	{

//    verursacht $3 schaden. zieht eine karte.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}