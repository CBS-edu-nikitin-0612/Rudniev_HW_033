using System;
using System.Text.RegularExpressions;
using System.IO;
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

namespace aditionalTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool isLogOn;
        private FileInfo fileInfo;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private FileStream fileStream;
        string userLogin;
        string userPassword;
        string warning;
        public LoginWindow()
        {
            InitializeComponent();
            isLogOn = false;
            fileInfo = new FileInfo("loginInfo.txt");
            warning = "";
        }

        private void logOnActivateButton_Click(object sender, RoutedEventArgs e)
        {
            logInActivateButton.IsEnabled = true;
            logOnActivateButton.IsEnabled = false;
            isLogOn = true;
            logButton.Content = "Log on";
            WarningLabel.Content = "";
        }
        private void logInActivateButton_Click(object sender, RoutedEventArgs e)
        {
            logInActivateButton.IsEnabled = false;
            logOnActivateButton.IsEnabled = true;
            isLogOn = false;
            logButton.Content = "Log in";
            WarningLabel.Content = "";
        }

        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            bool resultCheckInfo;
            userLogin = loginTextBox.Text;
            userPassword = passwordTextBox.Text;

            if (isLogOn)
            {
                resultCheckInfo = CheckingInfoLogOn();
                if (resultCheckInfo)
                {
                    resultCheckInfo = CheckingAndSaveInfoLogOnToFile();
                    if (resultCheckInfo)
                    {
                        WarningLabel.Content = "";
                        MessageBox.Show("You have successfully registered!", "successfully registered", MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                resultCheckInfo = CheckingInfoForLogIn();
                if (resultCheckInfo)
                {
                    WarningLabel.Content = "";
                    MessageBox.Show("You have successfully log in!", "successfully log in", MessageBoxButton.OK);
                }
                else
                {
                    warning = "Wrong login or password!";
                    WarningLabel.Content = warning;
                }
            }
        }
        private bool CheckingInfoLogOn()
        {
            WarningLabel.Content = "";
            warning = "";
            bool result = false;
            string pattern = "";
            Regex regex = null;

            if (userLogin.Length > 3)
            {
                pattern = @"^[A-Za-z]+$";
                regex = new Regex(pattern);
                result = regex.IsMatch(userLogin);
                if (!result)
                {
                    warning = "Wrong login, \nonly symbol A(a)-Z(z)!";
                    WarningLabel.Content = warning;
                    return false;
                }
            }
            else
            {
                warning = "Short login, \nat least 4 characters!";
                WarningLabel.Content = warning;
                return false;
            }

            if (userPassword.Length > 7)
            {
                pattern = @"^[A-Za-z0-9]+$";
                regex = new Regex(pattern);
                result = regex.IsMatch(userPassword);
                if (!result)
                {
                    warning = "Wrong password, only \nsymbol A(a)-Z(z) and 0-9!";
                    WarningLabel.Content = warning;
                    return false;
                }
            }
            else
            {
                warning = "Short password, \nat least 8 characters!";
                WarningLabel.Content = warning;
                return false;
            }

            WarningLabel.Content = warning;
            return result;
        }
        private bool CheckingAndSaveInfoLogOnToFile()
        {
            string[] currentCheckingLoginInfo;

            using (fileStream = File.Open(fileInfo.FullName, FileMode.Open))
            {
                streamReader = new StreamReader(fileStream);
                while (!streamReader.EndOfStream)
                {
                    currentCheckingLoginInfo = streamReader.ReadLine().Split(" ");
                    if (currentCheckingLoginInfo[0] == userLogin)
                    {
                        warning = "This login is already in use!";
                        WarningLabel.Content = warning;
                        streamReader.Close();
                        return false;
                    }
                }
                streamReader.Close();
            }

            using (fileStream = File.Open(fileInfo.FullName, FileMode.Append))
            {
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine($"{userLogin} {userPassword}");
                streamWriter.Close();
            }

            return true;
        }
        private bool CheckingInfoForLogIn()
        {
            string[] currentCheckingLoginInfo;
            using (fileStream = File.Open(fileInfo.FullName, FileMode.OpenOrCreate))
            {
                streamReader = new StreamReader(fileStream);
                while (!streamReader.EndOfStream)
                {
                    currentCheckingLoginInfo = streamReader.ReadLine().Split(" ");
                    if (currentCheckingLoginInfo[0] == userLogin && currentCheckingLoginInfo[1] == userPassword)
                    {
                        return true;
                    }
                }
                streamReader.Close();
            }
            return false;
        }

    }
}
