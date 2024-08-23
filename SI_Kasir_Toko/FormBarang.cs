using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Org.BouncyCastle.Math;

namespace SI_Kasir_Toko
{
    using static GlobalVariable;
    public partial class FormBarang : Form
    {
        public string nameBarang;
        public FormBarang()
        {
            InitializeComponent();

        }

        private void loadDataBarang()
        {
            var dataBarang = from barang in Db.Barcode2s
                             select new
                             {
                                 Kode = barang.BarcodeID,
                                 Nama = barang.Nama,
                             };
            dataGridView1.DataSource = dataBarang.ToList();
        }

        private void FormBarang_Load(object sender, EventArgs e)
        {
            loadDataBarang();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            dashboardAdmin.Show();
            this.Hide();
        }

        private bool isInputValid()
        {
            if (string.IsNullOrEmpty(fieldBarang.Text))
            {
                MessageBox.Show("Pastikan Nama Barang Sudah Terisi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (string.IsNullOrEmpty(fieldStock.Text))
            {
                MessageBox.Show("Pastikan Stock Barang Sudah Terisi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (string.IsNullOrEmpty(fieldHarga.Text))
            {
                MessageBox.Show("Pastikan Harga Barang Sudah Terisi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (string.IsNullOrEmpty(fieldDataMasuk.Text))
            {
                MessageBox.Show("Pastikan Tanggal Barang Masuk Sudah Terisi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isInputValid())
            {
                if (long.TryParse(txtBarang.Text, out long barcodeIDValue))
                {
                    Barcode2 newBarang = new Barcode2
                    {
                        BarcodeID = barcodeIDValue,
                        Nama = fieldBarang.Text
                    };

                    Db.Barcode2s.InsertOnSubmit(newBarang);
                    Db.SubmitChanges();

                    MessageBox.Show("Data Barang Berhasil Dimasukan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadDataBarang(); // Refresh data setelah insert
                }
                else
                {
                    MessageBox.Show("Barcode ID tidak valid. Pastikan hanya angka yang dimasukkan dan dalam rentang yang benar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pastikan Semua Data Sudah Terisi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            loadDataBarang();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            fieldBarang.Text = "";
            fieldStock.Value = 0;
            fieldHarga.Value = 0;
            fieldDataMasuk.Value = DateTime.Now;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string namaBarang = selectedRow.Cells[1].Value != null ? selectedRow.Cells[1].Value.ToString() : "";
                int hargaBarang = selectedRow.Cells[2].Value != null ? Convert.ToInt32(selectedRow.Cells[2].Value) : 0;
                int stokBarang = selectedRow.Cells[3].Value != null ? Convert.ToInt32(selectedRow.Cells[3].Value) : 0;
                DateTime? dataMasuk = selectedRow.Cells[4].Value != null ? Convert.ToDateTime(selectedRow.Cells[4].Value) : (DateTime?)null;

                fieldBarang.Text = namaBarang;
                fieldHarga.Value = hargaBarang;
                fieldStock.Value = stokBarang;
                fieldDataMasuk.Value = dataMasuk ?? DateTime.Now;

                nameBarang = namaBarang;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (fieldBarang.Text != "")
            {
                var editListed = Db.Barangs.FirstOrDefault(i => i.NamaBarang == nameBarang);
                if (editListed != null)
                {
                    editListed.ID = editListed.ID;
                    editListed.NamaBarang = fieldBarang.Text;
                    editListed.StokBarang = Convert.ToInt32(fieldStock.Value);
                    editListed.HargaBarang = Convert.ToInt32(fieldHarga.Value);
                    editListed.DataMasuk = fieldDataMasuk.Value;

                    Db.SubmitChanges();
                    MessageBox.Show("Data Berhasil Dirubah", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin anda rubah lebih dahulu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 || !string.IsNullOrEmpty(fieldBarang.Text))
            {
                var result = MessageBox.Show("Yakin ingin mengubah stok barang ini menjadi 0?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var barang = Db.Barangs.FirstOrDefault(i => i.NamaBarang == fieldBarang.Text);
                    if (barang != null)
                    {
                        barang.StokBarang = 0; // Mengubah stok barang menjadi 0
                        Db.SubmitChanges();
                        MessageBox.Show("Stok barang berhasil diubah menjadi 0", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Barang tidak ditemukan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin anda ubah lebih dahulu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Output.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(cell.Value.ToString());
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Data Success di Print", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            
        }

        private void txtBarang_TextChanged(object sender, EventArgs e)
        {
            var textSearched = txtBarang.Text;
            if (!string.IsNullOrEmpty(txtBarang.Text))
            {
                var dataTransaksi = (from transaksi in Db.Barcode2s
                                     select new
                                     {
                                         Code_Transaksi = transaksi.BarcodeID,
                                         Nama_Barang = transaksi.Nama,
                                     }).ToList(); // Panggil ToList() untuk memuat data ke dalam memori

                // Terapkan filter dalam memori menggunakan LINQ to Objects
                var filteredDataTransaksi = dataTransaksi.Where(i => i.Nama_Barang.ToLower().Contains(textSearched) ||
                                                                     i.Code_Transaksi.ToString().Equals(textSearched)).ToList();

                if (dataTransaksi != null)
                {
                    dataGridView1.DataSource = filteredDataTransaksi.ToList();
                }
                else
                {
                    MessageBox.Show("Pastikan anda mencari berdasarkan nama barang", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Silakan isi field search terlebih dahulu", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
