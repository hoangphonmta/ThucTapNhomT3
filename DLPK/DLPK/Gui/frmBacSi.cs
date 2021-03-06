﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLPK.BLL;
using DLPK.Entity;
using DLPK.DAO;
using System.Globalization;

namespace DLPK.Gui
{
    
    public partial class frmBacSi : Form
    {
        private string _TenKhoa;
        BacSiBLL Bacsi = new BacSiBLL();
        BacSiDAO bacSi = new BacSiDAO();
        public frmBacSi()
        {
            InitializeComponent();
        }
        void Load_BacSi()
        {
            ChuyenKhoa chuyenKhoa = new ChuyenKhoa(-1, _TenKhoa);
            List<BacSi> ListBacSi = Bacsi.BacSi_Select(chuyenKhoa);
            foreach (BacSi item in ListBacSi)
            {
                lvBacSi.Items.Add(item.TenBS);
            }
            //DataTable data = Bacsi.ViewBS();
            //for(int i= 0;i<data.Rows.Count;i++)
            //{
            //    lvBacSi.Items.Add(data.Rows[i]["TenBS"].ToString());
            //}
        }
        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMain main = new frmMain();
            main.Show();
            this.Close();
        }
        public frmBacSi(string TenKhoa) : this()
        {
            _TenKhoa = TenKhoa;
        }
        private void lsvBacSi_MouseClick(object sender, MouseEventArgs e)
        {
            flpTime.Controls.Clear();
            cbLichKham.Items.Clear();
            string TenBS = lvBacSi.SelectedItems[0].SubItems[0].Text;
            lbTenBS.Text = TenBS;
            BacSi bacSi = new BacSi(-1, TenBS, "", -1, -1, -1, "");
            DataTable ChiTiet_BacSi = Bacsi.BacSi_ChiTiet(bacSi);
            lbPhongKham.Text = ChiTiet_BacSi.Rows[0]["TenPK"].ToString();
            lbDiaChi.Text = ChiTiet_BacSi.Rows[0]["DiaChi"].ToString();
            lbGiaKham.Text = ChiTiet_BacSi.Rows[0]["GiaTien"].ToString();
            lbMota.Text = ChiTiet_BacSi.Rows[0]["GhiChu"].ToString();
            DataTable data = Bacsi.Select_NgayKham(bacSi);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DateTime Ngay = Convert.ToDateTime(data.Rows[i]["Ngay"].ToString());
                string NgayKham = Ngay.ToString("dd/MM/yyyy");
                cbLichKham.Items.Add(NgayKham);
            }
            
        }

        private void frmBacSi_Load(object sender, EventArgs e)
        {
            // LoadTime();
            Load_BacSi();
        }

        public void Load_NgayKham()
        {
            string TenBS = lvBacSi.SelectedItems[0].SubItems[0].Text;
            BacSi bacSi = new BacSi(-1, TenBS, "", -1, -1, -1, "");
            
        }

        private void cbLichKham_SelectedValueChanged(object sender, EventArgs e)
        {
            flpTime.Controls.Clear();
            BacSi bacSi = new BacSi(-1, lbTenBS.Text, "", -1, -1, -1, "");
            string[] formats = {"dd/MM/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd",
                   "dd-MM-yyyy", "M/d/yyyy", "dd MMM yyyy"};
            string converted = DateTime.ParseExact(cbLichKham.Text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("MM/dd/yyyy");
            ThoiGian thoiGian = new ThoiGian(-1, "", converted, -1);
            List<ThoiGian> ListThoiGian = Bacsi.ViewTime(bacSi, thoiGian);
            foreach (ThoiGian item in ListThoiGian)
            {
                Button btn = new Button() { Width = BacSiDAO.TimeWight, Height = BacSiDAO.TimeHight };
                btn.Text = item.Time;
                btn.BackColor = Color.Green;
                flpTime.Controls.Add(btn);
                btn.Click += (object sender1, EventArgs ee) =>
                {
                    frmDatLich datLich = new frmDatLich(lbTenBS.Text, item.Time, cbLichKham.Text);
                    datLich.ShowDialog();
                };
            }
        }
    }
}
