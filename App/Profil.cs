using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace App
{
    public partial class Profil : Form
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
        DataBase db = new DataBase();

        public string userId;

        public Profil(string userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserData();
        }

        // Вывод информации о пользователе
        public void LoadUserData()
        {
            try
            {
                // Получаем данные пользователя из базы данных по его идентификатору
                User user = db.GetUserById(userId);

                // Заполняем элементы формы данными о пользователе
                textName.Text = $"{user.Name} {user.Surname}";
                textEmail.Text = user.Email;
                textData.Text = user.Data;
                textRole.Text = user.RoleId.ToString();

                // Получаем путь к изображению из базы данных
                string photoPath = user.PhotoPath;

                // Проверяем, есть ли путь к изображению
                if (!string.IsNullOrEmpty(photoPath))
                {
                    // Загружаем изображение из пути
                    pictureBox.Image = Image.FromFile(photoPath);
                }
                else
                {
                    // Если путь к изображению отсутствует, очищаем pictureBox
                    pictureBox.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        // Загрузка идей
        public void LoadUserIdeas(string userId)
        {
            try
            {
                // Устанавливаем вертикальную прокрутку
                panelActivity.AutoScroll = true;

                // Очищаем panelActivity перед загрузкой новых данных
                panelActivity.Controls.Clear();

                // Получаем данные из базы данных с помощью метода класса DataBase
                DataTable userIdeaData = db.GetUserIdeas(userId);

                // Получаем ширину panelActivity с учетом полосы прокрутки
                int panelActivityWidth = panelActivity.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 20;

                // Переменная для отслеживания текущей позиции по вертикали
                int currentY = 10; // Отступ от верхней границы panelActivity

                // Пробегаемся по каждой строке и создаем блоки идей пользователя
                foreach (DataRow row in userIdeaData.Rows)
                {
                    int ideaId = Convert.ToInt32(row["idea_id"]);
                    DateTime creationDate = Convert.ToDateTime(row["creation_date"]);
                    string formattedDate = creationDate.ToString("yyyy-MM-dd");

                    // Создаем панель для одной идеи
                    Panel ideaPanel = new Panel();
                    ideaPanel.BackColor = Color.White; // Цвет фона панели
                    ideaPanel.BorderStyle = BorderStyle.FixedSingle; // Обводка для разделения блоков
                    ideaPanel.Size = new Size(panelActivityWidth, 140); // Ширина с учетом полосы прокрутки и отступов
                    ideaPanel.Location = new Point(10, currentY); // Отступы от левой и верхней границ

                    // Добавляем метки в панель идеи
                    Label titleLabel = new Label();
                    titleLabel.Text = row["idea_title"].ToString();
                    titleLabel.AutoSize = true;
                    titleLabel.Location = new Point(10, 10);

                    Label descriptionLabel = new Label();
                    descriptionLabel.Text = row["idea_description"].ToString();
                    descriptionLabel.AutoSize = true;
                    descriptionLabel.Location = new Point(10, 30);

                    Label dateLabel = new Label();
                    dateLabel.Text = formattedDate;
                    dateLabel.AutoSize = true;
                    dateLabel.Location = new Point(10, 50);

                    // Кнопка "Подробнее"
                    Button detailsButton = new Button();
                    detailsButton.Text = "Подробнее";
                    detailsButton.Location = new Point(ideaPanel.Width - 100, 10); // Позиция кнопки "Подробнее"
                    detailsButton.AutoSize = true;
                    detailsButton.Click += (s, e) => ShowIdeaDetails(row); // Обработчик нажатия

                    // Кнопка "Удалить"
                    Button deleteButton = new Button();
                    deleteButton.Text = "Удалить";
                    deleteButton.Location = new Point(ideaPanel.Width - 100, 40); // Позиция кнопки "Удалить"
                    deleteButton.AutoSize = true;
                    deleteButton.Click += (s, e) => DeleteIdea(ideaId); // Обработчик нажатия

                    // Кнопка "Редактировать"
                    Button editButton = new Button();
                    editButton.Text = "Редактировать";
                    editButton.Location = new Point(ideaPanel.Width - 100, 70); // Позиция кнопки "Редактировать"
                    editButton.AutoSize = true;
                    editButton.Click += (s, e) => EditIdea(ideaId); // Обработчик нажатия

                    // Добавляем метки и кнопки в панель идеи
                    ideaPanel.Controls.Add(titleLabel);
                    ideaPanel.Controls.Add(descriptionLabel);
                    ideaPanel.Controls.Add(dateLabel);
                    ideaPanel.Controls.Add(detailsButton);
                    ideaPanel.Controls.Add(deleteButton);
                    ideaPanel.Controls.Add(editButton);

                    // Добавляем панель идеи в panelActivity
                    panelActivity.Controls.Add(ideaPanel);

                    // Увеличиваем текущую позицию по вертикали
                    currentY += ideaPanel.Height + 20; // Отступ между блоками идей и панелью
                }

                // Добавляем отступ снизу
                panelActivity.Controls.Add(new Panel { Size = new Size(0, 5), Location = new Point(0, currentY) });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        // Метод для отображения деталей идеи
        private void ShowIdeaDetails(DataRow ideaData)
        {
            // Здесь вы можете реализовать открытие новой формы или другого механизма для отображения подробной информации об идее
            // Примерно так:
            string title = ideaData["idea_title"].ToString();
            string description = ideaData["idea_description"].ToString();
            DateTime creationDate = Convert.ToDateTime(ideaData["creation_date"]);
            string formattedDate = creationDate.ToString("yyyy-MM-dd");

            MessageBox.Show($"Заголовок: {title}\n\nОписание: {description}\n\nДата создания: {formattedDate}", "Подробная информация об идее");
        }

        // Метод для удаления идеи
        private void DeleteIdea(int ideaId)
        {
            // Здесь нужно выполнить удаление идеи из базы данных по ideaId
            try
            {
                db.DeleteIdea(ideaId); // Предположим, что у вас есть метод для удаления идеи в классе базы данных
                LoadUserIdeas(userId); // Перезагружаем список идей после удаления
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении идеи: " + ex.Message);
            }
        }

        // Метод для редактирования идеи
        private void EditIdea(int ideaId)
        {
            // Здесь нужно реализовать открытие формы для редактирования идеи
            // Примерно так:
            try
            {
                //EditIdeaForm editForm = new EditIdeaForm(ideaId); // Передаем ideaId в форму редактирования
                //editForm.ShowDialog();
                //LoadUserIdeas(userId); // Перезагружаем список идей после редактирования
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании идеи: " + ex.Message);
            }
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем данные пользователя из базы данных по его идентификатору
                User user = db.GetUserById(userId);

                if (user != null)
                {
                    // Определяем, какая главная страница должна быть открыта в зависимости от роли пользователя
                    switch (user.RoleId)
                    {
                        case 1: // Если роль - Innovator
                            MainInnovator mainInnovator = new MainInnovator(userId);
                            mainInnovator.Show();
                            this.Close();
                            break;
                        case 2: // Если роль - Developer
                            MainDeveloper mainDeveloper = new MainDeveloper(userId);
                            mainDeveloper.Show();
                            this.Close();
                            break;
                        case 3: // Если роль - Investor
                            MainInvestor mainInvestor = new MainInvestor(userId);
                            mainInvestor.Show();
                            this.Close();
                            break;
                        case 4: // Если роль - Admin
                            MainAdmin mainAdmin = new MainAdmin(userId);
                            mainAdmin.Show();
                            this.Close();
                            break;
                        default:
                            // Если роль неизвестна, выводим сообщение об ошибке
                            MessageBox.Show("Неизвестная роль пользователя!");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка : " + ex.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Выводим диалоговое окно с запросом подтверждения
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Проверяем результат диалогового окна
            if (result == DialogResult.Yes)
            {
                // Если пользователь подтвердил выход, открываем главное окно входа
                MainStart mainStart = new MainStart();
                mainStart.Show();
                this.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            User user = db.GetUserById(userId);

            ProfilEdit profilEdit = new ProfilEdit(userId, user.Name, user.Surname, user.Email, user.Password, user.PhotoPath);
            profilEdit.profilEdit = this; // Передаем ссылку на текущую форму Profil
            profilEdit.Show();        }

        private void Profil_Load(object sender, EventArgs e)
        {
            LoadUserIdeas(userId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
