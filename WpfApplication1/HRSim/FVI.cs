using System;


namespace HRSim
{
    public class FittedValueIter
    {
        bool isInit = false;
        //ValueLearningAgent valueIterator;
        int numEpisodes = 1000;

        public FittedValueIter()
        {
            //this.valueIterator = new ValueLearningAgent();
        }

        public void Init()
        {

            if (GameManager.Instance == null)
            {
                GameManager.Init();
            }
            else
            {
                GameManager.Instance.initPlayField();
            }

            GameManager.Instance.setPlayer(0, new LearningPlayer(0, GameManager.Instance.mPlayfield));
            GameManager.Instance.setPlayer(1, new LearningPlayer(1, GameManager.Instance.mPlayfield)); // we can try set to the same player
            isInit = true;

        }

        public void valueIteration()
        {
            for (int i = 0; i < numEpisodes; ++i)
            {
               
                int moveNum = 0;
                int firstWon = 0;
                int secondWon = 0;
                {
                    if (!isInit) Init();
                    moveNum++;
                    Helpfunctions.Instance.logg("movenum = " + moveNum);

                    int result = GameManager.Instance.playMove();
                    while (result == -1)
                    {
                        moveNum++;
                        Helpfunctions.Instance.logg("movenum = " + moveNum);
                        result = GameManager.Instance.playMove();
                    }
                    if (result == 0)
                    {
                        firstWon++;
                    }
                    else if (result == 1)
                    {
                        secondWon++;
                    }
                    else
                    {//for debuging
                        break;
                    }
                    isInit = false;
                    Helpfunctions.Instance.logg("PLAYER 1 WON " + firstWon + " GAMES");
                    Helpfunctions.Instance.logg("PLAYER 2 WON " + secondWon + " GAMES");
                    GC.Collect();
                }
                Helpfunctions.Instance.logg("PLAYER 1 WON " + firstWon + " GAMES");
                Helpfunctions.Instance.logg("PLAYER 2 WON " + secondWon + " GAMES");
            }


        }
    }
}
