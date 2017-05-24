using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_CS2_221 : PenTemplate //spitefulsmith
	{

//    wutanfall:/ eure waffe hat +2 angriff.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}