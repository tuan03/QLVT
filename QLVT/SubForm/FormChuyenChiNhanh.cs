using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.SubForm
{
    public partial class FormChuyenChiNhanh : DevExpress.XtraEditors.XtraForm
    {
        
        public FormChuyenChiNhanh()
        {
            InitializeComponent();
        }
       

        private void FormChuyenChiNhanh_Load(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)Program.bindingSource.DataSource;

            // Tạo DataView và lọc bỏ các hàng có tenserver chứa số "1"
            DataView dataView = new DataView(dataTable);
            String filter = "tenserver NOT LIKE '%" + (Program.brand + 1).ToString() + "%'";
            dataView.RowFilter = filter;

            cmbChiNhanh.DataSource = dataView;
            cmbChiNhanh.DisplayMember = "tencn";
            cmbChiNhanh.ValueMember = "tenserver";
            //cmbChiNhanh.SelectedIndex = Program.brand;
        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }



        /************************************************************
         * tạo delegate - một cái biến mà khi được gọi, nó sẽ thực hiện 1 hàm(function) khác
         * Ví dụ: ở class formNhanVien, ta có hàm chuyển chi nhánh, hàm này cần 1 tham số, chính
         * là tên server được chọn ở formChuyenChiNhanh này. Để gọi được hàm chuyển chi nhánh ở formNHANVIEN
         * Chúng ta khai báo 1 delete là branchTransfer để gọi hàm chuyển chi nhánh về form này
         *************************************************************/
        public delegate void MyDelegate(string ChiNhanh);
        public MyDelegate branchTransfer;
        private void btnXACNHAN_Click(object sender, EventArgs e)
        {
            if (cmbChiNhanh.Text.Trim().Equals(""))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            /*Step 2*/
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn chuyển nhân viên này đi ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if( dialogResult == DialogResult.OK)
            {
                branchTransfer(cmbChiNhanh.SelectedValue.ToString());
            }
                
            this.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}