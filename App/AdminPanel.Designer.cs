namespace App
{
    partial class AdminPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdateIdea = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.dataGridViewUser = new System.Windows.Forms.DataGridView();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.surname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.role_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photo_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_button = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delete_button = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewIdea = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.idea_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_id_idea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idea_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idea_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creation_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_button_idea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delete_button_idea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnIdeaAdd = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.btnUpdateIdea.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUser)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIdea)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1400, 24);
            this.panel2.TabIndex = 2;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBack.Location = new System.Drawing.Point(1283, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(56, 24);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "<";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1344, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 24);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdateIdea
            // 
            this.btnUpdateIdea.Controls.Add(this.tabPage1);
            this.btnUpdateIdea.Controls.Add(this.tabPage2);
            this.btnUpdateIdea.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUpdateIdea.Location = new System.Drawing.Point(12, 24);
            this.btnUpdateIdea.Name = "btnUpdateIdea";
            this.btnUpdateIdea.SelectedIndex = 0;
            this.btnUpdateIdea.Size = new System.Drawing.Size(1376, 764);
            this.btnUpdateIdea.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUpdateUser);
            this.tabPage1.Controls.Add(this.dataGridViewUser);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage1.Location = new System.Drawing.Point(4, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1368, 720);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Пользователи";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Location = new System.Drawing.Point(6, 675);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(151, 39);
            this.btnUpdateUser.TabIndex = 5;
            this.btnUpdateUser.Text = "Обновить";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // dataGridViewUser
            // 
            this.dataGridViewUser.AllowUserToAddRows = false;
            this.dataGridViewUser.AllowUserToDeleteRows = false;
            this.dataGridViewUser.AllowUserToResizeColumns = false;
            this.dataGridViewUser.AllowUserToResizeRows = false;
            this.dataGridViewUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.name,
            this.surname,
            this.email,
            this.password,
            this.role_id,
            this.registration_date,
            this.photo_path,
            this.edit_button,
            this.delete_button});
            this.dataGridViewUser.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewUser.Name = "dataGridViewUser";
            this.dataGridViewUser.Size = new System.Drawing.Size(1356, 663);
            this.dataGridViewUser.TabIndex = 0;
            this.dataGridViewUser.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            // 
            // user_id
            // 
            this.user_id.HeaderText = "ID";
            this.user_id.Name = "user_id";
            // 
            // name
            // 
            this.name.HeaderText = "Имя";
            this.name.Name = "name";
            // 
            // surname
            // 
            this.surname.HeaderText = "Фамилия";
            this.surname.Name = "surname";
            // 
            // email
            // 
            this.email.HeaderText = "Почта";
            this.email.Name = "email";
            // 
            // password
            // 
            this.password.HeaderText = "Пароль";
            this.password.Name = "password";
            // 
            // role_id
            // 
            this.role_id.HeaderText = "ID Роль";
            this.role_id.Name = "role_id";
            this.role_id.Width = 200;
            // 
            // registration_date
            // 
            this.registration_date.HeaderText = "Дата регистрации";
            this.registration_date.Name = "registration_date";
            this.registration_date.Width = 200;
            // 
            // photo_path
            // 
            this.photo_path.HeaderText = "Путь фото";
            this.photo_path.Name = "photo_path";
            this.photo_path.Width = 200;
            // 
            // edit_button
            // 
            this.edit_button.HeaderText = "";
            this.edit_button.Name = "edit_button";
            // 
            // delete_button
            // 
            this.delete_button.HeaderText = "";
            this.delete_button.Name = "delete_button";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnIdeaAdd);
            this.tabPage2.Controls.Add(this.dataGridViewIdea);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage2.Location = new System.Drawing.Point(4, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1368, 720);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Идеи";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewIdea
            // 
            this.dataGridViewIdea.AllowUserToAddRows = false;
            this.dataGridViewIdea.AllowUserToDeleteRows = false;
            this.dataGridViewIdea.AllowUserToResizeColumns = false;
            this.dataGridViewIdea.AllowUserToResizeRows = false;
            this.dataGridViewIdea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIdea.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idea_id,
            this.user_id_idea,
            this.idea_title,
            this.idea_description,
            this.creation_date,
            this.status,
            this.edit_button_idea,
            this.delete_button_idea});
            this.dataGridViewIdea.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewIdea.Name = "dataGridViewIdea";
            this.dataGridViewIdea.Size = new System.Drawing.Size(1356, 663);
            this.dataGridViewIdea.TabIndex = 6;
            this.dataGridViewIdea.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIdea_CellContentClick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(6, 675);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 39);
            this.button1.TabIndex = 5;
            this.button1.Text = "Обновить таблицу";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // idea_id
            // 
            this.idea_id.HeaderText = "ID";
            this.idea_id.Name = "idea_id";
            // 
            // user_id_idea
            // 
            this.user_id_idea.HeaderText = "ID Пользователя";
            this.user_id_idea.Name = "user_id_idea";
            this.user_id_idea.Width = 200;
            // 
            // idea_title
            // 
            this.idea_title.HeaderText = "Название";
            this.idea_title.Name = "idea_title";
            this.idea_title.Width = 200;
            // 
            // idea_description
            // 
            this.idea_description.HeaderText = "Описание";
            this.idea_description.Name = "idea_description";
            this.idea_description.Width = 200;
            // 
            // creation_date
            // 
            this.creation_date.HeaderText = "Дата создания";
            this.creation_date.Name = "creation_date";
            this.creation_date.Width = 200;
            // 
            // status
            // 
            this.status.HeaderText = "Статус";
            this.status.Name = "status";
            // 
            // edit_button_idea
            // 
            this.edit_button_idea.HeaderText = "";
            this.edit_button_idea.Name = "edit_button_idea";
            this.edit_button_idea.Width = 200;
            // 
            // delete_button_idea
            // 
            this.delete_button_idea.HeaderText = "";
            this.delete_button_idea.Name = "delete_button_idea";
            this.delete_button_idea.Width = 200;
            // 
            // btnIdeaAdd
            // 
            this.btnIdeaAdd.Location = new System.Drawing.Point(164, 675);
            this.btnIdeaAdd.Name = "btnIdeaAdd";
            this.btnIdeaAdd.Size = new System.Drawing.Size(206, 39);
            this.btnIdeaAdd.TabIndex = 7;
            this.btnIdeaAdd.Text = "Добавить идею";
            this.btnIdeaAdd.UseVisualStyleBackColor = true;
            this.btnIdeaAdd.Click += new System.EventHandler(this.btnIdeaAdd_Click);
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.btnUpdateIdea);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdminPanel";
            this.Load += new System.EventHandler(this.AdminPanel_Load);
            this.panel2.ResumeLayout(false);
            this.btnUpdateIdea.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUser)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIdea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl btnUpdateIdea;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridViewUser;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridViewIdea;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn surname;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn password;
        private System.Windows.Forms.DataGridViewTextBoxColumn role_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn photo_path;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn delete_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn idea_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id_idea;
        private System.Windows.Forms.DataGridViewTextBoxColumn idea_title;
        private System.Windows.Forms.DataGridViewTextBoxColumn idea_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn creation_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_button_idea;
        private System.Windows.Forms.DataGridViewTextBoxColumn delete_button_idea;
        private System.Windows.Forms.Button btnIdeaAdd;
    }
}