using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuizMahasiswaa1
{
    
    public partial class MasterBarang : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-MHR7TR8T;Initial Catalog=QuizMahasiswa;Integrated Security=True;");
        public MasterBarang()
        {
            InitializeComponent();
        }
        DataClasses1DataContext db = new DataClasses1DataContext();
        void LoadData()
        {
            var st = from tb in db.tbl_barangs select tb;
            dataGridView1.DataSource = st;
        }

        private void MasterBarang_Load(object sender, EventArgs e)
        {
            LoadData();
            autoid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int id = int.Parse(TxtBoxID.Text);
            string namabarang = txtNamaBarang.Text;
            int harga = int.Parse(txtHarga.Text);
            int stock = int.Parse(txtStock.Text);
            string namasupplier = txtSupplier.Text;

            var data = new tbl_barang
            {
                id_barang = id,
                nama_barang = namabarang,
                harga = harga,
                stok = stock,
                nama_supplier = namasupplier
            };

            db.tbl_barangs.InsertOnSubmit(data);
            db.SubmitChanges();
            MessageBox.Show("Save Succesfully");
            clear();
            LoadData();
            autoid();
        }

        void autoid()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select isnull(max (cast (id_barang as int)),0) + 1 from tbl_barang", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            TxtBoxID.Text = dt.Rows[0][0].ToString();
            con.Close();
        }

        void clear()
        {
            TxtBoxID.Clear();
            txtNamaBarang.Clear();
            txtNamaSupplier.Clear();
            txtNamaCari.Clear();
            txtSupplier.Clear();
            txtHarga.Clear();
            txtStock.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(TxtBoxID.Text);
            string namabarang = txtNamaBarang.Text;
            int harga = int.Parse(txtHarga.Text);
            int stock = int.Parse(txtStock.Text);
            string namasupplier = txtNamaSupplier.Text;

            var data = (from s in db.tbl_barangs where s.nama_barang == txtNamaBarang.Text select s).First();

            data.nama_barang = namabarang;
            data.harga = harga;
            data.stok = stock;
            data.nama_supplier = namasupplier;
            db.SubmitChanges();
            MessageBox.Show("Update Succesfuly");
            clear();
            LoadData();
            autoid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var data = from s in db.tbl_barangs where s.nama_barang == txtNamaCari.Text || s.nama_supplier == txtNamaSupplier.Text select s;
            dataGridView1.DataSource = data;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var data = (from s in db.tbl_barangs where s.nama_barang == txtNamaCari.Text select s).First();
            db.tbl_barangs.DeleteOnSubmit(data);
            db.SubmitChanges();
            MessageBox.Show("Delete Succesfully");
            clear();
            LoadData();
            autoid();
        }
    }
}
