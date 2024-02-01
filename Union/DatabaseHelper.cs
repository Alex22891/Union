using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Union
{
    public class DatabaseHelper
    {
        private readonly string connectionString = "server=localhost;user=root;database=AccountingUnion";

        private MySqlConnection GetOpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public List<Student> GetStudents()
        {
            using (MySqlConnection connection = GetOpenConnection())
            {
                string query = "SELECT s.*, g.name_group, f.name_faculty " +
                               "FROM Student s " +
                               "LEFT JOIN `StudentGroup` g ON s.id_group = g.id_group " +
                               "LEFT JOIN Faculty f ON g.id_faculty = f.id_faculty";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<Student> students = new List<Student>();

                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                Id = Convert.ToInt32(reader["id_student"]),
                                Fio = reader["fio"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["date_birth"]),
                                Education = reader["obrazovanie"].ToString(),
                                Address = reader["adres"].ToString(),
                                Telephone = reader["telephone"].ToString(),
                                Nbileta = Convert.ToInt32(reader["Nbileta"]),
                                IdGroup = Convert.ToInt32(reader["id_group"]),
                                OtherInfo = reader["other_info"].ToString(),
                            };

                            students.Add(student);
                        }

                        return students;
                    }
                }
            }
        }

        public async Task<List<Faculty>> GetFacultiesAsync()
        {
            List<Faculty> faculties = new List<Faculty>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Faculty";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Faculty faculty = new Faculty
                        {
                            Id = Convert.ToInt32(reader["id_faculty"]),
                            name_faculty = reader["name_faculty"].ToString()
                        };

                        faculties.Add(faculty);
                    }
                }
            }

            return faculties;
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            List<Group> groups = new List<Group>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM StudentGroup";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Group group = new Group
                        {
                            id_group = Convert.ToInt32(reader["id_group"]),
                            name_group = reader["name_group"].ToString()
                        };

                        groups.Add(group);
                    }
                }
            }

            return groups;
        }

        public void UpdateStudent(Student student)
        {
            using (MySqlConnection connection = GetOpenConnection())
            {
                string query = "UPDATE Student SET fio = @fio, date_birth = @dateOfBirth, obrazovanie = @education, " +
                               "adres = @address, telephone = @telephone, Nbileta = @studentTicketNumber, " +
                               "id_group = @groupId, other_info = @otherInfo WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fio", student.Fio);
                    command.Parameters.AddWithValue("@dateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@education", student.Education);
                    command.Parameters.AddWithValue("@address", student.Address);
                    command.Parameters.AddWithValue("@telephone", student.Telephone);
                    command.Parameters.AddWithValue("@studentTicketNumber", student.Nbileta);
                    command.Parameters.AddWithValue("@groupId", student.IdGroup);
                    command.Parameters.AddWithValue("@otherInfo", student.OtherInfo);
                    command.Parameters.AddWithValue("@id", student.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddStudent(Student student)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Проверяем, существует ли указанная группа в таблице StudentGroup
                string checkGroupQuery = "SELECT COUNT(*) FROM StudentGroup WHERE id_group = @groupId";
                MySqlCommand checkGroupCommand = new MySqlCommand(checkGroupQuery, connection);
                checkGroupCommand.Parameters.AddWithValue("@groupId", student.Group.id_group);
                int groupCount = Convert.ToInt32(checkGroupCommand.ExecuteScalar());

                if (groupCount == 0)
                {
                    MessageBox.Show($"Attempting to add student with GroupId: {student.Group.id_group}");
                    // Группа не существует, выбросим ошибку или обработаем этот случай по вашему усмотрению
                    throw new InvalidOperationException("Группа с указанным идентификатором не найдена.");
                }

                // Группа существует, выполняем вставку студента
                string query = "INSERT INTO Student (fio, date_birth, obrazovanie, adres, telephone, Nbileta, id_group, other_info) " +
                               "VALUES (@fio, @dateOfBirth, @education, @address, @telephone, @studentTicketNumber, @groupId, @otherInfo)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@fio", student.Fio);
                command.Parameters.AddWithValue("@dateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@education", student.Education);
                command.Parameters.AddWithValue("@address", student.Address);
                command.Parameters.AddWithValue("@telephone", student.Telephone);
                command.Parameters.AddWithValue("@studentTicketNumber", student.Nbileta);
                command.Parameters.AddWithValue("@groupId", student.Group.id_group);
                command.Parameters.AddWithValue("@otherInfo", student.OtherInfo);

                command.ExecuteNonQuery();
            }
        }
        public List<Sanatorium> GetSanatoria()
        {
            List<Sanatorium> sanatoria = new List<Sanatorium>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Sanatorium";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Sanatorium sanatorium = new Sanatorium
                        {
                            Id = Convert.ToInt32(reader["id_sanatorium"]),
                            Nbileta = Convert.ToInt32(reader["Nbileta"]),
                            Nzaezda = Convert.ToInt32(reader["Nzaezda"]),
                            God = reader.GetDateTime(reader.GetOrdinal("god")),
                            NachaloZaezda = reader.GetDateTime(reader.GetOrdinal("nachalo_zaezda")),
                            KonecZaezda = reader.GetDateTime(reader.GetOrdinal("konec_zaezda")),
                            StatusOplaty = reader["status_oplaty"].ToString(),
                        };

                        sanatoria.Add(sanatorium);
                    }
                }
            }

            return sanatoria;
        }
        public List<Payment> GetPayments()
        {
            List<Payment> payments = new List<Payment>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Payment";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Payment payment = new Payment
                        {
                            IdOplaty = Convert.ToInt32(reader["id_oplaty"]),
                            Summa = Convert.ToInt32(reader["summa"]),
                            GodOplaty = reader.GetDateTime(reader.GetOrdinal("god_oplaty")),
                            StatusOplaty = reader["status_oplaty"].ToString(),
                            IdPerioda = Convert.ToInt32(reader["id_perioda"]),
                            Nbileta = Convert.ToInt32(reader["Nbileta"]),
                        };

                        payments.Add(payment);
                    }
                }
            }

            return payments;
        }
    }
}
