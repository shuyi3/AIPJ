using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_EX1_590 : PenTemplate //bloodknight
	{

//    kampfschrei:/ alle diener verlieren gottesschild/. erhält +3/+3 für jeden verlorenen schild.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}