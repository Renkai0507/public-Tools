using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaccInv.ApiWork;
using VaccInv.Models;
using VaccInv.Models.Jsn;

namespace VaccInv.Tools
{
    class ApiService
    {
        public VaccTrans rcvtran;
        public List<VaccTransDetail> rcvlogs;
        public List<IVacInv> GetOnlineInv()
        {
            JsnInvPost jsnpost = new JsnInvPost();
            jsnpost.AgencyCode = GlobalClass.AgencyCode;
            jsnpost.CheckCode = GlobalClass.CheckCode;
            jsnpost.Timestamp = DateTime.Now.ToString();
            InvApiwork work = new InvApiwork();
            if (work.GetIVacInv(jsnpost))
            {
                return work.IVacinvs;
            }
            else
                return null;
        }

        internal bool UpldVacc(VaccTrans tran, List<VaccTransDetail> log, string NeworDel)
        {
            JsnUpldpost post = DbmodelToPost(tran, log);            
            post.DataStatus = NeworDel;
            UpldApiwork worker = new UpldApiwork();
            bool UpdateOK = worker.UpldRcd(post);//上傳成功或失敗
            rcvtran = tran;
            rcvlogs = log;
            rcvtran.StatusCode = worker.Vacctranslog.StatusCode;
            rcvtran.StatusMsg = worker.Vacctranslog.StatusMsg;
            worker.Vacctranslog.CsmnBalcRcd.ForEach(X =>
            {
                rcvlogs.Where(Y => Y.Bath_No == X.BatchID).Single().StatusCode = X.StatusCode;
                rcvlogs.Where(Y => Y.Bath_No == X.BatchID).Single().StatusMsg = X.StatusMsg;
            });
            if (UpdateOK)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tran">上傳主紀錄</param>
        /// <param name="log">疫苗狀態</param>       
        /// <returns> 回傳PostJson格式</returns>
        JsnUpldpost DbmodelToPost(VaccTrans tran, List<VaccTransDetail> log)
        {
            List<VaccRecord> records = new List<VaccRecord>();
            log.ForEach(X =>
            {
                VaccRecord rcrd = new VaccRecord()
                {
                    VaccID = X.ECode.Trim(),
                    BatchID = X.Bath_No,
                    BalanceQty = X.AfterQty,
                    ConsumeQty = X.TransQty,
                    
                    ReserveQty = X.MinQty,
                };
                records.Add(rcrd);
            });
            JsnUpldpost post = new JsnUpldpost()
            {
                AgencyCode = GlobalClass.AgencyCode,
                DataKey = tran.PKID.ToString(),
                CheckCode = GlobalClass.CheckCode,
                CsmnBalcRcd = records,
                Timestamp = tran.UpdateTime.ToString()
            };
            return post;
        }
    }
}
