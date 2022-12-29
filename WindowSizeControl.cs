using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tools
{
    class WindowSizeControl
    {
        Form frm;
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;  // 是否已設定各控制的尺寸資料到Tag屬性

        public WindowSizeControl(Form frm)
        {
            this.frm = frm;
            frm.Load += delegate { Loadfrm(); };
            frm.Resize += delegate { ResizeSetCon(); };
        }

        private void ResizeSetCon()
        {
            float newx = (frm.Width) / X;
            float newy = (frm.Height) / Y;
            SetControls(newx, newy, frm);
        }

        private void Loadfrm()
        {
            X = frm.Width;//獲取窗體的寬度
            Y = frm.Height;//獲取窗體的高度
            isLoaded = true;// 已設定各控制項的尺寸到Tag屬性中
            SetTag(frm);//調用方法
        }
        private void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }
        private void SetControls(float newx, float newy, Control cons)
        {
            if (isLoaded)
            {
                //遍歷窗體中的控制項，重新設置控制項的值
                foreach (Control con in cons.Controls)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//獲取控制項的Tag屬性值，並分割後存儲字元串數組
                    float a = System.Convert.ToSingle(mytag[0]) * newx;//根據窗體縮放比例確定控制項的值，寬度
                    con.Width = (int)a;//寬度
                    a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                    con.Height = (int)(a);
                    a = System.Convert.ToSingle(mytag[2]) * newx;//左邊距離
                    con.Left = (int)(a);
                    a = System.Convert.ToSingle(mytag[3]) * newy;//上邊緣距離
                    con.Top = (int)(a);
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字體大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
            }
        }
    }
}
