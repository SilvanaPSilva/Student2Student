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
            personalInformation.Visibility = Visibility.Visible;            
        }

        private void btnSearchTutors_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
