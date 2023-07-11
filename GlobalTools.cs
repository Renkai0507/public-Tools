

using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Tools
{
    public static class GlobalTools
    {
    /// <summary>
    /// 限制TextBox只能輸入數字
    /// </summary>
    /// <param name="txt"></param>
        public static void Numberonly(TextBox txt)
        {
            var reg = new Regex("^[0-9]*$");
            var str = txt.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                txt.Text = sb.ToString();
                //定义输入焦点在最后一个字符
                txt.SelectionStart = txt.Text.Length;
            }
        }
         /// <summary>
        /// 用KeyPress觸發限制TextBox輸入含小數點的數字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void NumberPointonly(TextBox sender, KeyPressEventArgs e)
        {
            // 允許輸入數字、小數點、退格和刪除鍵
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '\b' && e.KeyChar != 127)
            {
                e.Handled = true; // 禁止輸入
                return;
            }

            // 確保小數點只能輸入一次且在合適的位置
            if (e.KeyChar == '.')
            {
                if (sender.Text.Contains("."))
                {
                    e.Handled = true; // 已經有小數點，禁止輸入
                    return;
                }

                if (sender.Text.Length == 0)
                {
                    sender.Text = "0"; // 若小數點為第一個字符，自動補零
                }
                else if (sender.SelectionStart == 0)
                {
                    sender.Text = "0" + sender.Text; // 若小數點在第一位，自動補零
                }
            }

            // 確保只能輸入到小數點第一位
            if (sender.Text.Contains("."))
            {
                int decimalPlaces = sender.Text.Length - sender.Text.IndexOf(".") - 1;
                if (decimalPlaces >= 1 && sender.SelectionStart > sender.Text.IndexOf("."))
                {
                    if (e.KeyChar != '\b' && e.KeyChar != 127)
                    {
                        e.Handled = true; // 小數點後已有一位數字，禁止輸入更多
                        return;
                    }

                }
            }
            
        }

        public static DataTable LoadCsvFile()
        {
            DataTable dt = new DataTable();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "請選擇匯入檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "files (*.CSV)|";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!IsFileInUse(dialog.FileName))
                {
                    CsvReader cr = new CsvReader(dialog.FileName);
                    dt = cr.csv2dt(dialog.FileName, 0);
                    return dt;
                }
                else
                    MessageBox.Show("檔案已開啟占用中,請關閉該檔案", "檔案占用中",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }


        private void btnCsvOut(DataGridView dgvExcel)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "CSV File(*.csv) | *.csv";
            savefile.FileName = DateTime.Now.Year - 1911 + DateTime.Now.ToString("MMdd");
            if (dgvExcel.RowCount < 1)
            {
                MessageBox.Show("資料列表尚無資料，請先確認檔案");
                return;
            }
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(savefile.FileName, false, System.Text.Encoding.GetEncoding("big5"));
                string header = string.Empty;
                foreach (DataGridViewTextBoxColumn item in dgvExcel.Columns)
                {
                    //header = item.ToString();
                    header += item.HeaderText + ",";
                }
                writer.WriteLine(header);
                foreach (DataGridViewRow item in dgvExcel.Rows)
                {
                    string strRow = string.Empty;
                    foreach (DataGridViewCell cell in item.Cells)
                    {
                        if (cell.Value != null)
                        {
                            strRow += "\"" + cell.Value.ToString() + "\",";
                        }

                    }
                    writer.WriteLine(strRow);
                }
                writer.Close();
            }
        }
        static bool IsFileInUse(string FileName)
        {
            bool isuse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                isuse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return isuse;
        }
        
    }
}
