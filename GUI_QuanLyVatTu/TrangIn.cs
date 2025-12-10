using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;
namespace GUI_QuanLyVatTu
{
    public partial class TrangIn : Form
    {
        public TrangIn()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            DAL_InHoaDon dAL_InHoaDon = new DAL_InHoaDon();
            List<InHoaDon> inHoaDons = dAL_InHoaDon.SelectAll();
            dgvHoaDon.DataSource = inHoaDons;

        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void TrangIn_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
