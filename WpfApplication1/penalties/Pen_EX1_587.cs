using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_587 : PenTemplate //windspeaker
	{

//    kampfschrei:/ verleiht einem befreundeten diener windzorn/.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}