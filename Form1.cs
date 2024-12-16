using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace De01
{
    public partial class frmSinhvien : Form
    {

        public frmSinhvien()
        {
            InitializeComponent();
        }

        private void frmSinhvien_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Lop> listLop = context.Lops.ToList();
                List<Sinhvien> listSinhvien = context.Sinhviens.ToList();
                FillFalcultyCombobox(listLop);
                BindGrid(listSinhvien);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void FillFalcultyCombobox(List<Lop> listLop)
        {
            this.cboLop.DataSource = listLop;
            this.cboLop.DisplayMember = "TenLop";
            this.cboLop.ValueMember = "MaLop";
        }

        private void BindGrid(List<Sinhvien> listStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaSV;
                dataGridView1.Rows[index].Cells[1].Value = item.HotenSV;
                dataGridView1.Rows[index].Cells[2].Value = item.Ngaysinh;
                dataGridView1.Rows[index].Cells[3].Value = item.Lop.MaLop;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Sinhvien> studentList = context.Sinhviens.ToList();
                if (studentList.Any(s => s.MaSV == txtMaSV.Text))
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng sinh viên mới
                var newStudent = new Sinhvien
                {
                    MaSV = txtMaSV.Text,
                    HotenSV = txtHoten.Text,

                    Ngaysinh = dtNgaysinh.Value,
                    MaLop = cboLop.SelectedValue.ToString(),
                };

                // Thêm sinh viên vào CSDL
                context.Sinhviens.Add(newStudent);
                context.SaveChanges();

                // Hiển thị lại danh sách sinh viên sau khi thêm
                BindGrid(context.Sinhviens.ToList());

                // Thông báo thành công
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi khi thêm dữ liệu
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Sinhvien> students = context.Sinhviens.ToList();
                var sinhvien = students.FirstOrDefault(s => s.MaSV == txtMaSV.Text);

                if (sinhvien != null)
                {

                    if (students.Any(s => s.MaSV == txtMaSV.Text && s.MaSV != sinhvien.MaSV))
                    {
                        MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    sinhvien.HotenSV = txtHoten.Text;

                    sinhvien.Ngaysinh = dtNgaysinh.Value;
                    sinhvien.MaLop = cboLop.SelectedValue.ToString();

                    context.SaveChanges();

                    BindGrid(context.Sinhviens.ToList());

                    MessageBox.Show("Chỉnh sửa thông tin sinh viên thành công!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {

                    MessageBox.Show("Sinh viên không tìm thấy!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Sinhvien> students = context.Sinhviens.ToList();
                var student = students.FirstOrDefault(s => s.MaSV == txtMaSV.Text);
                if (student != null)
                {
                    context.Sinhviens.Remove(student);
                    context.SaveChanges();
                    BindGrid(context.Sinhviens.ToList());
                    MessageBox.Show("Sinh vien da duoc xoa thanh cong!", "Thong bao!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh vien khong tim thay!", "Thong bao!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi cap nhat du lieu: {ex.Message}", "Thong bao!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectRow = dataGridView1.Rows[e.RowIndex];
                    txtMaSV.Text = selectRow.Cells[0].Value.ToString();
                    txtHoten.Text = selectRow.Cells[1].Value.ToString();
                    dtNgaysinh.Text = selectRow.Cells[2].Value.ToString();
                    cboLop.Text = selectRow.Cells[3].Value.ToString();
                }
            }

        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy MSSV từ TextBox
                string searchMSSV = txtTim.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchMSSV))
                {
                    MessageBox.Show("Vui lòng nhập MSSV cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng context để làm việc với database
                Model1 context = new Model1();

                // Tìm sinh viên theo MSSV
                var sinhvien = context.Sinhviens.FirstOrDefault(s => s.MaSV == searchMSSV);

                if (sinhvien != null)
                {
                    // Hiển thị thông tin sinh viên trong các TextBox
                    txtMaSV.Text = sinhvien.MaSV;
                    txtHoten.Text = sinhvien.HotenSV;
                    dtNgaysinh.Value = (DateTime)sinhvien.Ngaysinh;
                    cboLop.SelectedValue = sinhvien.MaLop;

                    // Hiển thị sinh viên trong DataGridView
                    BindGrid(new List<Sinhvien> { sinhvien });

                    MessageBox.Show("Tìm thấy sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên với MSSV này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm sinh viên: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn chắc chắn muốn thoát không?",
                                        "Thông báo!",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
    }


