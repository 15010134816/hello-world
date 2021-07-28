using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 秒杀帮助类
    /// </summary>
    public class SeckillHelper
    {
        private static SeckillHelper _instance = null;
        private static readonly object lockobj = new object();
        private static Dictionary<string, int> stockDic = new Dictionary<string, int>();
        private SeckillHelper()
        {
            GetStock();
        }

        private static void GetStock()
        {
            var ds = DbHelperSQL.GetDataSet("Q_Get_Stock", new SqlParameter[] {
            });
            var dt = ds.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                stockDic.Add(item["goodsId"].ToString(), Convert.ToInt32(item["num"]));
            }
        }

        public static SeckillHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new SeckillHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        public int Seckill(string userId, int goodsId)
        {
            if (stockDic[goodsId.ToString()] == 0)
            {
                return -1;
            }
            var result = DbHelperSQL.ExecuteSqlPro("U_Save_Seckill", new SqlParameter[] {
                new SqlParameter("userId",userId),
                new SqlParameter("goodsId",goodsId)
            });
            return result;
        }
        public void ResetStock()
        {
            GetStock();
        }
    }
}
