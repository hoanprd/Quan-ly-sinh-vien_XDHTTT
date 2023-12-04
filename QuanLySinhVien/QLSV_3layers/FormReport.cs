using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLSV_3layers
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
        }
        private string tukhoa = "";
        private void LoadDSSV()
        {
        
            //khai báo list customparameter
            List<CustomParameter> lstPara = new List<CustomParameter>();
            lstPara.Add(new CustomParameter()
            {
                key = "@tukhoa",
                value = tukhoa
            });
            dataGridView1.DataSource = new Database().SelectData("SelectAllSinhVien", lstPara);

            //đặt tên cột
            dataGridView1.Columns["masinhvien"].HeaderText = "Mã SV";
            dataGridView1.Columns["hoten"].HeaderText = "Họ tên";
            dataGridView1.Columns["nsinh"].HeaderText = "Ngày sinh";
            dataGridView1.Columns["gt"].HeaderText = "Giới tính";
            dataGridView1.Columns["quequan"].HeaderText = "Quê quán";
            dataGridView1.Columns["diachi"].HeaderText = "Địa chỉ";
            dataGridView1.Columns["email"].HeaderText = "Email";
            dataGridView1.Columns["dienthoai"].HeaderText = "Điện thoại";
        }
        private void FormReport_Load(object sender, EventArgs e)
        {
            LoadDSSV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)                      //Kiểm tra dữ liệu trong dataGridView1.
            {
                SaveFileDialog save = new SaveFileDialog();      //Tạo một đối tượng SaveFileDialog  cho phép hien vị trí và tên của tệp muốn lưu.
                save.Filter = "PDF (*.pdf)|*.pdf";                //chỉ cho phép lưu tệp dưới định dạng PDF.
                save.FileName = "Result.pdf";                     
                bool ErrorMessage = false;                          //  Khai báo một biến ErrorMessage để theo dõi xem có lỗi xảy ra trong quá trình xuất dữ liệu hay không
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))                 
                    {
                        try
                        {
                            File.Delete(save.FileName);              //kiểm tra xem  vị trí và tên tệp lưu và có tệp tồn tại ở vị trí đó không
                        }                                            //tệp đã tồn tại, nó cố gắng xóa tệp đó,
                        catch (Exception ex)
                        {

                            ErrorMessage = true;
                            MessageBox.Show("Unable to wride data in disk" + ex.Message);   //có lỗi xảy ra trong quá trình xóa,hiển thị một thông báo lỗi
                        }
                    }
                    if (!ErrorMessage)         //kiểm tra biến ErrorMessage để đảm bảo rằng không có lỗi xảy ra trong các bước trước đó
                    {
                        try
                        {
                            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H,true); //Tạo một đối tượng BaseFont vs kiểu chữ Times New Roman
                            PdfPTable pTable = new PdfPTable(dataGridView1.Columns.Count);                                      //Tạo một bảng PDF
                            pTable.DefaultCell.Padding = 2;                                     //khoảng cách trong ô
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;          
                            var font = new iTextSharp.text.Font(baseFont, 12);        // tạo một đối tượng Font ,sử dụng phông chữ trc đó với cỡ chữ 12

                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {                                                                           //Đối tượng PdfPCell được tạo ra
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));           // đối tượng Phrase mới được tạo với nội dung là tiêu đề của cột
                                pTable.AddCell(pCell);                                                  //Đối tượng PdfPCell này được thêm vào bảng PDF
                            }
                            foreach (DataGridViewRow viewRow in dataGridView1.Rows)                 //duyệt qua từng ô dữ liệu trong dataGridView1
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)                   // duyệt qua từng ô dữ liệu trong từng dòng  của dataGridView1
                                {
                                    pTable.AddCell(new Phrase(dcell.Value.ToString(),font));
                                }
                            }


                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data Export Successfully", "info");

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show("Error while exporting Data" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record Found", "Info");

            }
        }
    }
}
