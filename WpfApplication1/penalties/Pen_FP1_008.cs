using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Pen_FP1_008 : PenTemplate //spectralknight
	{

//    kann nicht als ziel von zaubern oder heldenfähigkeiten gewählt werden.
		public override int getPlayPenalty(Playfield p, Minion m, Minion target, int choice, bool isLethal)
		{
		return 0;
		}

	}
}