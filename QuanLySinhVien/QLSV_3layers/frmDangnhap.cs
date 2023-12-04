﻿using System;
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
    public partial class frmDangnhap : Form
    {
        public frmDangnhap()
        {
            InitializeComponent();
        }
        public static string maSVIndex, matKhauThayHienTai, loaiTK;
        public string tendangnhap = "";
        public string loaitk;
        private void btnThoat_Click(object sender, EventArgs e)   //cho phép người dùng thoát khỏi ứng dụng bằng cách gọi Application.Exit().
        {
            frmMain.thoatDangNhap = true;
            Application.Exit();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)      //Kiểm tra ràng buộc để đảm bảo người dùng đã chọn loại tài khoản, 
        {                                                                  //nhập tên đăng nhập và mật khẩu
            #region ktra_rbuoc
            //kiểm tra ràng buộc
            if (cbbLoaiTaiKhoan.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn loại tài khoản");
                return;
            }

            if(string.IsNullOrEmpty(txtTendangnhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản","Tài khoản không được để trống");
                txtTendangnhap.Select();
                return;
            }

            if(string.IsNullOrEmpty(txtMatkhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu","Mật khẩu không thể để trống");
            }
            #endregion


            tendangnhap = txtTendangnhap.Text;
            loaitk = "";

            #region swtk
            switch (cbbLoaiTaiKhoan.Text)
            {
                case "Quản trị viên":
                    loaitk="admin";
                    break;
                case "Giáo viên":
                    loaitk = "gv";
                    break;
                case "Sinh viên":
                    loaitk = "sv";                    
                    break;
            }
            #endregion


            List<CustomParameter> lst = new List<CustomParameter>()  //Xác định loại tài khoản dựa trên giá trị đã chọn từ ComboBox 
            {
                new CustomParameter()
                {
                    key = "@loaitaikhoan",
                    value=loaitk
                },
                 new CustomParameter()
                {
                    key = "@taikhoan",
                    value=txtTendangnhap.Text
                },
                  new CustomParameter()
                {
                    key = "@matkhau",
                    value=txtMatkhau.Text
                },
            };

            var rs = new Database().SelectData("dangnhap", lst);  //Gọi phương thức SelectData từ đối tượng Database để kiểm tra thông tin đăng nhập.
            if (rs.Rows.Count>0)
            {
                MessageBox.Show("Đăng nhập thành công");
                maSVIndex = txtTendangnhap.Text;
                matKhauThayHienTai = txtMatkhau.Text;
                loaiTK = loaitk;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại tên đăng nhập hoặc mật khẩu","Tài khoản hoặc mật khẩu không hợp lệ");
            }
            
        }

        private void frmDangnhap_Load(object sender, EventArgs e)
        {

        }
    }
}