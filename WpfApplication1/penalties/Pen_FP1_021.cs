using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_FP1_021 : PenTemplate //deathsbite
	{

//    todesröcheln:/ fügt allen dienern 1 schaden zu.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}