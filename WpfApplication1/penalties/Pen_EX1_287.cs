using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_287 : PenTemplate //counterspell
	{

//    geheimnis:/ wenn euer gegner einen zauber wirkt, kontert/ ihr ihn.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}