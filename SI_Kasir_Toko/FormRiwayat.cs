using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace SI_Kasir_Toko
{
    using static GlobalVariable;
    using static iTextSharp.text.pdf.security.SignaturePermissions;

    public partial class FormRiwayat : Form
    {
        public FormRiwayat()
        {
            InitializeComponent();
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            loadDataBarang();
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            dashboardKasir.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
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
                            PdfPTable pdfTable = new PdfPTable(dataGridView2.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dataGridView2.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView2.Rows)
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

                                MessageBox.Show("Data Success di Print", "Info");
                            }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string namaBarang = selectedRow.Cells[1].Value != null ? selectedRow.Cells[1].Value.ToString() : "";
                int hargaBarang = selectedRow.Cells[2].Value != null ? Convert.ToInt32(selectedRow.Cells[2].Value) : 0;
                int stokBarang = selectedRow.Cells[3].Value != null ? Convert.ToInt32(selectedRow.Cells[3].Value) : 0;
                DateTime? dataMasuk = selectedRow.Cells[4].Value != null ? Convert.ToDateTime(selectedRow.Cells[4].Value) : (DateTime?)null;

                int newRowIdx = dataGridView2.Rows.Add();

                // Mengambil baris yang baru saja ditambahkan
                DataGridViewRow newRow = dataGridView2.Rows[newRowIdx];

                // Menetapkan nilai ke kolom-kolom di dataGridView2
                newRow.Cells["Code"].Value = newRowIdx + 2; // Misalnya, nomor urut
                newRow.Cells["Nama"].Value = namaBarang;
                newRow.Cells["Jumlah"].Value = 1; // Misalnya, jumlah default
                newRow.Cells["Harga"].Value = hargaBarang;
                newRow.Cells["Total"].Value = hargaBarang; // Misalnya, total default

                // Menghitung total berdasarkan jumlah dan harga
                int jumlah = Convert.ToInt32(newRow.Cells["Jumlah"].Value);
                int harga = Convert.ToInt32(newRow.Cells["Harga"].Value);
                int total = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["Total"].Value);
                newRow.Cells["Total"].Value = jumlah * harga;

                fieldTotalBelanja.Value = total;
                fieldKembalian.Value = total - fieldNominal.Value;
            }
        }

        private void fieldNominal_ValueChanged(object sender, EventArgs e)
        {
            var kembali = fieldNominal.Value - fieldKembalian.Value;
            fieldKembalian.Value = kembali;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var textSearched = fieldSearched.Text.ToLower();
            if (!string.IsNullOrEmpty(fieldSearched.Text))
            {
                var dataBarang = (from barang in Db.Barangs
                                 select new
                                 {
                                     Kode = barang.ID,
                                     Nama = barang.NamaBarang,
                                     Harga = barang.HargaBarang,
                                     Stock = barang.StokBarang,
                                     Masuk = barang.DataMasuk
                                 }).Where(i => i.Nama.ToLower().Contains(textSearched));
                //dataGridView1.DataSource = dataBarang.ToList();

                if (dataBarang != null)
                {
                    dataGridView1.DataSource = dataBarang.ToList();
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the last transaction by ID
                var lastTransaction = Db.Transactions.OrderBy(i => i.ID).FirstOrDefault();

                // Initialize the new transaction ID
                int newTransactionId = lastTransaction != null ? lastTransaction.ID + 1 : 1;

                // Ensure there are rows in the data grid
                if (dataGridView2.Rows.Count == 0)
                {
                    MessageBox.Show("No items to transact.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Ensure the total belanja field is not null and is an integer
                if (!int.TryParse(fieldTotalBelanja.Value.ToString(), out int totalBelanja))
                {
                    MessageBox.Show("Invalid total belanja value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Ensure the first cell value is not null and is an integer
                if (!int.TryParse(dataGridView2.Rows[0].Cells[0].Value?.ToString(), out int idBarang))
                {
                    MessageBox.Show("Invalid barang ID value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Debugging information
                Console.WriteLine($"Trying to insert transaction with IDBarang: {idBarang}");

                // Check if the IDBarang exists in the Barang table
                var barangExists = Db.Barangs.Any(b => b.ID == idBarang);
                if (!barangExists)
                {
                    MessageBox.Show($"Barang ID {idBarang} does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create the new transaction
                Transaction newTransaksi = new Transaction
                {
                    ID = newTransactionId,
                    TotalHarga = totalBelanja,
                    IDBarang = idBarang,
                    JumlahTransaksi = dataGridView2.Rows.Count,
                    TransaksiAt = DateTime.Now
                };

                // Insert and submit the new transaction
                Db.Transactions.InsertOnSubmit(newTransaksi);
                Db.SubmitChanges();

                // Show success message
                MessageBox.Show("Berhasil menambahkan data transaksi", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Log the exception (this could be written to a file or other logging mechanism)
                Console.WriteLine($"Exception: {ex.Message}");
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
    