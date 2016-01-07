namespace HRSim
{
    using System.Collections.Generic;
    using System;

	public class HashTable
	{
        public Dictionary<float, List<Playfield>> TTable {get; set;}

        public HashTable()
        { 
            TTable = new Dictionary<float, List<Playfield>>();
        }

        public virtual bool addToMap(Playfield state)
        {           
            return false;
        }

        public List<Playfield> getListByKey(float key) 
        {
            List<Playfield> list = null;
            TTable.TryGetValue(key, out list);
            return list;
        }

        public void clearTable()
        {
            TTable.Clear();
        }

        public virtual float getHashkey(Playfield p) 
        {
            return 0f;
        }
	}
}
