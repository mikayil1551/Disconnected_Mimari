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
namespace Disconnected_mimari
{
    public partial class KategoriForm : Form
    {
        public KategoriForm()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Server=localhost;DataBase=Northwind;
            Integrated Security=true");
        private void KategoriForm_Load(object sender, EventArgs e)
        {
            KategorilerListesi();
        }

        private void KategorilerListesi()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select * from Kategoriler", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgvKategoriler.DataSource = dt;
            dgvKategoriler.Columns["KategoriID"].Visible = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string adi = txtAdi.Text;
            string tanimi = txtTanimi.Text;
            if (adi==""||tanimi=="")
            {
                MessageBox.Show("Lutfen gerekli alanlari doldurunuz");
                return;
            }
           
                SqlCommand komut = new SqlCommand();
                komut.CommandText = string.Format(@"insert Kategoriler(KategoriAdi,Tanimi) 
                values('{0}','{1}')", adi, tanimi);
                komut.Connection = baglanti;
                baglanti.Open();
                int etkilenen = komut.ExecuteNonQuery();
                if (etkilenen > 0)
                {
                    MessageBox.Show("Veri basariyla eklendi");
                    KategorilerListesi();
                }
                else
                {
                    
                    MessageBox.Show("Ekleme basarisiz");
                }

                baglanti.Close();
            
            
        }
    }
}
