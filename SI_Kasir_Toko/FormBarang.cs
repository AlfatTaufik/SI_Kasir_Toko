using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
            var dataBarang = from barang in Db.Barangs
                             select new
                             {
                                 Kode = barang.ID,
                                 Nama = barang.NamaBarang,
                                 Harga = barang.HargaBarang,
                                 Stock = barang.StokBarang,
                                 Masuk = barang.DataMasuk
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
            var latestId = Db.Barangs.OrderByDescending(i => i.ID).FirstOrDefault();
            var IdNow = latestId.ID;
            var newId = IdNow++;

            if (isInputValid())
            {
                Barang newBarang = new Barang
                {
                    ID = newId,
                    NamaBarang = fieldBarang.Text,
                    HargaBarang = Convert.ToInt32(fieldHarga.Value),
                    StokBarang = Convert.ToInt32(fieldStock.Value),
                    DataMasuk = fieldDataMasuk.Value
                };

                Db.Barangs.InsertOnSubmit(newBarang);
                Db.SubmitChanges();
                MessageBox.Show("Data Barang Berhasil Dimasukan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (dataGridView1.SelectedRows.Count > 0 || fieldBarang.Text != null)
            {
                var result = MessageBox.Show("Yakin ingin menghapus data ini?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var deleteListed = Db.Barangs.FirstOrDefault(i => i.NamaBarang == fieldBarang.Text);
                    Db.Barangs.DeleteOnSubmit(deleteListed);
                    Db.SubmitChanges();
                    MessageBox.Show("Data Berhasil Dihapus", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin anda hapus lebih dahulu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
    }
}
