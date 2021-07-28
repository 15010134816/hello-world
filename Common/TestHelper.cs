using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TestHelper
    {
        private static TestHelper _instance;
        private static readonly object lockobj = new object();
        private TestHelper()
        {
        }
        public static TestHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new TestHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        public static int total = 10;
        public static Queue<string> queueAll = new Queue<string>();
        private static Queue<string> queueCur = new Queue<string>();
        public string GetTest(string id)
        {
            queueAll.Enqueue(id);
            if (queueAll.Count > total)
            {
                return "商品已抢完";
            }
            queueCur.Enqueue(id);
            if (queueCur.Count == total)
            {
                Task.Run(() =>
                {
                    HandleQueue();
                });
            }
            //var num = Convert.ToInt32(CacheHelper.Get("Num"));
            //CacheHelper.Set("Num", num - 1);
            return "抢到商品编号" + (total - queueCur.Count);
        }
        private void HandleQueue()
        {
            while (queueCur.Count > 0)
            {
                queueCur.Dequeue();
            }
        }
    }
}
