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

namespace Disconnected_mimari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Server=localhost;Database=Northwind;
            Integrated Security=true ");
        private void Form1_Load(object sender, EventArgs e)
        {
            //Disconnected Mimari yontemi ile veri listeleme işlemidir.
            UrunListesi();

        }

        private void UrunListesi()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select* from Urunler where Sonlandi=0", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["UrunID"].Visible = false;
            dataGridView1.Columns["KategoriID"].Visible = false;
            dataGridView1.Columns["TedarikciID"].Visible = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {

            string adi = txtUrunAdi.Text;
            decimal fiyat = nudFiyat.Value;
            decimal stok = nudStok.Value;
            if (adi==""||fiyat==null||stok==null)
            {
                MessageBox.Show("Lutfen tum alanlari doldurunuz ");
                return;
            }
            
                SqlCommand komut = new SqlCommand();
                komut.CommandText = string.Format(@"insert Urunler (UrunAdi,BirimFiyati,HedefStokDuzeyi)
            values('{0}',{1},{2})", adi, fiyat, stok);
                komut.Connection = baglanti;
                baglanti.Open();
                int etkilenen = komut.ExecuteNonQuery();
                if (etkilenen > 0)
                {

                    MessageBox.Show("Urun basarili bir sekilde eklendi");
                    UrunListesi();
                }
                else
                {
                    MessageBox.Show("Eklenme basarisiz");

                }
                baglanti.Close();
           
    


        }

        private void btnKategoriler_Click(object sender, EventArgs e)
        {
            KategoriForm kf = new KategoriForm();
            kf.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //datagriedview'nin secili satiri alma islemi
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["UrunAdi"].Value.ToString();
            txtUrunAdi.Tag = dataGridView1.CurrentRow.Cells["UrunID"].Value;
            nudFiyat.Value = (decimal)dataGridView1.CurrentRow.Cells["BirimFiyati"].Value;
            nudStok.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["HedefStokDuzeyi"].Value);

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.CommandText = string.Format(@"update Urunler set UrunAdi='{0}',BirimFiyati={1}
            ,HedefStokDuzeyi={2} where UrunID={3}", txtUrunAdi.Text,Convert.ToDecimal(nudFiyat.Value.ToString().Replace(",",".")),nudStok.Value,
            txtUrunAdi.Tag);
            komut.Connection = baglanti;
            baglanti.Open();
           
            try
            {
                int etk = komut.ExecuteNonQuery();
                baglanti.Close();
                if (etk > 0)
                {
                    MessageBox.Show("Kayit Guncellendi");
                    UrunListesi();

                }
                else
                {
                    MessageBox.Show("Islem basarisiz");
                }
            }
            catch (Exception ex)
            {

                baglanti.Close();
                MessageBox.Show("Islem basarisiz"+ex.Message);
            }
            
           

        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow!=null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UrunID"].Value);
                SqlCommand cmd = new SqlCommand(string.Format(@"delete Urunler where UrunID={0}
                ",id),baglanti);
                baglanti.Open();
                int etk = cmd.ExecuteNonQuery();
                baglanti.Close();
                if (etk>0)
                {
                    MessageBox.Show("Kayit Silinmisdir");
                    UrunListesi();
                }
                else
                {
                    MessageBox.Show("Islem basarisiz");
                }


            }
        }
    }
}
