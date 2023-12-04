using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmMonHoc : Form
    {
        public frmMonHoc(string mamh)               // giá trị của tham số mamh cho trường dữ liệu mamh trong lớp frmMonHoc
        {
            this.mamh = mamh;
            InitializeComponent();
        }

        private string mamh;
        private string nguoithuchien = "admin";
        private void frmMonHoc_Load(object sender, EventArgs e)         // kiểm tra xem biến mamh có trống hay ko
        {
            if(string.IsNullOrEmpty(mamh))                          
            {
                this.Text = "Thêm mới môn học";
            }
            else
            {
                this.Text = "Cập nhật môn học";
                var r = new Database().Select("exec selectMH '"+mamh+"'");  //thực hiện một truy vấn cơ sở dữ liệu để lấy thông tin về môn học với mã mamh
                txtTenMH.Text = r["tenmonhoc"].ToString();                    // thiết lập giá trị của hộp văn bản (txtTenMH) 
                txtSoTC.Text = r["sotinchi"].ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var stc = int.Parse(txtSoTC.Text);          //chuyển đổi giá trị trong hộp văn bản txtSoTC thành một số nguyên và lưu vào biến stc
                if (stc<=0)
                {
                    MessageBox.Show("Số tín chỉ phải lớn hơn 0");
                    txtSoTC.Select();
                    return;
                }
            }
            catch 
            {
                MessageBox.Show("Số tín chỉ phải là kiểu số nguyên");
                txtSoTC.Select();
                return;
            }

            if(string.IsNullOrEmpty(txtTenMH.Text))      //kt xem ten mh có bị trống hay ko
            {
                MessageBox.Show("Tên môn học không được để trống");
                txtTenMH.Select();
                return;
            }


            //khai báo một biến chuỗi sql và khởi tạo nó với giá trị rỗng,Biến lưu trữ câu lệnh SQL dựa trên trạng thái của biến mamh.
            string sql = "";                 
            List<CustomParameter> lstPara = new List<CustomParameter>();  //khai báo một danh sách (List) các tham số tùy chỉnh (CustomParameter).
            if (string.IsNullOrEmpty(mamh))                                 //thêm mới môn học
            {
                sql = "insertMH";
                lstPara.Add(new CustomParameter() {
                        key = "@nguoitao",
                        value = nguoithuchien
                });
            }else
            {                                                   //cập nhật môn học
                lstPara.Add(new CustomParameter()
                {
                    key = "@mamonhoc",
                    value = mamh
                });

                lstPara.Add(new CustomParameter()
                {
                    key = "@nguoicapnhat",
                    value = nguoithuchien
                });
                sql = "updateMH";
            }
            lstPara.Add(new CustomParameter()
            {
                key = "@tenmonhoc",
                value = txtTenMH.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@sotinchi",
                value = txtSoTC.Text
            });


            var rs = new Database().ExeCute(sql,lstPara);

            if(rs == 1)
            {
                if(string.IsNullOrEmpty(mamh))
                {
                    MessageBox.Show("Thêm mới môn học thành công");
                }else
                {
                    MessageBox.Show("Cập nhật thông tin môn học thành công");
                }

                this.Dispose();

            }else
            {
                MessageBox.Show("Thực hiện truy vấn thất bại");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
