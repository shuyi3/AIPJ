using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_PlaceholderCard : PenTemplate //placeholdercard
	{

//
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}