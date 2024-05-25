using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace App
{
    internal class DataBase
    {
        MySqlConnection conn = new MySqlConnection("server = localhost; port = 3306; username = root; password =; database = innovation_platform");

        public void openConn()
        {
            if(conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        public void closeConn()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
        public MySqlConnection getConn()
        {
            return conn;
        }

        // Получение информации о пользователях
        public DataTable GetUserData()
        {
            DataTable userData = new DataTable();
            try
            {
                conn.Open();

                string query = "SELECT user_id, name, surname, email, password, role_id, registration_date, photo_path FROM Users";

                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                adapter.Fill(userData);
            }
            finally
            {
                conn.Close();
            }
            return userData;
        }

        public DataRow GetUserDataProfil(string userId)
        {
            DataTable userData = new DataTable();
            try
            {
                openConn();

                string query = "SELECT user_id, name, surname, email, password, role_id, registration_date, photo_path FROM Users WHERE user_id = @userId";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@userId", userId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                adapter.Fill(userData);
            }
            finally
            {
                closeConn();
            }

            if (userData.Rows.Count > 0)
            {
                return userData.Rows[0];
            }
            else
            {
                throw new Exception("Пользователь не найден.");
            }
        }

        //Получение информации о идеях
        public DataTable GetIdeaData()
        {
            DataTable dataTable = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT idea_id, user_id, idea_title, idea_description, creation_date, status FROM Ideas";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении данных идей: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dataTable;
        }

        // Метод для получения данных идеи по ID
        public DataRow GetIdeaById(int ideaId)
        {
            DataTable dataTable = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT idea_id, user_id, idea_title, idea_description, creation_date, status FROM Ideas WHERE idea_id = @ideaId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ideaId", ideaId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении данных идеи: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Метод для удаления идем из базы данных
        public void DeleteIdeaById(string ideaId)
        {
            try
            {
                conn.Open();
                string query = "DELETE FROM Ideas WHERE idea_id = @ideaId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ideaId", ideaId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при удалении идеи: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Изменение для идем в базе данных
        public void UpdateIdeaData(string ideaId, string newTitle, string newDescription, string newStatus)
        {
            try
            {
                conn.Open();
                string query = "UPDATE Ideas SET idea_title = @newTitle, idea_description = @newDescription, status = @newStatus WHERE idea_id = @ideaId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@newTitle", newTitle);
                cmd.Parameters.AddWithValue("@newDescription", newDescription);
                cmd.Parameters.AddWithValue("@newStatus", newStatus);
                cmd.Parameters.AddWithValue("@ideaId", ideaId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении идеи: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Метод для удаление из базы данных
        public bool DeleteUser(string userId)
        {
            try
            {
                conn.Open();

                string query = "DELETE FROM Users WHERE user_id = @user_id";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@user_id", userId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true; // Успешно удалено
                }
                else
                {
                    return false; // Ничего не удалено
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                return false; // Ошибка при удалении
            }
            finally
            {
                conn.Close();
            }
        }

        // Метод для обновления базы данных
        public void SaveUserData(string userId, string newName, string newSurname, string newEmail, string newPassword, string newRoleId, string newPhotoPath)
        {
            try
            {
                // Открываем соединение
                conn.Open();

                // Создаем команду для выполнения SQL-запроса
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "UPDATE Users SET name = @newName, surname = @newSurname, email = @newEmail, password = @newPassword, role_id = @newRoleId, photo_path = @newPhotoPath WHERE user_id = @userId";

                // Добавляем параметры к команде
                command.Parameters.AddWithValue("@newName", newName);
                command.Parameters.AddWithValue("@newSurname", newSurname);
                command.Parameters.AddWithValue("@newEmail", newEmail);
                command.Parameters.AddWithValue("@newPassword", newPassword);
                command.Parameters.AddWithValue("@newRoleId", newRoleId);
                command.Parameters.AddWithValue("@newPhotoPath", newPhotoPath);
                command.Parameters.AddWithValue("@userId", userId);

                // Выполняем команду
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных пользователя: " + ex.Message);
                throw new Exception("Ошибка при сохранении данных пользователя: " + ex.Message);
            }
            finally
            {
                // Закрываем соединение
                conn.Close();
            }
        }

        internal void SaveUserData(object userId, string newName, string newSurname, string newEmail, string newPassword, string newRoleId)
        {
            throw new NotImplementedException();
        }

        // Получения идентификатора пользователя по email и password
        public string GetCurrentUserId(string email, string password)
        {
            try
            {
                openConn(); // Открыть подключение к базе данных

                // SQL-запрос для получения идентификатора пользователя по email и password
                string query = "SELECT user_id FROM Users WHERE email = @Email AND password = @Password";

                // Создание команды для выполнения запроса
                MySqlCommand command = new MySqlCommand(query, conn);

                // Добавление параметров к команде
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                // Выполнение запроса и получение результата
                object result = command.ExecuteScalar();

                // Проверка наличия результата
                if (result != null)
                {
                    // Возвращаем идентификатор пользователя в виде строки
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("Пользователь с указанным email и паролем не найден.");
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок
                MessageBox.Show("Ошибка при получении идентификатора пользователя: " + ex.Message);
            }
            finally
            {
                closeConn(); // Закрыть подключение к базе данных
            }

            // В случае ошибки или если пользователь не найден, возвращаем null
            return null;
        }

        // Получение данных о пользователя по его идентификатору
        public User GetUserById(string userId)
        {
            User user = null;

            try
            {
                // Открытие подключения
                openConn();

                // SQL-запрос для получения данных пользователя по его идентификатору
                string query = "SELECT * FROM Users WHERE user_id = @UserId";

                // Создание команды для выполнения запроса
                MySqlCommand command = new MySqlCommand(query, conn);

                // Добавление параметра к команде
                command.Parameters.AddWithValue("@UserId", userId);

                // Создание объекта для чтения данных из базы данных
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Проверка наличия данных
                    if (reader.Read())
                    {
                        // Получение значения даты из результата запроса
                        DateTime registrationDate = reader.GetDateTime(reader.GetOrdinal("registration_date"));

                        // Получение пути к фотографии пользователя
                        string photoPath = reader.IsDBNull(reader.GetOrdinal("photo_path")) ? null : reader.GetString(reader.GetOrdinal("photo_path"));

                        // Получение пароля пользователя
                        string password = reader.IsDBNull(reader.GetOrdinal("password")) ? null : reader.GetString(reader.GetOrdinal("password"));

                        // Создание нового экземпляра класса User и заполнение его данными из результата запроса
                        user = new User
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Surname = reader.GetString(reader.GetOrdinal("surname")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                            Data = registrationDate.ToString("yyyy-MM-dd"),
                            PhotoPath = photoPath, // Установка пути к фотографии пользователя
                            Password = password // Установка пароля пользователя
                            // Заполните остальные поля пользовательского класса, если необходимо
                        };
                    }
                    else
                    {
                        // Если данные не найдены, выводим сообщение об ошибке
                        MessageBox.Show("Пользователь с указанным идентификатором не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок
                MessageBox.Show("Ошибка при получении данных пользователя: " + ex.Message);
            }
            finally
            {
                // Закрытие подключения
                closeConn();
            }

            return user;
        }

        // Получение ID роли
        public int GetRoleById(string userId)
        {
            int roleId = -1; // Значение по умолчанию, если роль не найдена

            try
            {
                // Открытие подключения к базе данных
                conn.Open();

                // SQL-запрос для получения данных пользователя по его идентификатору
                string query = "SELECT role_id FROM Users WHERE user_id = @UserId";

                // Создание команды для выполнения запроса
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    // Добавление параметра к команде
                    command.Parameters.AddWithValue("@UserId", userId);

                    // Выполнение запроса и получение значения роли
                    object result = command.ExecuteScalar();

                    // Проверка наличия данных
                    if (result != null)
                    {
                        roleId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с идентификатором {0} не найден.", userId);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок
                MessageBox.Show("Ошибка при получении роли пользователя: " + ex.Message);
            }

            return roleId;
        }

        // Добавление идеи в базу данных
        public void AddIdea(string userId, string ideaTitle, string ideaDescription)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO Ideas (user_id, idea_title, idea_description, creation_date, status) VALUES (@userId, @ideaTitle, @ideaDescription, @creationDate, @status)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@ideaTitle", ideaTitle);
                cmd.Parameters.AddWithValue("@ideaDescription", ideaDescription);
                cmd.Parameters.AddWithValue("@creationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@status", "Pending");
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при добавлении идеи: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Получение идем пользователя
        public DataTable GetUserIdeas(string userId)
        {
            DataTable userIdeas = new DataTable();
            try
            {
                openConn();
                string query = "SELECT * FROM ideas WHERE user_id = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, getConn()))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(userIdeas);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении идей пользователя: " + ex.Message);
            }
            finally
            {
                closeConn();
            }
            return userIdeas;
        }

        // Удаление идеи
        public void DeleteIdea(int ideaId)
        {
            try
            {
                openConn();
                string query = "DELETE FROM ideas WHERE idea_id = @ideaId";
                using (MySqlCommand cmd = new MySqlCommand(query, getConn()))
                {
                    cmd.Parameters.AddWithValue("@ideaId", ideaId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при удалении идеи: " + ex.Message);
            }
            finally
            {
                closeConn();
            }
        }

        // Добавление идей в разработку
        public void AddProject(int ideaId, string developerId, string description)
        {
            string query = "INSERT INTO Projects (idea_id, developer_id, project_description, start_date) VALUES (@ideaId, @developerId, @description, @startDate)";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ideaId", ideaId);
                cmd.Parameters.AddWithValue("@developerId", developerId);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@startDate", DateTime.Now);

                openConn();
                cmd.ExecuteNonQuery();
                closeConn();
            }
        }
        
        // Получение идей взятых в разработку пользователем
        public DataTable GetDeveloperProjects(string developerId)
        {
            string query = "SELECT Ideas.* FROM Projects INNER JOIN Ideas ON Projects.idea_id = Ideas.idea_id WHERE Projects.developer_id = @developerId";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@developerId", developerId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        // Проверка, взята ли идея пользователем
        public bool IsIdeaTakenByUser(int ideaId, string userId)
        {
            try
            {
                openConn();

                string query = "SELECT COUNT(*) FROM Projects WHERE idea_id = @ideaId AND developer_id = @userId";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@ideaId", ideaId);
                command.Parameters.AddWithValue("@userId", userId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
            finally
            {
                closeConn();
            }
        }

        // Удаление записи проекта
        public void RemoveProject(int ideaId, string userId)
        {
            try
            {
                openConn();

                string query = "DELETE FROM Projects WHERE idea_id = @ideaId AND developer_id = @userId";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@ideaId", ideaId);
                command.Parameters.AddWithValue("@userId", userId);

                command.ExecuteNonQuery();
            }
            finally
            {
                closeConn();
            }
        }


    }
}
