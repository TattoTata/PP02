using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Word = DocumentFormat.OpenXml.Wordprocessing; // Алиас для Wordprocessing элементов

namespace ПП._2
{
    public class DocumentGenerator
    {
        private readonly string _connectionString;

        public DocumentGenerator(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Генерирует договор по ID работы (ObjectWorkID)
        /// </summary>
        /// <param name="objectWorkId">ID записи в ObjectWorks</param>
        /// <param name="filePath">Путь для сохранения файла</param>
        public bool GenerateContract(int objectWorkId, string filePath)
        {
            try
            {
                // 1. Получаем данные из БД
                var contractData = GetContractData(objectWorkId);
                if (contractData == null)
                {
                    MessageBox.Show("Не удалось загрузить данные для договора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 2. Создаем документ Word
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Word.Document(); // Используем алиас Word
                    Word.Body body = mainPart.Document.AppendChild(new Word.Body()); // Используем алиас Word

                    // Заголовок
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("ДОГОВОР ПОДРЯДА"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"№ {contractData.ContractNumber} от {DateTime.Now:dd.MM.yyyy}"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("")))); // Пустая строка

                    // Место и дата
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("г. Санкт-Петербург"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text(""))));

                    // Предмет договора
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("1. ПРЕДМЕТ ДОГОВОРА"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"1.1. Подрядчик обязуется выполнить работы по адресу: {contractData.Address}."))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"1.2. Вид работ: {contractData.WorkTypeName}."))));

                    // Стороны
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("2. СТОРОНЫ"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("2.1. Заказчик: ПАО \"Ростелеком\"."))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"2.2. Подрядчик: {contractData.ContractorName}, ИНН {contractData.INN}."))));

                    if (!string.IsNullOrEmpty(contractData.ContactPerson))
                    {
                        body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"Контактное лицо: {contractData.ContactPerson}, тел. {contractData.Phone}."))));
                    }

                    // Стоимость и сроки
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("3. СТОИМОСТЬ И СРОКИ"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"3.1. Стоимость работ: {contractData.Cost:N2} руб."))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"3.2. Дата начала: {contractData.StartDate:dd.MM.yyyy}"))));

                    string endDateStr = contractData.EndDate != DateTime.MinValue
                        ? contractData.EndDate.ToString("dd.MM.yyyy")
                        : "не указана";
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text($"3.3. Дата окончания: {endDateStr}"))));

                    // Подписи
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text(""))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("__________________ / Заказчик"))));
                    body.AppendChild(new Word.Paragraph(new Word.Run(new Word.Text("__________________ / Подрядчик"))));

                    mainPart.Document.Save();
                }

                MessageBox.Show($"Договор успешно создан!\nСохранен в: {filePath}", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации договора: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Вспомогательный класс для данных
        private class ContractData
        {
            public string ContractNumber { get; set; }
            public string Address { get; set; }
            public string WorkTypeName { get; set; }
            public string ContractorName { get; set; }
            public string INN { get; set; }
            public string ContactPerson { get; set; }
            public string Phone { get; set; }
            public decimal Cost { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        private ContractData GetContractData(int objectWorkId)
        {
            ContractData data = new ContractData();

            string query = @"
                SELECT 
                    ow.ObjectWorkID,
                    ow.StartDate,
                    ow.EndDate,
                    ow.Cost,
                    cs.Address,
                    cs.Name AS ObjectName,
                    wt.Name AS WorkTypeName,
                    c.CompanyName,
                    c.INN,
                    c.ContactPerson,
                    c.Phone
                FROM ObjectWorks ow
                JOIN ConstructionSite cs ON ow.ObjectID = cs.ObjectID
                JOIN WorkTypes wt ON ow.WorkTypeID = wt.WorkTypeID
                JOIN Contractors c ON ow.ContractorID = c.ContractorID
                WHERE ow.ObjectWorkID = @ObjectWorkID";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ObjectWorkID", objectWorkId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data.ContractNumber = $"Д-{reader["ObjectWorkID"]}";
                            data.Address = reader["Address"]?.ToString() ?? "Не указан";
                            data.WorkTypeName = reader["WorkTypeName"]?.ToString() ?? "Не указан";
                            data.ContractorName = reader["CompanyName"]?.ToString() ?? "Не указан";
                            data.INN = reader["INN"]?.ToString() ?? "Не указан";
                            data.ContactPerson = reader["ContactPerson"]?.ToString() ?? "";
                            data.Phone = reader["Phone"]?.ToString() ?? "";
                            data.Cost = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0;
                            data.StartDate = reader["StartDate"] != DBNull.Value ? Convert.ToDateTime(reader["StartDate"]) : DateTime.Now;
                            data.EndDate = reader["EndDate"] != DBNull.Value ? Convert.ToDateTime(reader["EndDate"]) : DateTime.MinValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return data;
        }
    }
}