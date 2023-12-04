using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string duongdan = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string kiemTraDangKyPath = duongdan + @"\QLSV\checkdk.txt";

            if (!Directory.Exists(duongdan + @"\QLSV"))
            {
                Directory.CreateDirectory(duongdan + "//QLSV");
            }

            if (!File.Exists(kiemTraDangKyPath))
            {
                using (FileStream fs = File.Create(kiemTraDangKyPath))
                {
                    // Không cần thực hiện bất kỳ ghi nào vào FileStream
                }
            }

            Application.Run(new frmMain());//set form chạy đầu tiên là form frmMain
        }
    }
}
