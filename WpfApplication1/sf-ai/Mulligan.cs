﻿using System;
using System.Collections.Generic;

namespace HREngine.Bots
{

    public class Mulligan
    {
        public class CardIDEntity
        {
            public string id = "";
            public int entitiy = 0;
            public CardIDEntity(string id, int entt)
            {
                this.id = id;
                this.entitiy = entt;
            }
        }

        class mulliitem
        {
            public string cardid = "";
            public string enemyclass = "";
            public string ownclass = "";
            public int howmuch = 2;
            public string[] requiresCard = null;
            public int manarule = -1;
            public string rulestring = "";
            public mulliitem(string id, string own, string enemy, int number, string[] req = null, int mrule = -1)
            {
                this.cardid = id;
                this.ownclass = own;
                this.enemyclass = enemy;
                this.howmuch = number;
                this.requiresCard = req;
                this.manarule = mrule;
            }

            public mulliitem(string all, string id, string own, string enemy, int number, string[] req = null, int mrule = -1)
            {
                this.cardid = id;
                this.ownclass = own;
                this.enemyclass = enemy;
                this.howmuch = number;
                this.requiresCard = req;
                this.manarule = mrule;
                this.rulestring = all;
            }

        }

        class concedeItem
        {
            public HeroEnum urhero = HeroEnum.None;
            public List<HeroEnum> enemhero = new List<HeroEnum>();
        }

        Silverfish sf;

        List<mulliitem> holdlist = new List<mulliitem>();
        List<mulliitem> deletelist = new List<mulliitem>();
        List<concedeItem> concedelist = new List<concedeItem>();
        public bool loserLoserLoser = false;

        //private static Mulligan instance;

        //public static Mulligan Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new Mulligan();
        //        }
        //        return instance;
        //    }
        //}

        public Mulligan(Silverfish sf)
        {
            this.sf = sf;
            readCombos();
        }

        private void readCombos()
        {
            string[] lines = new string[0] { };
            this.holdlist.Clear();
            this.deletelist.Clear();
            try
            {
                string path = sf.Settings.path;
                lines = System.IO.File.ReadAllLines(path + "_mulligan.txt");
            }
            catch
            {
                sf.Helpfunctions.logg("cant find _mulligan.txt");
                sf.Helpfunctions.ErrorLog("cant find _mulligan.txt (if you dont created your own mulliganfile, ignore this message)");
                return;
            }
            sf.Helpfunctions.logg("read _mulligan.txt...");
            sf.Helpfunctions.ErrorLog("read _mulligan.txt...");
            foreach (string line in lines)
            {
                if (line.StartsWith("loser"))
                {
                    this.loserLoserLoser = true;
                    continue;
                }

                if (line.StartsWith("concede:"))
                {
                    try
                    {
                        string ownh = line.Split(':')[1];
                        concedeItem ci = new concedeItem();
                        ci.urhero = sf.Hrtprozis.heroNametoEnum(ownh);
                        string enemlist = line.Split(':')[2];
                        foreach (string s in enemlist.Split(','))
                        {
                            ci.enemhero.Add(sf.Hrtprozis.heroNametoEnum(s));
                        }
                        concedelist.Add(ci);
                    }
                    catch
                    {
                        sf.Helpfunctions.logg("mullimaker cant read: " + line);
                        sf.Helpfunctions.ErrorLog("mullimaker cant read: " + line);
                    }
                    continue;
                }

                if (line.StartsWith("hold;"))
                {
                    try
                    {
                        string ownclass = line.Split(';')[1];
                        string enemyclass = line.Split(';')[2];
                        string cardlist = line.Split(';')[3];
                        foreach (string crd in cardlist.Split(','))
                        {
                            if (crd.Contains(":"))
                            {
                                if ((crd.Split(':')).Length == 3)
                                {
                                    this.holdlist.Add(new mulliitem(line, crd.Split(':')[0], ownclass, enemyclass, Convert.ToInt32(crd.Split(':')[1]), crd.Split(':')[2].Split('/')));
                                }
                                else
                                {
                                    this.holdlist.Add(new mulliitem(line, crd.Split(':')[0], ownclass, enemyclass, Convert.ToInt32(crd.Split(':')[1])));
                                }

                            }
                            else
                            {
                                this.holdlist.Add(new mulliitem(line, crd, ownclass, enemyclass, 2));
                            }
                        }

                        if (line.Split(';').Length == 5)
                        {
                            int manarule = Convert.ToInt32(line.Split(';')[4]);
                            this.holdlist.Add(new mulliitem(line, "#MANARULE", ownclass, enemyclass, 2, null, manarule));
                        }

                    }
                    catch
                    {
                        sf.Helpfunctions.logg("mullimaker cant read: " + line);
                        sf.Helpfunctions.ErrorLog("mullimaker cant read: " + line);
                    }
                }
                else
                {
                    if (line.StartsWith("discard;"))
                    {
                        try
                        {
                            string ownclass = line.Split(';')[1];
                            string enemyclass = line.Split(';')[2];
                            string cardlist = line.Split(';')[3];
                            foreach (string crd in cardlist.Split(','))
                            {
                                if (crd == null || crd == "") continue;
                                this.deletelist.Add(new mulliitem(line, crd, ownclass, enemyclass, 2));
                            }

                            if (line.Split(';').Length == 5)
                            {
                                int manarule = Convert.ToInt32(line.Split(';')[4]);
                                this.deletelist.Add(new mulliitem(line, "#MANARULE", ownclass, enemyclass, 2, null, manarule));
                            }

                        }
                        catch
                        {
                            sf.Helpfunctions.logg("mullimaker cant read: " + line);
                            sf.Helpfunctions.ErrorLog("mullimaker cant read: " + line);
                        }
                    }
                    else
                    {

                    }
                }

            }

        }

