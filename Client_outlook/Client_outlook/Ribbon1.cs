using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Client_outlook;
using System.Windows.Forms;
using System.Windows.Data;

namespace Client_outlook
{
    public partial class MainContainerRibbon
    {

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            Glob.accData.Password = Properties.Settings.Default.password;
            Glob.accData.Email = Properties.Settings.Default.email;
            ServAcc.Account acc = new ServAcc.Account();
            ServAcc.ResultatOfstringAccountDataGmrGjgq6 result = acc.Login(EmailBox.Text, PasswordBox.Text);
            if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
            {
                Glob.res = result._val1;
                Glob.accData = result._val2;
                Properties.Settings.Default.password = Glob.accData.Password;
                Properties.Settings.Default.email = Glob.accData.Email;
                Properties.Settings.Default.Save();
                MessageBox.Show("Connected!");
            }
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            ServAcc.Account acc = new ServAcc.Account();
            ServAcc.ResultatOfstringAccountDataGmrGjgq6 result = acc.Login(EmailBox.Text, PasswordBox.Text);
            if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
            {
                Glob.res = result._val1;
                Glob.accData = result._val2;
                Properties.Settings.Default.password = Glob.accData.Password;
                Properties.Settings.Default.email = Glob.accData.Email;
                Properties.Settings.Default.Save();
                MessageBox.Show("Connected!");
            }
            else
                MessageBox.Show("Bad email or password!");
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
                ServFeed.Feed sf = new ServFeed.Feed();
                ServFeed.Resultat feed = sf.AddFeed(Glob.res, URLBox.Text);
                if (feed._error == ServFeed.ResultatErrorCode.SUCCESS)
                    MessageBox.Show("Feed added : " + URLBox.Text);
                else
                    MessageBox.Show("Add Error!");
        }

        private void ShowRegisterBox_Click(object sender, RibbonControlEventArgs e)
        {
            if (group4.Visible == true)
                group4.Visible = false;
            else
                group4.Visible = true;
        }

        private void Register_Click(object sender, RibbonControlEventArgs e)
        {
            ServAcc.Account acc = new ServAcc.Account();
            ServAcc.Resultat result = acc.Register(EmailRegisterBox.Text, UserNameBox.Text, PasswordRegisterBox.Text);
            if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
            {
                ServAcc.ResultatOfstringAccountDataGmrGjgq6 login = acc.Login(EmailRegisterBox.Text, PasswordRegisterBox.Text);
                if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
                {
                    Glob.res = login._val1;
                    Glob.accData = login._val2;
                    MessageBox.Show("Connected!");
                }
                else
                    MessageBox.Show("Bad email or password!");
            }
            else
                MessageBox.Show("Error!");
        }

        private void ChangePassword_Click(object sender, RibbonControlEventArgs e)
        {
            if (CurrentPasswordBox.Text == Glob.accData.Password)
            {
                ServAcc.Account acc = new ServAcc.Account();
                ServAcc.Resultat result = acc.UpdatePassword(Glob.res, NewPasswordBox.Text);
                if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
                {
                    MessageBox.Show("Password Changed!");
                }
                else
                    MessageBox.Show("Error when updating password!");
            }
            else
                MessageBox.Show("Current Password isn't good!");

        }



    }
}
