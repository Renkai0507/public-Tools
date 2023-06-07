using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBService.DB
{
    public class BasicDb<T,Y>
    {
        protected string conn;
         IDbConnection db;
        public BasicDb()
        {

        }
        protected void Connect()
        {
            db = new SqlConnection(conn);
        }
        protected List<T> ExecSQL(string SQLstr)
        {
            if (db.State == ConnectionState.Closed) db.Open();            
            var dbresult = db.Query<T>(SQLstr, commandType: CommandType.Text);
            db.Close();
            return dbresult.ToList();
        }
        protected List<T> ConectSQL(string SQLstr, string[] Codes)
        {
            if (db.State == ConnectionState.Closed) db.Open();
            var dbresult = db.Query<T>(SQLstr, new { Codes }, commandType: CommandType.Text);
            db.Close();
            return dbresult.ToList();
        }
        protected List<T> SearchSQLKey(string SQLstr, Y key)
        {
            if (db.State == ConnectionState.Closed) db.Open();
            var dbresult = db.Query<T>(SQLstr, key, commandTimeout: 90, commandType: CommandType.Text);
            db.Close();
            return dbresult.ToList();
        }
        /// <summary>
        /// 傳入SQL字傳執行
        /// </summary>
        /// <param name="執行SQL指令"></param>
        /// <param name="代入的物件"></param>
        /// <returns>返回SQL執行結果</returns>
        protected List<T> ConectSQL(string SQLstr, T entity)
        {           
            if (db.State == ConnectionState.Closed) db.Open();
            var dbresult = db.Query<T>(SQLstr, entity, commandType: CommandType.Text);
            db.Close();
            return dbresult.ToList();
        }
        protected List<T> ConectSQL(string SQLstr, DynamicParameters Dp)
        {
            if (db.State == ConnectionState.Closed) db.Open();
            var dbresult = db.Query<T>(SQLstr, Dp, commandType: CommandType.Text);
            db.Close();
            return dbresult.ToList();
        }
        protected List<T> ConectSQL(string SQLstr, List<T> entity)
        {
            if (db.State == ConnectionState.Closed) db.Open();
            var dbresult = db.Query<T>(SQLstr, entity, commandType: CommandType.Text);
            db.Close();
            return dbresult.ToList();
        }
         protected int ExecSQL(string SQLstr, T entity)
        {
            IDbConnection db = new SqlConnection(conn);
            if (db.State == ConnectionState.Closed) db.Open();
            var result = db.Execute(SQLstr, entity, commandTimeout: 180, commandType: CommandType.Text);
            db.Close();
            return result;
        }
         protected List<string> GetStrs(string SQLstr)
        {
            IDbConnection db = new SqlConnection(conn);
            if (db.State == ConnectionState.Closed) db.Open();
            var result = db.Query<string>(SQLstr, commandTimeout: 180, commandType: CommandType.Text);
            db.Close();
            return result.ToList();
        }
    }
}
