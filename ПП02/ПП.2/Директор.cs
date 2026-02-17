using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace ПП._2
{
    public partial class Директор : Form
    {
        private string connectionString = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=ПП.2;Integrated Security=True";
        private string userRole;
        public Директор(string username, string role, string email)
        {
            InitializeComponent();
            // Настройка формы на основе данных пользователя
            Text = $"Панель Директора: {username}";
            userRole = role;

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(tabPage1.Name)) tabPage1.Name = "tabPage1"; // Объекты
            if (string.IsNullOrEmpty(tabPage2.Name)) tabPage2.Name = "tabPage2"; // Подрядчики
            if (string.IsNullOrEmpty(tabPage3.Name)) tabPage3.Name = "tabPage3"; // Вид документации
            if (string.IsNullOrEmpty(tabPage4.Name)) tabPage4.Name = "tabPage4"; // Руководитель объекта
            if (string.IsNullOrEmpty(tabPage5.Name)) tabPage5.Name = "tabPage5"; // Вид работ
            if (string.IsNullOrEmpty(tabPage6.Name)) tabPage6.Name = "tabPage6"; // Документы по объекту
            if (string.IsNullOrEmpty(tabPage7.Name)) tabPage7.Name = "tabPage7"; // Работы по объекту
            if (string.IsNullOrEmpty(tabPage8.Name)) tabPage8.Name = "tabPage8"; // пользователи 
            RefreshActiveTabData();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            comboBox1.TextChanged += comboBox_TextChanged_Universal;
            comboBox2.TextChanged += comboBox_TextChanged_Universal;
            comboBox3.TextChanged += comboBox_TextChanged_Universal;
            comboBox4.TextChanged += comboBox_TextChanged_Universal;
            comboBox5.TextChanged += comboBox_TextChanged_Universal;
            comboBox6.TextChanged += comboBox_TextChanged_Universal;
            comboBox7.TextChanged += comboBox_TextChanged_Universal;
            comboBox8.TextChanged += comboBox_TextChanged_Universal;
            
        }
        public System.Data.DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        con.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            return dataTable;
        }

        private void ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения команды: {ex.Message}");
            }
        }

        private void RefreshActiveTabData(bool forceReloadComboBox = false)
        {
            string selectedTabName = tabControl1.SelectedTab?.Name;
            if (string.IsNullOrEmpty(selectedTabName) && tabControl1.TabPages.Count > 0)
            {
                selectedTabName = tabControl1.TabPages[0].Name;
            }

            switch (selectedTabName)
            {
                case "tabPage1":
                    LoadConstructionSites(comboBox1.Text == " (Все) " ? null : comboBox1.Text);
                    if (forceReloadComboBox || comboBox1.DataSource == null) PopulateConstructionSiteSearchComboBox();
                    break;
                case "tabPage2":
                    LoadContractors(comboBox4.Text == " (Все) " ? null : comboBox4.Text);
                    if (forceReloadComboBox || comboBox4.DataSource == null) PopulateContractorSearchComboBox();
                    break;
                case "tabPage3":
                    LoadDocumentationTypes(comboBox5.Text == " (Все) " ? null : comboBox5.Text);
                    if (forceReloadComboBox || comboBox5.DataSource == null) PopulateDocTypeSearchComboBox();
                    break;
                case "tabPage4":
                    LoadSiteManagers(comboBox6.Text == " (Все) " ? null : comboBox6.Text);
                    if (forceReloadComboBox || comboBox6.DataSource == null) PopulateSiteManagerSearchComboBox();
                    break;
                case "tabPage5":
                    LoadWorkTypes(comboBox7.Text == " (Все) " ? null : comboBox7.Text);
                    if (forceReloadComboBox || comboBox7.DataSource == null) PopulateWorkTypeSearchComboBox();
                    break;
                case "tabPage6":
                    LoadObjectDocuments(comboBox2.Text == " (Все) " ? null : comboBox2.Text);
                    if (forceReloadComboBox || comboBox2.DataSource == null) PopulateObjectDocumentSearchComboBox();
                    break;
                case "tabPage7":
                    LoadObjectWorks(comboBox3.Text == " (Все) " ? null : comboBox3.Text);
                    if (forceReloadComboBox || comboBox3.DataSource == null) PopulateObjectWorkSearchComboBox();
                    break;
                case "tabPage8": 
                    LoadUsers(comboBox8.Text == " (Все) " ? null : comboBox8.Text);
                    if (forceReloadComboBox || comboBox8.DataSource == null) PopulateUserSearchComboBox();
                    break;


            }
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshActiveTabData(true);
        }
        #region Объекты (Construction Sites - tabPage1, dataGridView1, comboBox1 по ПОЛЮ Address)
        private void LoadConstructionSites(string searchAddress = null)
        {
            string query = "SELECT ObjectID, Name, Address, StartDate, EndDate, Budget, Status FROM ConstructionSiteN";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchAddress) && searchAddress != " (Все) ")
            {
                query += " WHERE Address LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchAddress}%") };
            }
            query += " ORDER BY Address, Name;";
            dataGridView1.DataSource = GetData(query, parameters);
            if (dataGridView1.Columns["ObjectID"] != null) dataGridView1.Columns["ObjectID"].Visible = false;
        }
        private void PopulateConstructionSiteSearchComboBox()
        {
            PopulateComboBox(comboBox1, "SELECT DISTINCT Address FROM ConstructionSite WHERE Address IS NOT NULL AND Address <> '' ORDER BY Address", "Address");
        }
        #endregion

        #region Подрядчики (Contractors - tabPage2, dataGridView4, comboBox4 по ПОЛЮ CompanyName) - БЕЗ ИЗМЕНЕНИЙ
        private void LoadContractors(string searchTerm = null)
        {
            string query = "SELECT ContractorID, CompanyName, INN, ContactPerson, Phone, Email FROM Contractors";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchTerm) && searchTerm != " (Все) ")
            {
                query += " WHERE CompanyName LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchTerm}%") };
            }
            query += " ORDER BY CompanyName;";
            dataGridView4.DataSource = GetData(query, parameters);
            if (dataGridView4.Columns["ContractorID"] != null) dataGridView4.Columns["ContractorID"].Visible = false;
        }
        private void PopulateContractorSearchComboBox()
        {
            PopulateComboBox(comboBox4, "SELECT DISTINCT CompanyName FROM Contractors ORDER BY CompanyName", "CompanyName");
        }
        #endregion

        #region Вид документации (Documentation Types - tabPage3, dataGridView5, comboBox5 по ПОЛЮ TypeName) - БЕЗ ИЗМЕНЕНИЙ
        private void LoadDocumentationTypes(string searchTerm = null)
        {
            string query = "SELECT DocTypeID, TypeName FROM DocumentationTypes";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchTerm) && searchTerm != " (Все) ")
            {
                query += " WHERE TypeName LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchTerm}%") };
            }
            query += " ORDER BY TypeName;";
            dataGridView5.DataSource = GetData(query, parameters);
            if (dataGridView5.Columns["DocTypeID"] != null) dataGridView5.Columns["DocTypeID"].Visible = false;
        }
        private void PopulateDocTypeSearchComboBox()
        {
            PopulateComboBox(comboBox5, "SELECT DISTINCT TypeName FROM DocumentationTypes ORDER BY TypeName", "TypeName");
        }
        #endregion

        #region Руководитель объекта (Site Managers - tabPage4, dataGridView6, comboBox6 по ПОЛЮ FullName) - БЕЗ ИЗМЕНЕНИЙ
        private void LoadSiteManagers(string searchTerm = null)
        {
            string query = @"
                SELECT sm.ManagerID, sm.FullName, sm.Phone, sm.Email, cs.Name AS ObjectName
                FROM SiteManagers sm
                LEFT JOIN ConstructionSite cs ON sm.ObjectID = cs.ObjectID";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchTerm) && searchTerm != " (Все) ")
            {
                query += " WHERE sm.FullName LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchTerm}%") };
            }
            query += " ORDER BY sm.FullName;";
            dataGridView6.DataSource = GetData(query, parameters);
            if (dataGridView6.Columns["ManagerID"] != null) dataGridView6.Columns["ManagerID"].Visible = false;
        }
        private void PopulateSiteManagerSearchComboBox()
        {
            PopulateComboBox(comboBox6, "SELECT DISTINCT FullName FROM SiteManagers ORDER BY FullName", "FullName");
        }
        #endregion

        #region Вид работ (Work Types - tabPage5, dataGridView7, comboBox7 по ПОЛЮ Name) - БЕЗ ИЗМЕНЕНИЙ
        private void LoadWorkTypes(string searchTerm = null)
        {
            string query = "SELECT WorkTypeID, Name, Description FROM WorkTypes";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchTerm) && searchTerm != " (Все) ")
            {
                query += " WHERE Name LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchTerm}%") };
            }
            query += " ORDER BY Name;";
            dataGridView7.DataSource = GetData(query, parameters);
            if (dataGridView7.Columns["WorkTypeID"] != null) dataGridView7.Columns["WorkTypeID"].Visible = false;
        }
        private void PopulateWorkTypeSearchComboBox()
        {
            PopulateComboBox(comboBox7, "SELECT DISTINCT Name FROM WorkTypes ORDER BY Name", "Name");
        }
        #endregion

        #region Документы по объекту (Object Documents - tabPage6, dataGridView2, comboBox2 по ПОЛЮ ObjectName)
        private void LoadObjectDocuments(string searchObjectName = null)
        {
            string query = @"
                SELECT od.DocumentID, cs.Name AS ObjectName, dt.TypeName AS DocumentTypeName,
                        od.FilePath, od.UploadDate
                FROM ObjectDocuments od
                JOIN ConstructionSite cs ON od.ObjectID = cs.ObjectID
                JOIN DocumentationTypes dt ON od.DocTypeID = dt.DocTypeID";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchObjectName) && searchObjectName != " (Все) ")
            {
                query += " WHERE cs.Name LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchObjectName}%") };
            }
            query += " ORDER BY cs.Name, dt.TypeName;";
            dataGridView2.DataSource = GetData(query, parameters);
            if (dataGridView2.Columns["DocumentID"] != null) dataGridView2.Columns["DocumentID"].Visible = false;
        }
        private void PopulateObjectDocumentSearchComboBox()
        {
            PopulateComboBox(comboBox2, "SELECT DISTINCT Name FROM ConstructionSite ORDER BY Name", "Name");
        }
        #endregion

        #region Работы по объекту (Object Works - tabPage7, dataGridView3, comboBox3 по ПОЛЮ ObjectName)
        private void LoadObjectWorks(string searchObjectName = null)
        {
            string query = @"
                SELECT ow.ObjectWorkID, cs.Name AS ObjectName, wt.Name AS WorkTypeName,
                        c.CompanyName AS ContractorName, ow.StartDate, ow.EndDate, ow.Cost
                FROM ObjectWorks ow
                JOIN ConstructionSite cs ON ow.ObjectID = cs.ObjectID
                JOIN WorkTypes wt ON ow.WorkTypeID = wt.WorkTypeID
                JOIN Contractors c ON ow.ContractorID = c.ContractorID";
            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchObjectName) && searchObjectName != " (Все) ")
            {
                query += " WHERE cs.Name LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchObjectName}%") };
            }
            query += " ORDER BY cs.Name, wt.Name;";
            dataGridView3.DataSource = GetData(query, parameters);
            if (dataGridView3.Columns["ObjectWorkID"] != null) dataGridView3.Columns["ObjectWorkID"].Visible = false;
        }
        private void PopulateObjectWorkSearchComboBox()
        {
            PopulateComboBox(comboBox3, "SELECT DISTINCT Name FROM ConstructionSite ORDER BY Name", "Name");
        }
        #endregion

        #region Пользователи (Users - tabPage8, dataGridView8, comboBox8 по ПОЛЮ Username)
        private void LoadUsers(string searchUsername = null)
        {
            string query = "SELECT ID, Login, Password, Email, Role FROM Users";
            SqlParameter[] parameters = null;

            if (!string.IsNullOrEmpty(searchUsername) && searchUsername != " (Все) ")
            {
                query += " WHERE Login LIKE @SearchTerm";
                parameters = new SqlParameter[] { new SqlParameter("@SearchTerm", $"%{searchUsername}%") };
            }
            query += " ORDER BY Login;";

            dataGridView8.DataSource = GetData(query, parameters);
            if (dataGridView8.Columns["ID"] != null)
                dataGridView8.Columns["ID"].Visible = false;
        }
        private void PopulateUserSearchComboBox()
        {
            PopulateComboBox(comboBox8,
                "SELECT DISTINCT Login FROM Users ORDER BY Login",
                "Login");
        }
        #endregion
        private void comboBox_TextChanged_Universal(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null) return;
            RefreshActiveTabData(false);
        }
        private void PopulateComboBox(ComboBox cb, string query, string displayMember, string valueMember = null)
        {
            System.Data.DataTable dt = GetData(query);
            System.Data.DataRow dr = dt.NewRow();
            dr[displayMember] = " (Все) ";
            dt.Rows.InsertAt(dr, 0);

            string previousValue = cb.Text;

            cb.DataSource = null;
            cb.Items.Clear();

            cb.DisplayMember = displayMember;
            if (!string.IsNullOrEmpty(valueMember) && dt.Columns.Contains(valueMember))
            {
                cb.ValueMember = valueMember;
            }
            cb.DataSource = dt;
            int foundIndex = cb.FindStringExact(previousValue);
            if (foundIndex != -1)
            {
                cb.SelectedIndex = foundIndex;
            }
            else if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form addForm = null;
            string activeTabName = tabControl1.SelectedTab?.Name;
            switch (activeTabName)
            {
                case "tabPage1": addForm = new ConstructionSiteForm(connectionString); break;
                case "tabPage2": addForm = new ContractorForm(connectionString); break;
                case "tabPage3": addForm = new DocumentationTypeForm(connectionString); break;
                case "tabPage4": addForm = new SiteManagerForm(connectionString); break;
                case "tabPage5": addForm = new WorkTypeForm(connectionString); break;
                case "tabPage6": addForm = new ObjectDocumentForm(connectionString); break;
                case "tabPage7": addForm = new ObjectWorkForm(connectionString); break;
                case "tabPage8": addForm = new UserForm(connectionString); break;
                default:
                    MessageBox.Show("Выберите вкладку для добавления записи.");
                    return;
            }
            if (addForm != null)
            {
                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshActiveTabData(true);
                }
            }
            else if (activeTabName != null)
            {
                MessageBox.Show($"Функция добавления для вкладки '{activeTabName}' не реализована.");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataGridView currentDgv = null;
            string idColumnName = "";
            Form editForm = null;
            int selectedId = -1;
            string activeTabName = tabControl1.SelectedTab?.Name;
            switch (activeTabName)
            {
                case "tabPage1": currentDgv = dataGridView1; idColumnName = "ObjectID"; break;
                case "tabPage2": currentDgv = dataGridView4; idColumnName = "ContractorID"; break;
                case "tabPage3": currentDgv = dataGridView5; idColumnName = "DocTypeID"; break;
                case "tabPage4": currentDgv = dataGridView6; idColumnName = "ManagerID"; break;
                case "tabPage5": currentDgv = dataGridView7; idColumnName = "WorkTypeID"; break;
                case "tabPage6": currentDgv = dataGridView2; idColumnName = "DocumentID"; break;
                case "tabPage7": currentDgv = dataGridView3; idColumnName = "ObjectWorkID"; break;
                case "tabPage8": currentDgv = dataGridView8; idColumnName = "Id"; break;
                default:
                    MessageBox.Show("Выберите вкладку для редактирования записи.");
                    return;
            }
            if (currentDgv == null || currentDgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите запись для редактирования.");
                return;
            }
            if (currentDgv.SelectedRows[0].Cells[idColumnName].Value == DBNull.Value || currentDgv.SelectedRows[0].Cells[idColumnName].Value == null)
            {
                MessageBox.Show("Не удалось получить ID выбранной записи.");
                return;
            }

            selectedId = Convert.ToInt32(currentDgv.SelectedRows[0].Cells[idColumnName].Value);

            switch (activeTabName)
            {
                case "tabPage1": editForm = new ConstructionSiteForm(connectionString, selectedId, userRole); break;
                case "tabPage2": editForm = new ContractorForm(connectionString, selectedId); break;
                case "tabPage3": editForm = new DocumentationTypeForm(connectionString, selectedId); break;
                case "tabPage4": editForm = new SiteManagerForm(connectionString, selectedId); break;
                case "tabPage5": editForm = new WorkTypeForm(connectionString, selectedId); break;
                case "tabPage6": editForm = new ObjectDocumentForm(connectionString, selectedId); break;
                case "tabPage7": editForm = new ObjectWorkForm(connectionString, selectedId); break;
                case "tabPage8": editForm = new UserForm(connectionString, selectedId); break;
            }

            if (editForm != null)
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshActiveTabData(true);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            DataGridView currentDgv = null;
            string idColumnName = "";
            string tableName = "";
            string entityName = "";
            string activeTabName = tabControl1.SelectedTab?.Name;

            switch (activeTabName)
            {
                case "tabPage1":
                    currentDgv = dataGridView1;
                    idColumnName = "ObjectID";
                    tableName = "ConstructionSiteN";
                    entityName = "объект строительства";
                    break;
                case "tabPage2":
                    currentDgv = dataGridView4;
                    idColumnName = "ContractorID";
                    tableName = "Contractors";
                    entityName = "подрядчика";
                    break;
                case "tabPage3":
                    currentDgv = dataGridView5;
                    idColumnName = "DocTypeID";
                    tableName = "DocumentationTypes";
                    entityName = "тип документации";
                    break;
                case "tabPage4":
                    currentDgv = dataGridView6;
                    idColumnName = "ManagerID";
                    tableName = "SiteManagers";
                    entityName = "ответственного";
                    break;
                case "tabPage5":
                    currentDgv = dataGridView7;
                    idColumnName = "WorkTypeID";
                    tableName = "WorkTypes";
                    entityName = "вид работ";
                    break;
                case "tabPage6":
                    currentDgv = dataGridView2;
                    idColumnName = "DocumentID";
                    tableName = "ObjectDocuments";
                    entityName = "документ объекта";
                    break;
                case "tabPage7":
                    currentDgv = dataGridView3;
                    idColumnName = "ObjectWorkID";
                    tableName = "ObjectWorks";
                    entityName = "работу по объекту";
                    break;
                case "tabPage8":
                    currentDgv = dataGridView8;
                    idColumnName = "Id";
                    tableName = "Users";
                    entityName = "пользователя";
                    break;

                default:
                    MessageBox.Show("Пожалуйста, выберите вкладку для удаления записи.");
                    return;
            }

            if (currentDgv == null || currentDgv.SelectedRows.Count == 0)
            {
                MessageBox.Show($"Пожалуйста, выберите {entityName} для удаления.");
                return;
            }

            if (currentDgv.SelectedRows[0].Cells[idColumnName].Value == DBNull.Value || currentDgv.SelectedRows[0].Cells[idColumnName].Value == null)
            {
                MessageBox.Show($"Не удалось получить ID выбранного {entityName} для удаления. Возможно, столбец '{idColumnName}' отсутствует или пуст.");
                return;
            }

            int selectedId = Convert.ToInt32(currentDgv.SelectedRows[0].Cells[idColumnName].Value);

            if (MessageBox.Show($"Вы уверены, что хотите удалить выбранн{(entityName.EndsWith("а") ? "ую" : "ый")} {entityName} (ID: {selectedId})?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {

                    if (tableName == "ConstructionSiteN")
                    {
                        ExecuteNonQuery("DELETE FROM SiteManagers WHERE ObjectID = @ID", new SqlParameter[] { new SqlParameter("@ID", selectedId) });
                        ExecuteNonQuery("DELETE FROM ObjectDocuments WHERE ObjectID = @ID", new SqlParameter[] { new SqlParameter("@ID", selectedId) });
                        ExecuteNonQuery("DELETE FROM ObjectWorks WHERE ObjectID = @ID", new SqlParameter[] { new SqlParameter("@ID", selectedId) });
                    }
                    else if (tableName == "DocumentationTypes")
                    {
                        if (Convert.ToInt32(GetData($"SELECT COUNT(*) FROM ObjectDocuments WHERE DocTypeID = @ID", new SqlParameter[] { new SqlParameter("@ID", selectedId) }).Rows[0][0]) > 0)
                        {
                            MessageBox.Show($"Невозможно удалить тип документации, так как он используется в документах объектов. Сначала удалите или измените связанные документы.");
                            return;
                        }
                    }
                    else if (tableName == "WorkTypes")
                    {
                        if (Convert.ToInt32(GetData($"SELECT COUNT(*) FROM ObjectWorks WHERE WorkTypeID = @ID", new SqlParameter[] { new SqlParameter("@ID", selectedId) }).Rows[0][0]) > 0)
                        {
                            MessageBox.Show($"Невозможно удалить вид работ, так как он используется в работах по объектам. Сначала удалите или измените связанные работы.");
                            return;
                        }
                    }
                    else if (tableName == "Contractors")
                    {
                        if (Convert.ToInt32(GetData($"SELECT COUNT(*) FROM ObjectWorks WHERE ContractorID = @ID", new SqlParameter[] { new SqlParameter("@ID", selectedId) }).Rows[0][0]) > 0)
                        {
                            MessageBox.Show($"Невозможно удалить подрядчика, так как он связан с работами по объектам. Сначала удалите или измените связанные работы.");
                            return;
                        }
                    }

                    string deleteQuery = $"DELETE FROM {tableName} WHERE {idColumnName} = @ID";
                    ExecuteNonQuery(deleteQuery, new SqlParameter[] { new SqlParameter("@ID", selectedId) });

                    RefreshActiveTabData(true);
                    MessageBox.Show($"{char.ToUpper(entityName[0]) + entityName.Substring(1)} успешно удален(а).");
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 547)
                    {
                        MessageBox.Show($"Невозможно удалить {entityName}. Запись используется в других таблицах. Сначала удалите или измените связанные записи.\n(Код ошибки SQL: {sqlEx.Number})");
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка SQL при удалении {entityName}: {sqlEx.Message}\n(Код ошибки SQL: {sqlEx.Number})");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при удалении {entityName}: {ex.Message}");
                }
            }
        }
        private void BtnExportContract_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбрана запись в dataGridView3 (Работы по объекту)
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работу для создания договора.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем ID выбранной работы (ObjectWorkID)
            var selectedRow = dataGridView3.SelectedRows[0];

            // Проверяем наличие колонки ObjectWorkID
            if (!dataGridView3.Columns.Contains("ObjectWorkID") ||
                selectedRow.Cells["ObjectWorkID"].Value == null)
            {
                MessageBox.Show("Не удалось получить ID работы. Возможно, колонка ObjectWorkID скрыта.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int objectWorkId = Convert.ToInt32(selectedRow.Cells["ObjectWorkID"].Value);

            // Диалог сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents (*.docx)|*.docx";
            saveFileDialog.FileName = $"Договор_{objectWorkId}_{DateTime.Now:yyyyMMdd}.docx";
            saveFileDialog.Title = "Сохранить договор";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DocumentGenerator generator = new DocumentGenerator(connectionString);
                generator.GenerateContract(objectWorkId, saveFileDialog.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridView currentDgv = null;
            string activeTabName = tabControl1.SelectedTab?.Name;

            switch (activeTabName)
            {
                case "tabPage1": currentDgv = dataGridView1; break;
                case "tabPage2": currentDgv = dataGridView4; break;
                case "tabPage3": currentDgv = dataGridView5; break;
                case "tabPage4": currentDgv = dataGridView6; break;
                case "tabPage5": currentDgv = dataGridView7; break;
                case "tabPage6": currentDgv = dataGridView2; break;
                case "tabPage7": currentDgv = dataGridView3; break;
                case "tabPage8": currentDgv = dataGridView8; break;
                default:
                    MessageBox.Show("Выберите вкладку с данными для выгрузки.");
                    return;
            }

            if (currentDgv == null || (currentDgv.DataSource is System.Data.DataTable dt && dt.Rows.Count == 0) || currentDgv.DataSource == null)
            {
                MessageBox.Show("На выбранной вкладке нет данных для выгрузки.");
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            saveFileDialog.FileName = (tabControl1.SelectedTab?.Text ?? "Данные") + "_Выгрузка.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToExcel(currentDgv, saveFileDialog.FileName);
            }
        }
        private void ExportToExcel(DataGridView dgv, string filePath)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbook workbook = null;
            Worksheet worksheet = null;

            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Add();
                worksheet = (Worksheet)workbook.Sheets[1];

                System.Data.DataTable dataTable = dgv.DataSource as System.Data.DataTable;

                if (dataTable == null)
                {
                    MessageBox.Show("Не удалось получить данные из DataGridView.");
                    return;
                }
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value2 = dataTable.Columns[i].ColumnName;
                }
                object[,] data = new object[dataTable.Rows.Count, dataTable.Columns.Count];
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        data[row, col] = dataTable.Rows[row][col] == DBNull.Value ? "" : dataTable.Rows[row][col];
                    }
                }
                Range dataRange = (Range)worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[dataTable.Rows.Count + 1, dataTable.Columns.Count]];
                dataRange.Value2 = data;

                worksheet.Columns.AutoFit();

                workbook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, Type.Missing,
                    Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                MessageBox.Show($"Данные успешно выгружены в файл:\n{filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выгрузке в Excel: {ex.Message}");
            }
            finally
            {
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                if (excelApp != null)
                {
                    excelApp.Quit();
                    ReleaseObject(excelApp);
                }
            }
        }
        private void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null && Marshal.IsComObject(obj))
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                System.Diagnostics.Debug.WriteLine("Ошибка при освобождении COM-объекта: " + ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?","Подтверждение выхода", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide(); 
                LoginForm loginForm = new LoginForm();

                loginForm.Show();
            }
        }
    }
}