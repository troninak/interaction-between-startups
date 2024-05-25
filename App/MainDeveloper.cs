using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class MainDeveloper : Form
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

        public MainDeveloper(string userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        public void LoadIdeaData(string sortOrder = "Descending")
        {
            try
            {
                // Устанавливаем вертикальную прокрутку
                panelIdea.AutoScroll = true;

                // Очищаем panelIdea перед загрузкой новых данных
                panelIdea.Controls.Clear();

                // Получаем данные из базы данных с помощью метода класса DataBase
                DataTable ideaData = db.GetIdeaData();

                // Сортируем данные по дате создания
                if (sortOrder == "Ascending")
                {
                    ideaData.DefaultView.Sort = "creation_date ASC";
                }
                else
                {
                    ideaData.DefaultView.Sort = "creation_date DESC";
                }
                DataTable sortedIdeaData = ideaData.DefaultView.ToTable();

                // Получаем ширину panelIdea с учетом полосы прокрутки
                int panelIdeaWidth = panelIdea.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 20;

                // Переменная для отслеживания текущей позиции по вертикали
                int currentY = 10; // Отступ от верхней границы panelIdea

                // Пробегаемся по каждой строке и создаем блоки
                foreach (DataRow row in sortedIdeaData.Rows)
                {
                    DateTime creationDate = Convert.ToDateTime(row["creation_date"]);
                    string formattedDate = creationDate.ToString("yyyy-MM-dd");
                    string userId = row["user_id"].ToString();
                    int ideaId = Convert.ToInt32(row["idea_id"]);

                    // Получаем имя пользователя по userId
                    User user = db.GetUserById(userId);
                    string userName = user != null ? $"{user.Name} {user.Surname}" : "Неизвестный пользователь";

                    // Создаем панель для одной идеи
                    Panel ideaPanel = new Panel
                    {
                        BackColor = Color.White, // Цвет фона панели
                        BorderStyle = BorderStyle.FixedSingle,
                        Size = new Size(panelIdeaWidth, 140), // Ширина с учетом полосы прокрутки и отступов
                        Location = new Point(10, currentY) // Отступы от левой и верхней границ
                    };

                    // Добавляем метки в панель идеи
                    Label titleLabel = new Label
                    {
                        Text = row["idea_title"].ToString(),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };

                    Label userLabel = new Label
                    {
                        Text = "Автор: " + userName,
                        AutoSize = true,
                        Location = new Point(10, 30)
                    };

                    Label descriptionLabel = new Label
                    {
                        Text = row["idea_description"].ToString(),
                        AutoSize = true,
                        MaximumSize = new Size(panelIdeaWidth - 20, 0), // Устанавливаем максимальную ширину и неограниченную высоту
                        Location = new Point(10, 50)
                    };

                    Label dateLabel = new Label
                    {
                        Text = formattedDate,
                        AutoSize = true,
                        Location = new Point(10, 70)
                    };

                    // Кнопка "Взять в работу"
                    Button takeInWorkButton = new Button
                    {
                        Text = "Взять в работу",
                        Location = new Point(ideaPanel.Width - 120, 10), // Позиция кнопки "Взять в работу"
                        AutoSize = true
                    };
                    takeInWorkButton.Click += (s, e) => TakeIdeaInWork(row); // Обработчик нажатия

                    // Добавляем метки и кнопки в панель идеи
                    ideaPanel.Controls.Add(titleLabel);
                    ideaPanel.Controls.Add(userLabel);
                    ideaPanel.Controls.Add(descriptionLabel);
                    ideaPanel.Controls.Add(dateLabel);
                    ideaPanel.Controls.Add(takeInWorkButton);

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

        private void TakeIdeaInWork(DataRow ideaData)
        {
            try
            {
                string currentUserId = userId;
                int ideaId = Convert.ToInt32(ideaData["idea_id"]);

                // Проверяем, взял ли пользователь уже эту идею в работу
                if (db.IsIdeaTakenByUser(ideaId, currentUserId))
                {
                    MessageBox.Show("Вы уже взяли эту идею в работу.", "Ошибка");
                    return;
                }

                // Обновляем статус идеи и добавляем запись в таблицу Projects
                db.AddProject(ideaId, currentUserId, ideaData["idea_description"].ToString());

                MessageBox.Show("Идея успешно взята в работу.", "Успех");

                // Перезагружаем данные идей
                LoadIdeaData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sortOrder = comboBoxDate.SelectedItem.ToString();
            LoadIdeaData(sortOrder);
        }

        private void btnUpdateIdea_Click(object sender, EventArgs e)
        {
            LoadIdeaData();
        }

        private void MainDeveloper_Load(object sender, EventArgs e)
        {
            comboBoxDate.Items.Add("Ascending");
            comboBoxDate.Items.Add("Descending");
            comboBoxDate.SelectedIndex = 0; // Установите начальное значение по умолчанию

            btnSave.Click += new EventHandler(btnSave_Click);

            // Инициализация загрузки данных
            LoadIdeaData("Descending");
        }
    }
}
