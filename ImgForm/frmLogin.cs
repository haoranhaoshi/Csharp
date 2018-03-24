using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace OTW
{
    public partial class frmLisLogin : Form 
    {
        public frmLisLogin()
        {
            InitializeComponent();
        }
        public frmLisLogin(string[] args)
        {
            InitializeComponent();
            Args = args;
        }
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(SystemTime st);
        [DllImport("Kernel32.dll")]
        public static extern void SetLocalTime(SystemTime st);

        //定义变量,句柄类型
        public IntPtr han;
        /// <summary>
        /// 是否显示登录窗体
        /// </summary>
        public bool IsShowed
        {
            get;
            set;
        }
        /// <summary>
        /// 外部调用加载指定页面参数
        /// </summary>
        private string[] Args = null;
        /// <summary>
        /// 标识用户是否手工切换至非CA模式登录,若用户手工切换至非CA模式登录,则该属性将被置为True
        /// </summary>
        private bool IsHasChangedToUnuseCAMode
        {
            get;
            set;
        }



        private void frmLisLogin_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.FromArgb(0, 192, 192);
            this.BackColor = this.TransparencyKey;
            this.FormBorderStyle = FormBorderStyle.None;

            this.IsShowed = true;
            this.Hide();
            if (Program.LoadData(true) == 1)
            {
                this.UpdateFtp();
                try
                {
                    using (OTW.BizLogic.MenuLogic dLogic = new OTW.BizLogic.MenuLogic())
                    {
                        DateTime dt = dLogic.GetDateTimeFromSysDateTime();
                        SystemTime st = new SystemTime();
                        st.wYear = (ushort)dt.Year;
                        st.wMonth = (ushort)dt.Month;
                        st.wDay = (ushort)dt.Day;
                        st.wHour = (ushort)dt.Hour;
                        st.wMinute = (ushort)dt.Minute;
                        st.wSecond = (ushort)dt.Second;
                        SetLocalTime(st);
                    }
                }
                catch { }
                if (Args == null || Args.Length == 0)
                {
                    //if (Program.LoadData(true) == 1)
                    //{
                    #region CA
                    this.txtLoginName.Enabled = true;   //解锁用户名框
                    #endregion

                    this.Show();//窗体可见 窗体可见之后 赋焦点才起作用
                    try
                    {
                        lblDBType.Text = OTW.FrameWork.StaticModel.HdisInfo.DicLisSetting["DBTestType"].ToString();
                    }
                    catch
                    {
                        lblDBType.Text = string.Empty;
                    }
                    this.txtLoginName.Focus();
                    this.txtLoginName.SelectAll();
                    //}
                    //else
                    //{
                    //    this.IsShowed = false;
                    //}
                }
                else
                {
                    this.LoginMain(Args);
                }
            }
            else
            {
                this.IsShowed = false;
            }
        }

        private void LoginMain()
        {
            int i = OTW.BizLogic.PersonLogic.GetLoginPerson(this.txtLoginName.Text, this.txtPassword.Text,true);
            if (i < 0)
            {
                this.lalMessage.Visible = true;
                this.txtLoginName.Focus();
                this.txtLoginName.SelectAll();
                return;
            }
           
            this.Hide();
            this.Cursor = Cursors.AppStarting;
            ////获取WPF EXE的进程ID,让主窗体关闭时能也关闭WPF进程 
            //int ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            //OTW.Program.ProcessId = ProcessId;

            if (OTW.Program.MainForm == null)
            {
                OTW.Program.MainForm = new OTW.frmMain();
                //OTW.Program.MainForm = new OTW.frmMainNew();//fanshy
            }

            OTW.Program.MainForm.InitMenu(OTW.FrameWork.StaticModel.HdisInfo.LoginId);
            if (OTW.Program.IsTestDB)
            {
                OTW.Program.MainForm.Text = OTW.Program.MainForm.Text + " - 测试库";
            }

            OTW.Program.MainForm.Show();
        }
        private void LoginMain(string[] args)
        {
            int i = OTW.BizLogic.PersonLogic.GetLoginPerson("opbarcode", true);
            if (i < 0)
            {
                this.IsShowed = false;
                return;
            }
            this.Hide();
            OTW.FrameWork.StaticModel.HdisInfo.LoginId = args[1];
            OTW.FrameWork.StaticModel.HdisInfo.LoginName = args[2];

            this.Cursor = Cursors.AppStarting;
            ////获取WPF EXE的进程ID,让主窗体关闭时能也关闭WPF进程
            //int ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            //OTW.Program.ProcessId = ProcessId;
            OTW.Program.MainForm = new OTW.frmMain(args);
            //OTW.Program.MainForm = new OTW.frmMainNew(args);//fanshy

            OTW.Program.MainForm.InitMenu("999999");
            if (OTW.Program.IsTestDB)
            {
                OTW.Program.MainForm.Text = OTW.Program.MainForm.Text + " - 测试库";
            }

            OTW.Program.MainForm.Show();
            //this.Hide();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.LoginMain();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==Keys.Enter)
            {
                this.LoginMain();
            }
        }

        private void txtLoginName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtPassword.Focus();
                this.txtPassword.SelectAll();
            }
        }

        private void txtLoginName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.lalMessage.Visible = false;
            }
            catch
            {
            }
        }

        private void ShowLisGo()
        {
            frmLisGo lisGo = new frmLisGo();
            lisGo.Show();
        }
        //反向更新FTP EXE
        private void UpdateFtp()
        {

        }

        /// <summary>
        /// 当处于活动状态时 用户名位置获得焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLisLogin_Activated(object sender, EventArgs e)
        {
            this.txtLoginName.Focus();
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLisLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// 切换登录模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoginMode_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "是否切换为非CA登录模式? 使用非CA登录模式将被记录.", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.None);
            if (dr == DialogResult.Yes)
            {
                OTW.FrameWork.StaticModel.HdisInfo.SetSwitch("IsUseCALogin", false);  //屏蔽CA认证登录
                OTW.FrameWork.StaticModel.HdisInfo.SetSwitch("IsUseCAApproving", false);  //屏蔽CA签名
                //执行非延迟CA校验模式的逻辑
                if (!OTW.FrameWork.StaticModel.HdisInfo.GetSwitch("IsDelayCheckedCardMode"))
                {
                    this.txtLoginName.Enabled = true;   //释放锁住的用户名输入框
                }
                this.IsHasChangedToUnuseCAMode = true;    //对用户自行切换非CA模式的行为进行标记
            }
        }

        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 30);
            this.Region = new Region(FormPath);

        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            //SetWindowRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //SetWindowRegion();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            int WM_SYSCOMMAND = 0x0112;
            int SC_MOVE = 0xF010;
            int HTCAPTION = 0x0002;

            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
}
