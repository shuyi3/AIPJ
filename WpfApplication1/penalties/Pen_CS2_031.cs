using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_CS2_031 : PenTemplate //icelance
	{

//    friert/ einen charakter ein. wenn er bereits eingefroren/ ist, werden ihm stattdessen $4 schaden zugefügt.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}