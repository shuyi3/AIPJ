using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_FP1_006 : PenTemplate //deathcharger
	{

//    ansturm. todesröcheln:/ fügt eurem helden 3 schaden zu.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}