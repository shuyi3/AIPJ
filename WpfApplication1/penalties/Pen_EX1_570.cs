using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_570 : PenTemplate //bite
	{

//    verleiht eurem helden +4 angriff in diesem zug und 4 rüstung.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}