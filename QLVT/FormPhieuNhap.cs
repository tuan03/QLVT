using DevExpress.XtraGrid;
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
    public partial class FormPhieuNhap : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        /********************************************
         * đang thêm mới -> true -> đang dùng btnTHEM
         *              -> false -> có thể là btnGHI( chỉnh sửa) hoặc btnXOA
         *              
         * Mục đích: dùng biến này để phân biệt giữa btnTHEM - thêm mới hoàn toàn
         * và việc chỉnh sửa nhân viên( do mình ko dùng thêm btnXOA )
         * Trạng thái true or false sẽ được sử dụng 
         * trong btnGHI - việc này để phục vụ cho btnHOANTAC
         ********************************************/
        bool dangThemMoi = false;
        public string makho = "";
        string maChiNhanh = "";
 
        /********************************************************
         * chứa những dữ liệu hiện tại đang làm việc
         * gc chứa grid view đang làm việc
         ********************************************************/
        BindingSource bds = null;
        GridControl gc = null;
        string type = "";


        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            dataSet.EnforceConstraints = false;

            this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

            /*van con ton tai loi chua sua duoc*/
            //maChiNhanh = ((DataRowView)bdsVatTu[0])["MACN"].ToString();

            /*Step 2*/
            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;
        }

        private void cmbChiNhanh_SelectedIndexChanged_1(object sender, EventArgs e)
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
                this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
            }
        }

        private void btnCheDoPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnMENU.Links[0].Caption = "Phiếu Nhập";

            /*Step 1*/
            bds = bdsPhieuNhap;
            gc = gcPhieuNhap;


            /*Step 2*/
            /*Bat chuc nang cua phieu nhap*/
            txtMaPhieuNhap.Enabled = false;
            dteNgay.Enabled = false;

            txtMaDonDatHang.Enabled = false;
            btnChonDonHang.Enabled = false;

            txtMaNhanVien.Enabled = false;
            txtMaKho.Enabled = false;

            btnChonChiTietDonHang.Enabled = false;

            /*Tat chuc nang cua chi tiet phieu nhap*/
            txtMaVatChiTietPhieuNhap.Enabled = false;
            txtSoLuongChiTietPhieuNhap.Enabled = false;
            txtDonGiaChiTietPhieuNhap.Enabled = false;

            /*Bat cac grid control len*/
            gcPhieuNhap.Enabled = true;
            gcChiTietPhieuNhap.Enabled = true;


            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {

                cmbChiNhanh.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

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
                bool turnOn = (bdsPhieuNhap.Count > 0) ? true : false;
                this.btnXOA.Enabled = turnOn;
                this.btnGHI.Enabled = true;

                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                //this.txtMaDonDatHang.Enabled = false;

            }
        }

        private void btnCheDoChiTietPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnMENU.Links[0].Caption = "Chi Tiết Phiếu Nhập";

            /*Step 1*/
            bds = bdsChiTietPhieuNhap;
            gc = gcPhieuNhap;

            /*Step 2*/
            /*Tat chuc nang cua chi tiet phieu nhap*/
            txtMaPhieuNhap.Enabled = false;
            dteNgay.Enabled = false;

            txtMaDonDatHang.Enabled = false;
            btnChonDonHang.Enabled = false;

            txtMaNhanVien.Enabled = false;
            txtMaKho.Enabled = false;

            /*Bat chuc nang cua chi tiet don hang*/
            txtMaVatTu.Enabled = false;
            txtSoLuongChiTietPhieuNhap.Enabled = false;
            txtDonGiaChiTietPhieuNhap.Enabled = false;

            btnChonChiTietDonHang.Enabled = false;

            /*Bat cac grid control len*/
            gcPhieuNhap.Enabled = true;
            gcChiTietPhieuNhap.Enabled = true;

            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {
                cmbChiNhanh.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;
            }

            /* CHI NHANH & User co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbChiNhanh.Enabled = false;

                this.btnTHEM.Enabled = true;
                //bool turnOn = (bdsChiTietPhieuNhap.Count > 0) ? true : false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = true;

                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;
            }
        }

        /***************************************************
         * Hiện mã đơn hàng , mã kho được chọn
         ***************************************************/
        private void btnChonDonHang_Click(object sender, EventArgs e)
        {
            FormChonDonDatHang form = new FormChonDonDatHang();
            form.ShowDialog();

            this.txtMaDonDatHang.Text = Program.maDonDatHangDuocChon;
            this.txtMaKho.Text = Program.maKhoDuocChon;
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
            if (btnMENU.Links[0].Caption == "Phiếu Nhập")
            {
                this.txtMaPhieuNhap.Enabled = true;

                this.dteNgay.EditValue = DateTime.Now;
                this.dteNgay.Enabled = false;

                this.txtMaDonDatHang.Enabled = false;
                this.btnChonDonHang.Enabled = true;

                this.txtMaNhanVien.Text = Program.userName;
                this.txtMaKho.Text = Program.maKhoDuocChon;


                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsPhieuNhap.Current))["NGAY"] = DateTime.Now;
                ((DataRowView)(bdsPhieuNhap.Current))["MasoDDH"] = Program.maDonDatHangDuocChon;
                ((DataRowView)(bdsPhieuNhap.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsPhieuNhap.Current))["MAKHO"] =
                Program.maKhoDuocChon;

            }

            if (btnMENU.Links[0].Caption == "Chi Tiết Phiếu Nhập")
            {
                DataRowView drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không thêm chi tiết phiếu nhập trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    bdsChiTietPhieuNhap.RemoveCurrent();
                    return;
                }

                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["MAPN"] = ((DataRowView)(bdsPhieuNhap.Current))["MAPN"];
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["MAVT"] =
                    Program.maVatTuDuocChon;
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["SOLUONG"] =
                    Program.soLuongVatTu;
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["DONGIA"] =
                    Program.donGia;

                this.txtMaVatTu.Enabled = false;
                this.btnChonChiTietDonHang.Enabled = true;

                this.txtSoLuong.Enabled = true;
                this.txtSoLuong.EditValue = 1;

                this.txtDonGia.Enabled = true;
                this.txtDonGia.EditValue = 1;

                this.txtSoLuongChiTietPhieuNhap.Enabled = true;
                this.txtDonGiaChiTietPhieuNhap.Enabled = true;
            }


            /*Step 3*/
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnLAMMOI.Enabled = true;
            this.btnMENU.Enabled = false;
            this.btnTHOAT.Enabled = false;

            gcPhieuNhap.Enabled = false;
            gcChiTietPhieuNhap.Enabled = false;
        }

        private void btnTHOAT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.gcPhieuNhap.Enabled = true;
                this.gcChiTietPhieuNhap.Enabled = true;

                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
                this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi làm mời dữ liệu\n\n" + ex.Message, "Thông Báo", MessageBoxButtons.OK);
                Console.WriteLine(ex.Message);
                return;
            }
        }


        private void btnChonChiTietDonHang_Click(object sender, EventArgs e)
        {
            /*Lay MasoDDH hien tai cua gcPhieuNhap de so sanh voi MasoDDH se duoc chon*/
            Program.maDonDatHangDuocChon = ((DataRowView)(bdsPhieuNhap.Current))["MasoDDH"].ToString().Trim();
            Program.maPhieuNhapDuocChon = txtMaPhieuNhap.Text;
            //Console.WriteLine(Program.maDonDatHangDuocChon);
            FormChonChiTietDonHang form = new FormChonChiTietDonHang();
            form.ShowDialog();



            this.txtMaVatChiTietPhieuNhap.Text = Program.maVatTuDuocChon;
            this.txtSoLuongChiTietPhieuNhap.Value = Program.soLuongVatTu;
            this.txtDonGiaChiTietPhieuNhap.Value = Program.donGia;
        }



   
         private void capNhatSoLuongVatTu(string maVatTu, int soLuong)
        {
            string cauTruyVan = "EXEC sp_CapNhatSoLuongVatTu 'IMPORT','" + maVatTu + "', " + soLuong;


            int n = Program.ExecSqlNonQuery(cauTruyVan);
            Console.WriteLine(cauTruyVan);
        }
        private bool kiemTraDuLieuDauVao(String cheDo)
        {
            if (cheDo == "Phiếu Nhập")
            {
                if (txtMaPhieuNhap.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu nhập !", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuNhap.Focus();
                    return false;
                }


                if (txtMaNhanVien.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã nhân viên !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMaKho.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã kho !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMaDonDatHang.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã đơn đặt hàng !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
            }

            if (cheDo == "Chi Tiết Phiếu Nhập")
            {
                if (txtMaVatChiTietPhieuNhap.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã vật tư !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtSoLuongChiTietPhieuNhap.Value < 0 ||
                    txtSoLuongChiTietPhieuNhap.Value > Program.soLuongVatTu)
                {
                    MessageBox.Show("Số lượng vật tư không thể lớn hơn số lượng vật tư trong chi tiết đơn hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtSoLuongChiTietPhieuNhap.Focus();
                    return false;
                }

                if (txtDonGiaChiTietPhieuNhap.Value < 0)
                {
                    MessageBox.Show("Đơn giá không thể nhỏ hơn 0 VND", "Thông báo", MessageBoxButtons.OK);
                    txtDonGiaChiTietPhieuNhap.Focus();
                    return false;
                }
            }

            return true;
        }


        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            String cheDo = (btnMENU.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";


            /*Step 2*/
            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;



            /*Step 3*/


            /*Step 4*/
            String maPhieuNhap = txtMaPhieuNhap.Text.Trim();
            //Console.WriteLine(maPhieuNhap);
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_KiemTraMaPhieuNhap '" +
                    maPhieuNhap + "' " +
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
            int viTriConTro = bdsPhieuNhap.Position;
            int viTriMaPhieuNhap = bdsPhieuNhap.Find("MAPN", maPhieuNhap);

            /*Dang them moi phieu nhap*/
            if (result == 1 && cheDo == "Phiếu Nhập" && viTriMaPhieuNhap != viTriConTro)
            {
                MessageBox.Show("Mã phiếu nhập đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                txtMaPhieuNhap.Focus();
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
                        //Console.WriteLine(txtMaNhanVien.Text);
                        /*TH1: them moi phieu nhap*/
                        if (cheDo == "Phiếu Nhập" && dangThemMoi == true)
                        {

                        }

                        /*TH2: them moi chi tiet don hang*/
                        if (cheDo == "Chi Tiết Phiếu Nhập" && dangThemMoi == true)
                        {
                            string maVatTu = txtMaVatChiTietPhieuNhap.Text.Trim();
                            int soLuong = (int)txtSoLuongChiTietPhieuNhap.Value;

                            capNhatSoLuongVatTu(maVatTu, soLuong);
                        }

                        /*TH3: chinh sua phieu nhap -> chang co gi co the chinh sua
                         * duoc -> chang can xu ly*/
                        /*TH4: chinh sua chi tiet phieu nhap - > thi chi can may dong lenh duoi la xong*/

                        this.bdsPhieuNhap.EndEdit();
                        this.bdsChiTietPhieuNhap.EndEdit();
                        this.phieuNhapTableAdapter.Update(this.dataSet.PhieuNhap);
                        this.chiTietPhieuNhapTableAdapter.Update(this.dataSet.CTPN);

                        this.btnTHEM.Enabled = true;
                        this.btnXOA.Enabled = true;
                        this.btnGHI.Enabled = true;

                        this.btnLAMMOI.Enabled = true;
                        this.btnMENU.Enabled = true;
                        this.btnTHOAT.Enabled = true;

                        this.gcPhieuNhap.Enabled = true;
                        this.gcChiTietPhieuNhap.Enabled = true;

                        this.txtSoLuongChiTietPhieuNhap.Enabled = false;
                        this.txtDonGiaChiTietPhieuNhap.Enabled = false;
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

        private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRowView drv;
            string cheDo = (btnMENU.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";



            if (cheDo == "Phiếu Nhập")
            {
                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsChiTietPhieuNhap.Count > 0)
                {
                    MessageBox.Show("Không thể xóa phiếu nhập vì có chi tiết phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                DateTime ngay = ((DateTime)drv["NGAY"]);

            }

            if (cheDo == "Chi Tiết Phiếu Nhập")
            {
                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết phiếu nhập không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                drv = ((DataRowView)bdsChiTietPhieuNhap[bdsChiTietPhieuNhap.Position]);
            }



            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bds.Position;
                    if (cheDo == "Phiếu Nhập")
                    {
                        bdsPhieuNhap.RemoveCurrent();
                    }
                    if (cheDo == "Chi Tiết Phiếu Nhập")
                    {
                        bdsChiTietPhieuNhap.RemoveCurrent();
                    }


                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.dataSet.PhieuNhap);

                    this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietPhieuNhapTableAdapter.Update(this.dataSet.CTPN);

                    //bdsPhieuNhap.Position = viTri;
                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    dangThemMoi = false;
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.dataSet.PhieuNhap);

                    this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietPhieuNhapTableAdapter.Update(this.dataSet.CTPN);
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