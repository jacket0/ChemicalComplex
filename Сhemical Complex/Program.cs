using OfficeOpenXml;
using System;
using System.Windows.Forms;

namespace Сhemical_Complex
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var enterForm = new EnterForm())
            {
                if (enterForm.ShowDialog() == DialogResult.OK)
                {
                    Form form;

                    switch (enterForm.UserRole.ToLower())
                    {
                        case "admin":
                            form = new AdminForm();
                            break;
                        case "user":
                            form = new MainForm();
                            break;
                        default:
                            MessageBox.Show("Неизвестная роль пользователя");
                            return;
                    }

                    Application.Run(form);
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
