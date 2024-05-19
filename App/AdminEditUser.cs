﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace App
{
    public partial class AdminEditUser : Form
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

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                mouseX = e.X;
                mouseY = e.Y;
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Left += e.X - mouseX;
                this.Top += e.Y - mouseY;
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        // Основной код
        private DataBase db = new DataBase();

        private DataGridView dataGridView; // Поле для хранения ссылки на dataGridView

        private string userId;
        private string selectedPhotoPath = "";

        public AdminEditUser(DataGridView dataGridView, string userId, string name, string surname, string email, string password, string roleId, string registrationDate)
        {
            InitializeComponent();

            this.dataGridView = dataGridView; // Сохраняем ссылку на dataGridView

            this.userId = userId;

            // Устанавливаем текстовые поля на форме с текущими данными пользователя
            textName.Text = name;
            textSurname.Text = surname;
            textEmail.Text = email;
            textPassword.Text = password;
            textRole.Text = roleId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedPhotoPath = openFileDialog.FileName;
                textPath.Text = selectedPhotoPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем новые данные из текстовых полей
                string newName = textName.Text;
                string newSurname = textSurname.Text;
                string newEmail = textEmail.Text;
                string newPassword = textPassword.Text;
                string newRoleId = textRole.Text;
                string newPhotoPath = selectedPhotoPath; // Путь к фотографии

                // Передаем новые данные в метод сохранения данных пользователя
                db.SaveUserData(userId, newName, newSurname, newEmail, newPassword, newRoleId, newPhotoPath);

                //MessageBox.Show("Данные пользователя успешно изменены.");
                this.DialogResult = DialogResult.OK;

                // Обновляем данные в dataGridView после сохранения изменений
                ((AdminPanel)dataGridView.FindForm()).LoadUserData();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
