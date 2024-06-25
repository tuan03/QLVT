﻿using DevExpress.XtraGrid;
using QLVT.SubForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormPhieuXuat : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        bool dangThemMoi = false;
        public string makho = "";
        string maChiNhanh = "";

        BindingSource bds = null;
        GridControl gc = null;
        string type = "";
        public FormPhieuXuat()
        {
            InitializeComponent();
        }

        private void phieuXuatBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuXuat.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void FormPhieuXuat_Load(object sender, EventArgs e)
        {
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            dataSet.EnforceConstraints = false;

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);

            this.chiTietPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.chiTietPhieuXuatTableAdapter.Fill(this.dataSet.CTPX);

            /*Step 2*/
            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;
        }

        private void btnTHOAT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnCheDoPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnMENU.Links[0].Caption = "Phiếu Xuất";

            /*Step 1*/
            bds = bdsPhieuXuat;
            gc = gcChiTietPhieuXuat;


            /*Step 2*/
            /*Bat chuc nang cua phieu xuat*/
            txtMaPhieuXuat.Enabled = false;
            dteNgay.Enabled = false;

            txtTenKhachHang.Enabled = true;
            txtMaNhanVien.Enabled = false;

            btnChonKhoHang.Enabled = true;
            txtMaKho.Enabled = false;


            /*Tat chuc nang cua chi tiet phieu nhap*/
            txtMaVatTuChiTietPhieuXuat.Enabled = false;
            btnChonVatTu.Enabled = false;
            txtSoLuongChiTietPhieuXuat.Enabled = false;
            txtDonGiaChiTietPhieuXuat.Enabled = false;

            /*Bat cac grid control len*/
            gcPhieuXuat.Enabled = true;
            gcChiTietPhieuXuat.Enabled = true;


            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {
                cmbChiNhanh.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.groupBoxPhieuNhap.Enabled = false;


            }

            /* CHI NHANH & User co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbChiNhanh.Enabled = false;

                this.btnTHEM.Enabled = true;
                bool turnOn = (bdsPhieuXuat.Count > 0) ? true : false;
                this.btnXOA.Enabled = turnOn;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                //this.txtMaDonDatHang.Enabled = false;

            }
        }

        private void btnCheDoChiTietPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnMENU.Links[0].Caption = "Chi Tiết Phiếu Xuất";

            /*Step 1*/
            bds = bdsChiTietPhieuXuat;
            //gc = gcChiTietPhieuXuat;


            /*Step 2*/
            /*Tat het chuc nang cua phieu xuat*/
            txtMaPhieuXuat.Enabled = false;
            dteNgay.Enabled = false;

            txtTenKhachHang.Enabled = false;
            txtMaNhanVien.Enabled = false;

            btnChonKhoHang.Enabled = false;
            txtMaKho.Enabled = false;


            /*Bat chuc nang cua chi tiet phieu nhap*/
            txtMaVatTuChiTietPhieuXuat.Enabled = false;
            txtSoLuongChiTietPhieuXuat.Enabled = false;
            txtDonGiaChiTietPhieuXuat.Enabled = false;

            /*Bat cac grid control len*/
            gcPhieuXuat.Enabled = true;
            gcChiTietPhieuXuat.Enabled = true;


            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {
                cmbChiNhanh.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.groupBoxPhieuNhap.Enabled = false;


            }

            /* CHI NHANH & User co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbChiNhanh.Enabled = false;

                this.btnTHEM.Enabled = true;
                bool turnOn = (bdsPhieuXuat.Count > 0) ? true : false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                //this.txtMaDonDatHang.Enabled = false;
            }
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.serverName = cmbChiNhanh.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if (cmbChiNhanh.SelectedIndex != Program.brand)
            {
                Program.loginName = Program.remoteLogin;
                Program.loginPassword = Program.remotePassword;
            }
            /*Neu chon trung voi chi nhanh dang dang nhap o formDangNhap*/
            else
            {
                Program.loginName = Program.currentLogin;
                Program.loginPassword = Program.currentPassword;
            }

            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);

                this.chiTietPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.chiTietPhieuXuatTableAdapter.Fill(this.dataSet.CTPX);
            }
        }

        private void btnTHEM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            /*Step 1*/
            /*lấy vị trí hiện tại của con trỏ*/
            viTri = bds.Position;
            dangThemMoi = true;

            /*Step 2*/
            /*AddNew tự động nhảy xuống cuối thêm 1 dòng mới*/
            bds.AddNew();
            if (btnMENU.Links[0].Caption == "Phiếu Xuất")
            {
                this.txtMaPhieuXuat.Enabled = true;

                this.dteNgay.EditValue = DateTime.Now;
                this.dteNgay.Enabled = false;

                this.txtTenKhachHang.Enabled = true;
                this.txtMaNhanVien.Text = Program.userName;

                this.btnChonKhoHang.Enabled = true;
                this.txtMaKho.Text = Program.maKhoDuocChon;

                this.txtMaVatTuChiTietPhieuXuat.Enabled = false;
                this.btnChonVatTu.Enabled = false;
                this.txtSoLuongChiTietPhieuXuat.Enabled = false;
                this.txtDonGiaChiTietPhieuXuat.Enabled = false;
                
                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsPhieuXuat.Current))["NGAY"] = DateTime.Now;
                ((DataRowView)(bdsPhieuXuat.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsPhieuXuat.Current))["MAKHO"] =
                Program.maKhoDuocChon;

            }

            if (btnMENU.Links[0].Caption == "Chi Tiết Phiếu Xuất")
            {

                DataRowView drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không thể thêm chi tiết phiếu xuất trên phiếu  không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

               /*Gan tu dong may truong du lieu nay*/
               ((DataRowView)(bdsChiTietPhieuXuat.Current))["MAPX"] = ((DataRowView)(bdsPhieuXuat.Current))["MAPX"];
                ((DataRowView)(bdsChiTietPhieuXuat.Current))["MAVT"] =
                    Program.maVatTuDuocChon;



                this.btnLAMMOI.Enabled = true;
                this.txtMaVatTuChiTietPhieuXuat.Enabled = false;
                this.btnChonVatTu.Enabled = true;

                this.txtSoLuongChiTietPhieuXuat.Enabled = true;
                this.txtSoLuongChiTietPhieuXuat.EditValue = 1;

                this.txtDonGiaChiTietPhieuXuat.Enabled = true;
                this.txtDonGiaChiTietPhieuXuat.EditValue = 1;
            }

            

            /*Step 3*/
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnHOANTAC.Enabled = true;
            this.btnLAMMOI.Enabled = true;
            this.btnMENU.Enabled = false;
            this.btnTHOAT.Enabled = false;

            gcPhieuXuat.Enabled = false;
            gcChiTietPhieuXuat.Enabled = false;
        }

        private void btnChonKhoHang_Click(object sender, EventArgs e)
        {
            FormChonKhoHang form = new FormChonKhoHang();
            form.ShowDialog();

            this.txtMaKho.Text = Program.maKhoDuocChon;
        }

        private void btnChonVatTu_Click(object sender, EventArgs e)
        {
            FormChonVatTu form = new FormChonVatTu();
            form.ShowDialog();
            this.txtMaVatTuChiTietPhieuXuat.Text = Program.maVatTuDuocChon;
        }


        private bool kiemTraDuLieuDauVao(string cheDo)
        {
            if (cheDo == "Phiếu Xuất")
            {
                DataRowView drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không thể sửa phiếu xuất do người khác tạo", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMaPhieuXuat.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu nhập !", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuXuat.Focus();
                    return false;
                }

                if (txtMaPhieuXuat.Text.Length > 8)
                {
                    MessageBox.Show("Mã phiếu xuất không thể quá 8 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuXuat.Focus();
                    return false;
                }

                if (txtTenKhachHang.Text == "")
                {
                    MessageBox.Show("Không bỏ trống tên khách hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtTenKhachHang.Focus();
                    return false;
                }

                if( txtTenKhachHang.Text.Length > 100)
                {
                    MessageBox.Show("Tên khách hàng không quá 100 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtTenKhachHang.Focus();
                    return false;
                }

                if (txtMaKho.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã kho !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

            }

            if(cheDo == "Chi Tiết Phiếu Xuất")
            {
                DataRowView drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không thể thêm chi tiết phiếu xuất với phiếu xuất do người khác tạo !", "Thông báo", MessageBoxButtons.OK);
                    bdsChiTietPhieuXuat.RemoveCurrent();
                    return false;
                }

                if (txtMaPhieuXuat.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu nhập !", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuXuat.Focus();
                    return false;
                }

                if (txtMaPhieuXuat.Text.Length > 8)
                {
                    MessageBox.Show("Mã phiếu xuất không thể quá 8 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuXuat.Focus();
                    return false;
                }

                if (txtMaVatTuChiTietPhieuXuat.Text == "")
                {
                    MessageBox.Show("Thiếu mã vật tư !", "Thông báo", MessageBoxButtons.OK);
                    txtMaVatTuChiTietPhieuXuat.Focus();
                    return false;
                }

                if( txtMaVatTuChiTietPhieuXuat.Text.Length > 4)
                {
                    MessageBox.Show("Mã vật tư không quá 4 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtMaVatTuChiTietPhieuXuat.Focus();
                    return false;
                }

                if( txtSoLuongChiTietPhieuXuat.Value < 0 || txtSoLuongChiTietPhieuXuat.Value > Program.soLuongVatTu)
                {
                    MessageBox.Show("Số lượng vật tư không thể bé hơn 0 & lớn hơn số lượng vật tư đang có trong kho hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtSoLuongChiTietPhieuXuat.Focus();
                    return false;
                }

                if (txtDonGiaChiTietPhieuXuat.Value < 0)
                {
                    MessageBox.Show("Đơn giá không thể bé hơn 0 VND !", "Thông báo", MessageBoxButtons.OK);
                    txtDonGiaChiTietPhieuXuat.Focus();
                    return false;
                }
            }

            return true;
        }

        private void capNhatSoLuongVatTu(string maVatTu, int soLuong)
        {

            string cauTruyVan = "EXEC sp_CapNhatSoLuongVatTu 'EXPORT','" + maVatTu + "', " + soLuong.ToString();
            int n = Program.ExecSqlNonQuery(cauTruyVan);
        }
        /*
         *Step 1: xac dinh xem minh dang GHI o che do nao
         *Step 2: kiem tra du lieu dau vao
         *Step 3: tao cau truy van hoan tac
         *Step 4: dung stored procedure kiem tra xem phieu nhap da ton tai chua ?
         *Step 5: xu ly du lieu neu co
         */
        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            String cheDo = (btnMENU.Links[0].Caption == "Phiếu Xuất") ? "Phiếu Xuất" : "Chi Tiết Phiếu Xuất";

            /*Step 2*/
            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;

            /*Step 4*/
            String maPhieuXuat = txtMaPhieuXuat.Text.Trim();
            //Console.WriteLine(maPhieuNhap);
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_KiemTraMaPhieuXuat '" +
                    maPhieuXuat + "' " +
                    "SELECT 'Value' = @result";
            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                /*khong co ket qua tra ve thi ket thuc luon*/
                if (Program.myReader == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();

            /*Step 5*/
            int viTriConTro = bdsPhieuXuat.Position;
            int viTriMaPhieuXuat = bdsPhieuXuat.Find("MAPX", maPhieuXuat);

            /*Dang them moi phieu nhap*/
            if (result == 1 && cheDo == "Phiếu Xuất" && viTriMaPhieuXuat != viTriConTro)
            {
                MessageBox.Show("Mã phiếu xuất đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                txtMaPhieuXuat.Focus();
                return;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        /*TH1: them moi phieu nhap*/
                        if (cheDo == "Phiếu Xuất" && dangThemMoi == true)
                        {

                        }

                        /*TH2: them moi chi tiet don hang*/
                        if (cheDo == "Chi Tiết Phiếu Xuất" && dangThemMoi == true)
                        {
                            string maVatTu = txtMaVatTuChiTietPhieuXuat.Text.Trim();
                            int soLuong =(int) txtSoLuongChiTietPhieuXuat.Value;

                            capNhatSoLuongVatTu(maVatTu, soLuong);
                        }

                        /*TH3: chinh sua phieu nhap -> chang co gi co the chinh sua
                         * duoc -> chang can xu ly*/
                        /*TH4: chinh sua chi tiet phieu nhap - > thi chi can may dong lenh duoi la xong*/

                        this.bdsPhieuXuat.EndEdit();
                        this.bdsChiTietPhieuXuat.EndEdit();
                        this.phieuXuatTableAdapter.Update(this.dataSet.PhieuXuat);
                        this.chiTietPhieuXuatTableAdapter.Update(this.dataSet.CTPX);

                        this.txtMaPhieuXuat.Enabled = false;

                        this.btnTHEM.Enabled = true;
                        this.btnXOA.Enabled = true;
                        this.btnGHI.Enabled = true;

                        this.btnHOANTAC.Enabled = true;
                        this.btnLAMMOI.Enabled = true;
                        this.btnMENU.Enabled = true;
                        this.btnTHOAT.Enabled = true;

                        this.gcPhieuXuat.Enabled = true;
                        this.gcChiTietPhieuXuat.Enabled = true;
                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        bds.RemoveCurrent();
                        MessageBox.Show("Da xay ra loi !\n\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }


        private void btnHOANTAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.btnXOA.Enabled = true;
                string cheDo = (btnMENU.Links[0].Caption == "Phiếu Xuất") ? "Phiếu Xuất" : "Chi Tiết Phiếu Xuất";
                this.btnTHEM.Enabled = true;
                this.btnGHI.Enabled = true;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                gcPhieuXuat.Enabled = true;
                gcChiTietPhieuXuat.Enabled = true;
                if (cheDo == "Chi Tiết Phiếu Xuất") this.btnXOA.Enabled = false;
                this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);
                this.chiTietPhieuXuatTableAdapter.Fill(this.dataSet.CTPX);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Loi lam moi \n\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRowView drv;
            string cheDo = (btnMENU.Links[0].Caption == "Phiếu Xuất") ? "Phiếu Xuất" : "Chi Tiết Phiếu Xuất";

            if (cheDo == "Phiếu Xuất")
            {
                drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsChiTietPhieuXuat.Count > 0)
                {
                    MessageBox.Show("Không thể xóa vì có chi tiết phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                DateTime ngay = ((DateTime)drv["NGAY"]);

            }

            if (cheDo == "Chi Tiết Phiếu Xuất")
            {
                drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                drv = ((DataRowView)bdsChiTietPhieuXuat[bdsChiTietPhieuXuat.Position]);
            }



            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bds.Position;
                    if (cheDo == "Phiếu Xuất")
                    {
                        bdsPhieuXuat.RemoveCurrent();
                    }
                    if (cheDo == "Chi Tiết Phiếu Nhập")
                    {
                        bdsChiTietPhieuXuat.RemoveCurrent();
                    }


                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Update(this.dataSet.PhieuXuat);

                    this.chiTietPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietPhieuXuatTableAdapter.Update(this.dataSet.CTPX);

                    //bdsPhieuNhap.Position = viTri;
                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    dangThemMoi = false;
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnHOANTAC.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Update(this.dataSet.PhieuXuat);

                    this.chiTietPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietPhieuXuatTableAdapter.Update(this.dataSet.CTPX);
                    // tro ve vi tri cua nhan vien dang bi loi
                    bds.Position = viTri;
                    //bdsNhanVien.Position = bdsNhanVien.Find("MANV", manv);
                    return;
                }
            }
            else
            {
            }
        }
    }
}
