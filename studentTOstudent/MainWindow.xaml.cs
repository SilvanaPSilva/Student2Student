using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using studentTOstudent.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace studentTOstudent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserProfile _currentUser;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSignupHere_Click(object sender, RoutedEventArgs e)
        {
            mainTabs.SelectedItem = tabRegister;
        }

        private void Role_Checked(object sender, RoutedEventArgs e)
        {
            if (rbLearner.IsChecked == true)
            {
                subjectsGroupBox.Visibility = Visibility.Collapsed;
            }
            else if (rbTutor.IsChecked == true)
            {
                subjectsGroupBox.Visibility = Visibility.Visible;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (personalInformation.Visibility == Visibility.Collapsed)
            {
                var passwordRegister = txtPasswordRegister.Password;
                var confirmPasswordRegister = txtPasswordRegisterConfirmation.Password;

                if (string.IsNullOrWhiteSpace(txtEmailRegister.Text))
                {
                    MessageBox.Show("Please enter your email.");
                    return;
                }

                if (passwordRegister != confirmPasswordRegister)
                {
                    MessageBox.Show("Passwords not match");
                    return;
                }

                personalInformation.Visibility = Visibility.Visible;
                subjectsGroupBox.Visibility = Visibility.Visible;

                btnRegister.Visibility = Visibility.Collapsed;
                btnSaveRegister.Visibility = Visibility.Visible;


                // If sucessfull, next step
                personalInformation.Visibility = Visibility.Visible;

                // If tutor, show Subjetcs
                if (rbTutor.IsChecked == true)
                    subjectsGroupBox.Visibility = Visibility.Visible;

                return;
            };

        }

        private void btnSaveRegister_Click(object sender, RoutedEventArgs e)
        {
            // Se chegou aqui, estamos na segunda etapa → salvar no banco
            RegisterUserInDatabase();
        }

        private void RegisterUserInDatabase()
        {
            var emailRegister = txtEmailRegister.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtEmailRegister.Text))
            {
                MessageBox.Show("Please enter your email.");
                return;
            }


            var studentNumber = txtStudentNumber.Text.Trim();
            var firstName = txtFirstName.Text.Trim();
            var lastName = txtLastName.Text.Trim();
            var phone = txtPhoneNumber.Text.Trim();
            var university = txtUniversity.Text.Trim();
            var course = txtCourse.Text.Trim();
            var academicYearText = txtAcademicYear.Text.Trim();

            // validation
            if (string.IsNullOrEmpty(studentNumber)
                || string.IsNullOrEmpty(firstName)
                || string.IsNullOrEmpty(lastName)
                || string.IsNullOrEmpty(phone)
                || string.IsNullOrEmpty(university)
                || string.IsNullOrEmpty(course)
                || string.IsNullOrEmpty(academicYearText))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!int.TryParse(academicYearText, out int academicYear) || academicYear < 1 || academicYear > 10)
            {
                MessageBox.Show("Please enter a valid academic year (1-4).");
                return;
            }

            // Additional validation can be added here (e.g., email format, password strength, etc.)
                       
            //Salt and hash the password before storing it in the database
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(txtPasswordRegister.Password, salt);

            //Link to database and save the user information
            var cs = ConfigurationManager.ConnectionStrings["StudentToStudentDB"].ConnectionString;

            using (var connectionSQL = new SqlConnection(cs))
            using (var command = connectionSQL.CreateCommand())
            {
                command.CommandText = @"
                    INSERT TO dbo.Users
                    (StudentNumber, FirstName, LastName, Email, Phone, University,
                    Course, AcademicYear, PasswordHash, PasswordSalt)
                    VALUES
                    (@StudentNumber, @FirstName, @LastName, @Email, @Phone, @University,
                    @Course, @AcademicYear, @PasswordHash, @PasswordSalt);";

                command.Parameters.AddWithValue("@StudentNumber", studentNumber);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", emailRegister);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@University", university);
                command.Parameters.AddWithValue("@Course", course);
                command.Parameters.AddWithValue("@AcademicYear", academicYear);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.Parameters.AddWithValue("@PasswordSalt", salt);

                connectionSQL.Open();
                command.ExecuteNonQuery();

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("User registered successfully!");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627)
                        MessageBox.Show("Email or Student Number already exists.");

                    MessageBox.Show($"Error registering user: {ex.Message}");

                }
            }

            // If all validations pass, proceed with registration logic (e.g., save to database)
            MessageBox.Show("Registration successful!");

            //MessageBox.Show("User registered!");
            //personalInformation.Visibility = Visibility.Collapsed;
            //subjectsGroupBox.Visibility = Visibility.Collapsed;
        }

        private void btnSearchTutors_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //LOGIN FUNCTIONALITY

        private void BtnTestDb_Click(object sender, RoutedEventArgs e)
        {
            var cs = ConfigurationManager.ConnectionStrings["StudentToStudentDb"].ConnectionString;

            using (var connection = new SqlConnection(cs))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection successful!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection failed: {ex.Message}");
                }
            }
        }

        // Salt and hash the password before storing it in the database
        //Salt is a random value added to the password before hashing to ensure that even if two users have the same password, their hashes will be different. 
        private static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;

        }

        //Hashing is a one-way function that converts the password into a fixed-size string of characters, which cannot be reversed to get the original password.
        private static byte[] HashPassword(string password, byte[] salt, int bytes = 64)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, 100000))
            {
                return pbkdf2.GetBytes(bytes);
            }
            // PBKDF2 is a secure password hashing algorithm. It combines the password with a random salt and applies many iterations to make brute-force attacks slow. The salt prevents rainbow table attacks.
        }

        private static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, storedSalt, 100000))
            {
                var hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
                return hashOfEnteredPassword.SequenceEqual(storedHash);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var emailLogin = txtEmailLogin.Text.Trim();
            var passwordLogin = txtPasswordLogin.Password;

            // connection string
            var cs = ConfigurationManager.ConnectionStrings["StudentToStudentDB"].ConnectionString;

            using (var connectionSQL = new SqlConnection(cs))
            using (var command = connectionSQL.CreateCommand())
            {
                command.CommandText = @"
                    SELECT PasswordHash, PasswordSalt
                    FROM dbo.Users
                    WHERE Email = @Email";

                command.Parameters.AddWithValue("@Email", emailLogin);

                connectionSQL.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var storedHash = (byte[])reader["PasswordHash"];
                        var storedSalt = (byte[])reader["PasswordSalt"];
                        if (VerifyPassword(passwordLogin, storedHash, storedSalt))
                        {
                            MessageBox.Show("Login successful!");
                            // Load user profile or navigate to the main application window
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password.");
                    }
                }

            }

        }// FINISH LOGIN FUNCTIONALITY

        // REGISTRATION FUNCTIONALITY
       
    }
}