        public bool hasmulliganrules(string ownclass, string enemclass)
        {
            if (this.holdlist.Count == 0 && this.deletelist.Count == 0) return false;
            bool hasARule = false;
            foreach (mulliitem mi in this.holdlist)
            {
                if ((mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass)) hasARule = true;
            }
            foreach (mulliitem mi in this.deletelist)
            {
                if ((mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass)) hasARule = true;
            }
            return hasARule;
        }

        public bool hasHoldListRule(string ownclass, string enemclass)
        {
            bool hasARule = false;
            foreach (mulliitem mi in this.holdlist)
            {
                if ((mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass)) hasARule = true;
            }
            return hasARule;
        }

        public List<int> whatShouldIMulligan(List<CardIDEntity> cards, string ownclass, string enemclass)
        {
            List<int> discarditems = new List<int>();
            bool usedManarule = false;
            foreach (mulliitem mi in this.deletelist)
            {
                foreach (CardIDEntity c in cards)
                {
                    if (mi.cardid == "#MANARULE" && (mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass))
                    {
                        usedManarule = true;
                        if (CardDB.Instance.getCardDataFromID(CardDB.Instance.cardIdstringToEnum(c.id)).cost >= mi.manarule)
                        {
                            if (discarditems.Contains(c.entitiy)) continue;
                            sf.Helpfunctions.ErrorLog("discard " + c.id + " because of this rule " + mi.rulestring);
                            discarditems.Add(c.entitiy);
                        }
                        continue;
                    }

                    if (c.id == mi.cardid && (mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass))
                    {
                        if (discarditems.Contains(c.entitiy)) continue;
                        sf.Helpfunctions.ErrorLog("discard " + c.id + " because of this rule " + mi.rulestring);
                        discarditems.Add(c.entitiy);
                    }
                }
            }

            if (holdlist.Count == 0 || !hasHoldListRule(ownclass, enemclass)) return discarditems;

            Dictionary<string, int> holddic = new Dictionary<string, int>();
            foreach (CardIDEntity c in cards)
            {
                bool delete = true;
                foreach (mulliitem mi in this.holdlist)
                {

                    if (mi.cardid == "#MANARULE" && (mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass))
                    {
                        if (CardDB.Instance.getCardDataFromID(CardDB.Instance.cardIdstringToEnum(c.id)).cost <= mi.manarule)
                        {
                            delete = false;
                        }
                        continue;
                    }

                    if (c.id == mi.cardid && (mi.enemyclass == "all" || mi.enemyclass == enemclass) && (mi.ownclass == "all" || mi.ownclass == ownclass))
                    {

                        if (mi.requiresCard == null)
                        {

                            if (holddic.ContainsKey(c.id)) // we are holding one of the cards
                            {
                                if (mi.howmuch == 2)
                                {
                                    delete = false;
                                }
                            }
                            else
                            {
                                delete = false;
                            }
                        }
                        else
                        {
                            bool hasRequirements = false;
                            foreach (CardIDEntity reqs in cards)
                            {
                                foreach (string s in mi.requiresCard)
                                {
                                    if (s == reqs.id)
                                    {
                                        hasRequirements = true;
                                        break;
                                    }
                                }
                            }
                            if (hasRequirements)
                            {
                                if (holddic.ContainsKey(c.id)) // we are holding one of the cards
                                {
                                    if (mi.howmuch == 2)
                                    {
                                        delete = false;
                                    }
                                }
                                else
                                {
                                    delete = false;
                                }
                            }

                        }
                    }
                }

                if (!usedManarule)
                {
                    if (delete)
                    {
                        if (discarditems.Contains(c.entitiy)) continue;
                        discarditems.Add(c.entitiy);
                    }
                    else
                    {
                        discarditems.RemoveAll(x => x == c.entitiy);

                        if (holddic.ContainsKey(c.id))
                        {
                            holddic[c.id]++;
                        }
                        else
                        {
                            holddic.Add(c.id, 1);
                        }
                    }
                }
                else
                {//used manarules in discard line
                    if (!delete)
                    {
                        discarditems.RemoveAll(x => x == c.entitiy);

                        if (holddic.ContainsKey(c.id))
                        {
                            holddic[c.id]++;
                        }
                        else
                        {
                            holddic.Add(c.id, 1);
                        }
                    }
                }

            }

            return discarditems;

        }

        public void setAutoConcede(bool mode)
        {
            this.loserLoserLoser = mode;
        }

        public bool shouldConcede(HeroEnum ownhero, HeroEnum enemHero)
        {

            foreach (concedeItem ci in concedelist)
            {
                if (ci.urhero == ownhero && ci.enemhero.Contains(enemHero)) return true;
            }

            return false;
        }

    }

}