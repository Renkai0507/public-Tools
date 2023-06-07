

using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Tools
{
    public static class GlobalTools
    {
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
        
    }
}
