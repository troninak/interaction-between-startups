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
    public partial class MainInnovator : Form
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
        private string userId;

        private DataBase db = new DataBase();

        public MainInnovator(string userId)
        {
            InitializeComponent();
            this.userId = userId;
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

        private void btnProfil_Click(object sender, EventArgs e)
        {
            Profil profile = new Profil(userId);
            profile.Show();
            this.Hide();
        }

        private void btnIdeaAdd_Click(object sender, EventArgs e)
        {
            MainInnovatorAdd mainInnovatorAdd = new MainInnovatorAdd(userId);
            mainInnovatorAdd.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Динамическок создание блока с информацией об идеях и добавлять их в panelIdea
        public void LoadIdeaData()
        {
            try
            {
                // Устанавливаем вертикальную прокрутку
                panelIdea.AutoScroll = true;

                // Очищаем panelIdea перед загрузкой новых данных
                panelIdea.Controls.Clear();

                // Получаем данные из базы данных с помощью метода класса DataBase
                DataTable ideaData = db.GetIdeaData();

                // Получаем ширину panelIdea с учетом полосы прокрутки
                int panelIdeaWidth = panelIdea.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 20;

                // Переменная для отслеживания текущей позиции по вертикали
                int currentY = 10; // Отступ от верхней границы panelIdea

                // Пробегаемся по каждой строке и создаем блоки
                foreach (DataRow row in ideaData.Rows)
                {
                    DateTime creationDate = Convert.ToDateTime(row["creation_date"]);
                    string formattedDate = creationDate.ToString("yyyy-MM-dd");
                    string userId = row["user_id"].ToString();

                    // Получаем имя пользователя по userId
                    User user = db.GetUserById(userId);
                    string userName = user != null ? $"{user.Name} {user.Surname}" : "Неизвестный пользователь";

                    // Создаем панель для одной идеи
                    Panel ideaPanel = new Panel();
                    ideaPanel.BackColor = Color.White; // Цвет фона панели
                    ideaPanel.BorderStyle = BorderStyle.None; // Убираем обводку
                    ideaPanel.BorderStyle = BorderStyle.FixedSingle;
                    ideaPanel.Width = panelIdeaWidth; // Ширина с учетом полосы прокрутки и отступов
                    ideaPanel.AutoSize = true;
                    ideaPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    ideaPanel.Location = new Point(10, currentY); // Отступы от левой и верхней границ

                    // Добавляем метки в панель идеи
                    Label titleLabel = new Label();
                    titleLabel.Text = row["idea_title"].ToString();
                    titleLabel.AutoSize = true;
                    titleLabel.Location = new Point(10, 10);

                    Label userLabel = new Label();
                    userLabel.Text = "Автор: " + userName;
                    userLabel.AutoSize = true;
                    userLabel.Location = new Point(10, 30);

                    Label descriptionLabel = new Label();
                    descriptionLabel.Text = row["idea_description"].ToString();
                    descriptionLabel.AutoSize = true;
                    descriptionLabel.MaximumSize = new Size(panelIdeaWidth - 20, 0); // Устанавливаем максимальную ширину и неограниченную высоту
                    descriptionLabel.Location = new Point(10, 50);

                    Label dateLabel = new Label();
                    dateLabel.Text = formattedDate;
                    dateLabel.AutoSize = true;
                    dateLabel.Location = new Point(10, 70);

                    // Кнопка "Подробнее"
                    Button detailsButton = new Button();
                    detailsButton.Text = "Подробнее";
                    detailsButton.Location = new Point(ideaPanel.Width - 100, 10); // Позиция кнопки "Подробнее"
                    detailsButton.AutoSize = true;
                    detailsButton.Click += (s, e) => ShowIdeaDetails(row); // Обработчик нажатия

                    // Добавляем метки и кнопку "Подробнее" в панель идеи
                    ideaPanel.Controls.Add(titleLabel);
                    ideaPanel.Controls.Add(userLabel);
                    ideaPanel.Controls.Add(descriptionLabel);
                    ideaPanel.Controls.Add(dateLabel);
                    ideaPanel.Controls.Add(detailsButton);

                    // Добавляем панель идеи в panelIdea
                    panelIdea.Controls.Add(ideaPanel);

                    // Увеличиваем текущую позицию по вертикали
                    currentY += ideaPanel.Height + 20; // Отступ между блоками идей и панелью
                }

                // Добавляем отступ снизу
                panelIdea.Controls.Add(new Panel { Size = new Size(0, 5), Location = new Point(0, currentY) });
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
            string userId = ideaData["user_id"].ToString();
            User user = db.GetUserById(userId);
            string userName = user != null ? $"{user.Name} {user.Surname}" : "Неизвестный пользователь";
            DateTime creationDate = Convert.ToDateTime(ideaData["creation_date"]);
            string formattedDate = creationDate.ToString("yyyy-MM-dd");

            MessageBox.Show($"Заголовок: {title}\n\nАвтор: {userName}\n\nОписание: {description}\n\nДата создания: {formattedDate}", "Подробная информация об идее");
        }

        private void MainInnovator_Load(object sender, EventArgs e)
        {
            LoadIdeaData();
        }
    }
}
