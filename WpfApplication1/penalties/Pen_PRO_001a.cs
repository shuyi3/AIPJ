using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_PRO_001a : PenTemplate //iammurloc
	{

//    ruft drei, vier oder fünf murlocs herbei (1/1).
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}