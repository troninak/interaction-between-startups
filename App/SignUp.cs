using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class SignUp : Form
    {
        // Настройка окна
        private bool isDragging = false;
        private int mouseX, mouseY;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                mouseX = e.X;
                mouseY = e.Y;
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Left += e.X - mouseX;
                this.Top += e.Y - mouseY;
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        // Основной код

        public SignUp()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MainStart main = new MainStart();
            main.Show();
            this.Close();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            if (email.Text == "" || pass.Text == "" || name.Text == "" || surname.Text == "")
            {
                textError.Text = "Вы заполнили не все поля!";
                return;
            }

            if (isUserExists())
            {
                return;
            }

            DateTime now = DateTime.Now;

            DataBase db = new DataBase();

            DataTable dt = new DataTable();

            MySqlCommand command = new MySqlCommand("INSERT INTO `Users` (`name`, `surname`, `email`, `password`, `role_id`, `registration_date`) VALUES (@name, @surname, @email, @password, @role_id, @registration_date)", db.getConn());

            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email.Text;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = pass.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name.Text;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = surname.Text;
            command.Parameters.Add("@role_id", MySqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@registration_date", MySqlDbType.VarChar).Value = now.ToString("yyyy-MM-dd");

            db.openConn();
            
            if (command.ExecuteNonQuery() == 1)
            {
                // Получаем ID только что зарегистрированного пользователя
                string userId = db.GetCurrentUserId(email.Text, pass.Text);

                int roleId = db.GetRoleById(userId);

                // Создаем экземпляр формы в зависимости от роли пользователя
                switch (roleId) // Роль "Инноватор"
                {
                    case 1: // Innovator
                        MainInnovator mainInnovator = new MainInnovator(userId);
                        mainInnovator.Show();
                        this.Close();
                        break;
                    case 2: // Developer
                        MainDeveloper mainDeveloper = new MainDeveloper(userId);
                        mainDeveloper.Show();
                        this.Close();
                        break;
                    case 3: // Investor
                        MainInvestor mainInvestor = new MainInvestor(userId);
                        mainInvestor.Show();
                        this.Close();
                        break;
                    case 4: // Admin
                        MainAdmin mainAdmin = new MainAdmin(userId);
                        mainAdmin.Show();
                        this.Close();
                        break;
                    default:
                        // Если роль не соответствует ни одной из известных ролей, выводим сообщение об ошибке
                        MessageBox.Show("Неизвестная роль пользователя!");
                        break;
                }

                textError.Text = "";
            }
            else
            {
                MessageBox.Show("Ошибка при создании!");
            }

            db.closeConn();
        }

        public Boolean isUserExists()
        {
            DataBase db = new DataBase();

            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `Users` WHERE `email` = @uE", db.getConn());

            command.Parameters.Add("@uE", MySqlDbType.VarChar).Value = email.Text;

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                textError.Text = "Такой аккаунт уже существует!";
                return true;
            }
            else
            {
                textError.Text = "";
                return false;
            }
        }
    }
}
