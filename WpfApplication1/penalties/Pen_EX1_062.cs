using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_062 : PenTemplate //oldmurkeye
	{

//    ansturm/. hat +1 angriff für jeden anderen murloc auf dem schlachtfeld.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}