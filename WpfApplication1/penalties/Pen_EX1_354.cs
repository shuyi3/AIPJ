using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_354 : PenTemplate //layonhands
	{

//    stellt #8 leben wieder her. zieht 3 karten.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}