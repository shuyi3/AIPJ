using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_PRO_001 : PenTemplate //elitetaurenchieftain
	{

//    kampfschrei:/ verleiht beiden spielern die macht des rock! (durch eine powerakkordkarte)
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}