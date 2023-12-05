using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Words;
using ThuVienWinform.Report.AsposeWordExtension;
using Aspose.Words.Tables;

namespace QLSV_3layers
{
    public partial class frmBaoCaoDSSV : Form
    {
        DataGridViewRow r;

        private int row = 0;
        private string tukhoa = "", dir;

        public frmBaoCaoDSSV()
        {
            InitializeComponent();
        }

        private void frmBaoCaoDSSV_Load(object sender, EventArgs e)
        {
            dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            ShowData();
        }

        private void ShowData()
        {
            //khai báo list customparameter
            List<CustomParameter> lstPara = new List<CustomParameter>();
            lstPara.Add(new CustomParameter()
            {
                key = "@tukhoa",
                value = tukhoa
            });
            dgvBaoCao.DataSource = new Database().SelectData("SelectAllSinhVien", lstPara);

            dgvBaoCao.Columns["masinhvien"].HeaderText = "Mã SV";
            dgvBaoCao.Columns["hoten"].HeaderText = "Họ tên";
            dgvBaoCao.Columns["nsinh"].HeaderText = "Ngày sinh";
            dgvBaoCao.Columns["gt"].HeaderText = "Giới tính";
            dgvBaoCao.Columns["quequan"].HeaderText = "Quê quán";
            dgvBaoCao.Columns["diachi"].HeaderText = "Địa chỉ";
            dgvBaoCao.Columns["email"].HeaderText = "Email";
            dgvBaoCao.Columns["dienthoai"].HeaderText = "Điện thoại";

            row = dgvBaoCao.Rows.Count - 1;
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            var homNay = DateTime.Now;

            Document baoCao = new Document("Mau_Bao_Cao_SinhVien.doc");

            baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { homNay.ToString() });

            Table bangThongTin = baoCao.GetChild(NodeType.Table, 1, true) as Table;
            int hangHienTai = 1;
            bangThongTin.InsertRows(hangHienTai, hangHienTai, row);
            for (int i = 1; i <= row; i++)
            {
                r = dgvBaoCao.Rows[hangHienTai - 1];

                bangThongTin.PutValue(hangHienTai, 0, r.Cells["masinhvien"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 1, r.Cells["hoten"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 2, r.Cells["nsinh"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 3, r.Cells["gt"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 4, r.Cells["quequan"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 5, r.Cells["diachi"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 6, r.Cells["email"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 7, r.Cells["dienthoai"].Value.ToString());
                hangHienTai++;
            }

            try
            {
                baoCao.Save("BaoCaoSinhVien.docx");
                string temp = Application.StartupPath.ToString() + "/BaoCaoSinhVien.docx";
                string sourceFilePath = temp;
                string outputFilePath = dir + "/BaoCaoSinhVien.pdf";

                ConvertToPdf(sourceFilePath, outputFilePath);
                System.Diagnostics.Process.Start(outputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ConvertToPdf(string sourceFilePath, string outputFilePath)
        {
            Document doc = new Document(sourceFilePath);

            doc.Save(outputFilePath, SaveFormat.Pdf);
        }
    }
}
