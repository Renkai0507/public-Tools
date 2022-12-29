using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace Tools
{
    class CsvReader
    {
        string ConnectionString;
        FileInfo file;
        public DataTable data;

        public CsvReader(string fullpath)
        {
            file = new FileInfo(fullpath);
            ConnectionString = string.Format(
                @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
                file.DirectoryName);
            ReadCsvToTable();
        }
        private void ReadCsvToTable()
        {
            data = new DataTable(file.Name);
            OleDbConnection con = new OleDbConnection(ConnectionString);
            con.Open();
            string Query = string.Format("select * from [{0}]",file.Name);
            OleDbDataAdapter adp = new OleDbDataAdapter(Query,con);
            adp.Fill(data);
        }
    }
}