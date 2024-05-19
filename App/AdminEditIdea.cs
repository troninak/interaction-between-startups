using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace App
{
    public partial class AdminEditIdea : Form
    {
        private string userId;
        private string ideaId;
        private DataBase db = new DataBase();

        public AdminEditIdea(string userId, string ideaId, string ideaTitle, string ideaDescription, string ideaStatus)
        {
            InitializeComponent();
            this.ideaId = ideaId;
            this.userId = userId;
            textTitle.Text = ideaTitle;
            textDescription.Text = ideaDescription;
            comboBoxStatus.Text = ideaStatus;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel(userId);
            adminPanel.Show();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string newTitle = textTitle.Text;
                string newDescription = textDescription.Text;
                string newStatus = comboBoxStatus.Text;

                db.UpdateIdeaData(ideaId, newTitle, newDescription, newStatus);

                MessageBox.Show("Идея успешно обновлена.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении идеи: " + ex.Message);
            }
        }
    }
}
