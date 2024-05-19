using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class AdminPanel : Form
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

        private string userId;

        public AdminPanel(string userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainAdmin mainAdmin = new MainAdmin(userId);
            mainAdmin.Show();
            this.Close();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            LoadUserData();
            LoadIdeaData();
        }

        // Загрузка информации из базы данных для пользователя
        public void LoadUserData()
        {
            try
            {
                // Очищаем dataGridView перед загрузкой новых данных
                dataGridViewUser.Rows.Clear();

                // Получаем данные из базы данных с помощью метода класса DataBase
                DataTable userData = db.GetUserData();

                // Установка фиксированной высоты
                dataGridViewUser.RowTemplate.Height = 40;

                //foreach (DataGridViewColumn column in dataGridView.Columns)
                //{
                //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //}

                // Привязываем данные к dataGridView и добавляем кнопки удаления для каждой строки
                foreach (DataRow row in userData.Rows)
                {
                    DateTime registrationDate = Convert.ToDateTime(row["registration_date"]);
                    string formattedDate = registrationDate.ToString("yyyy-MM-dd"); // Форматируем дату без времени

                    // Получаем путь к фотографии пользователя
                    string photoPath = row.IsNull("photo_path") ? string.Empty : row["photo_path"].ToString();

                    int index = dataGridViewUser.Rows.Add(
                        row["user_id"].ToString(),
                        row["name"].ToString(),
                        row["surname"].ToString(),
                        row["email"].ToString(),
                        row["password"].ToString(),
                        row["role_id"].ToString(),
                        formattedDate, // Используем отформатированную дату
                        photoPath // Путь к фотографии пользователя
                    );

                    // Создаем и добавляем кнопку удаления для текущей строки
                    DataGridViewButtonCell deleteButtonCell = new DataGridViewButtonCell();
                    deleteButtonCell.Value = "Удалить";
                    dataGridViewUser.Rows[index].Cells["delete_button"] = deleteButtonCell;

                    // Создаем и добавляем кнопку редактирования для текущей строки
                    DataGridViewButtonCell editButtonCell = new DataGridViewButtonCell();
                    editButtonCell.Value = "Редактировать";
                    dataGridViewUser.Rows[index].Cells["edit_button"] = editButtonCell;
                }

                // Автоматически настраиваем ширину столбцов
                dataGridViewUser.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        // Загрузка информации из базы данных для идей
        public void LoadIdeaData()
        {
            try
            {
                // Очищаем dataGridView перед загрузкой новых данных
                dataGridViewIdea.Rows.Clear();

                // Получаем данные из базы данных с помощью метода класса DataBase
                DataTable ideaData = db.GetIdeaData();

                // Установка фиксированной высоты
                dataGridViewIdea.RowTemplate.Height = 40;

                // Привязываем данные к dataGridView и добавляем кнопки удаления и редактирования для каждой строки
                foreach (DataRow row in ideaData.Rows)
                {
                    DateTime creationDate = Convert.ToDateTime(row["creation_date"]);
                    string formattedDate = creationDate.ToString("yyyy-MM-dd"); // Форматируем дату без времени

                    int index = dataGridViewIdea.Rows.Add(
                        row["idea_id"].ToString(),
                        row["user_id"].ToString(),
                        row["idea_title"].ToString(),
                        row["idea_description"].ToString(),
                        formattedDate, // Используем отформатированную дату
                        row["status"].ToString()
                    );

                    // Создаем и добавляем кнопку удаления для текущей строки
                    DataGridViewButtonCell deleteButtonCell = new DataGridViewButtonCell();
                    deleteButtonCell.Value = "Удалить";
                    dataGridViewIdea.Rows[index].Cells["delete_button_idea"] = deleteButtonCell;

                    // Создаем и добавляем кнопку редактирования для текущей строки
                    DataGridViewButtonCell editButtonCell = new DataGridViewButtonCell();
                    editButtonCell.Value = "Редактировать";
                    dataGridViewIdea.Rows[index].Cells["edit_button_idea"] = editButtonCell;
                }

                // Автоматически настраиваем ширину столбцов
                dataGridViewIdea.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        // Метод для удаления идеи
        private void DeleteIdea(string ideaId)
        {
            try
            {
                db.DeleteIdeaById(ideaId);
                MessageBox.Show("Идея успешно удалена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении идеи: " + ex.Message);
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadIdeaData();
        }

        private void dataGridViewIdea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что кликнули не на заголовок и не на пустую строку
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;

                // Если кликнули по столбцу с кнопкой удаления
                if (dgv.Columns[e.ColumnIndex].Name == "delete_button_idea")
                {
                    // Получаем ID идеи из строки
                    string ideaId = dgv.Rows[e.RowIndex].Cells["idea_id"].Value.ToString();

                    // Удаляем запись из базы данных
                    DeleteIdea(ideaId);

                    // Обновляем данные в DataGridView
                    LoadIdeaData();
                }

                // Если кликнули по столбцу с кнопкой редактирования
                if (dgv.Columns[e.ColumnIndex].Name == "edit_button_idea")
                {
                    // Получаем данные из строки
                    string ideaId = dgv.Rows[e.RowIndex].Cells["idea_id"].Value.ToString();
                    string ideaTitle = dgv.Rows[e.RowIndex].Cells["idea_title"].Value.ToString();
                    string ideaDescription = dgv.Rows[e.RowIndex].Cells["idea_description"].Value.ToString();
                    string ideaStatus = dgv.Rows[e.RowIndex].Cells["status"].Value.ToString();

                    // Открываем форму редактирования и передаем данные
                    AdminEditIdea adminEditIdea = new AdminEditIdea(userId, ideaId, ideaTitle, ideaDescription, ideaStatus);
                    adminEditIdea.Show();

                    // Обновляем данные в DataGridView после редактирования
                    LoadIdeaData();
                }
            }
        }

        private void btnIdeaAdd_Click(object sender, EventArgs e)
        {
            MainInnovatorAdd mainInnovatorAdd = new MainInnovatorAdd(userId);
            mainInnovatorAdd.Show();
        }

        // Обработчик события для удаления
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажата кнопка в столбце delete_button и строка не заголовок
            if (e.ColumnIndex == dataGridViewUser.Columns["delete_button"].Index && e.RowIndex >= 0)
            {
                // Получаем user_id выбранной строки
                string userId = dataGridViewUser.Rows[e.RowIndex].Cells["user_id"].Value.ToString();

                // Выполняем удаление пользователя с данным user_id
                bool success = db.DeleteUser(userId);

                if (success)
                {
                    // Если удаление прошло успешно, обновляем данные в dataGridView
                    LoadUserData();
                    MessageBox.Show("Пользователь удален успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении пользователя.");
                }
            }

            // Проверяем, была ли нажата кнопка "Редактировать"
            if (e.ColumnIndex == dataGridViewUser.Columns["edit_button"].Index && e.RowIndex >= 0)
            {
                // Получаем данные выбранной строки
                string userId = dataGridViewUser.Rows[e.RowIndex].Cells["user_id"].Value.ToString();
                string name = dataGridViewUser.Rows[e.RowIndex].Cells["name"].Value.ToString();
                string surname = dataGridViewUser.Rows[e.RowIndex].Cells["surname"].Value.ToString();
                string email = dataGridViewUser.Rows[e.RowIndex].Cells["email"].Value.ToString();
                string password = dataGridViewUser.Rows[e.RowIndex].Cells["password"].Value.ToString();
                string roleId = dataGridViewUser.Rows[e.RowIndex].Cells["role_id"].Value.ToString();
                string registrationDate = dataGridViewUser.Rows[e.RowIndex].Cells["registration_date"].Value.ToString();

                // Создаем и открываем форму редактирования с передачей данных выбранного пользователя
                AdminEditUser editForm = new AdminEditUser(dataGridViewUser, userId, name, surname, email, password, roleId, registrationDate);
                editForm.Show();
            }
        }
    }
}
