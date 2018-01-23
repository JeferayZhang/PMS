using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Common;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace Common
{
    public class ExcelHelper
    {
        public XSSFWorkbook workbook07;
        public HSSFWorkbook workbook03;
        private int version = 3;   //3表示是03版本的，7表示是07版本的
        public ISheet sheet;

        public ExcelHelper(string excelPath = "")
        {
            try
            {
                FileInfo fi = new FileInfo(excelPath);
                if (fi.Extension.ToUpper() == ".XLSX")
                    version = 7;
            }
            catch { }

            if (excelPath == "")
            {
                if (version == 3)
                    workbook03 = new HSSFWorkbook();
                else
                    workbook07 = new XSSFWorkbook();
                return;
            }
            try
            {
                FileStream fileStream = new FileStream(excelPath, FileMode.Open);
                if (version == 3)
                    workbook03 = new HSSFWorkbook(fileStream);
                else
                    workbook07 = new XSSFWorkbook(fileStream);
                fileStream.Close();
            }
            catch
            {
                if (version == 3)
                    workbook03 = new HSSFWorkbook();
                else
                    workbook07 = new XSSFWorkbook();
            }
        }


        public ISheet getsheetbyname(string name)
        {
            if (version == 3)
                return workbook03.GetSheet(name);
            else
                return workbook07.GetSheet(name);
        }

        public ISheet getsheetbyindex(int index)
        {
            if (version == 3)
                return workbook03.GetSheetAt(index);
            else
                return workbook07.GetSheetAt(index);
        }

        public void OpenOrCreateNew(string sheetName = "Sheet1")
        {
            sheet = workbook03.GetSheet(sheetName);
            if (sheet == null)
                sheet = workbook03.CreateSheet(sheetName);
        }

        public DataTable ReadDataTable()
        {
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
                return new DataTable();
            int cellfirst = headerRow.FirstCellNum;
            int celllast = headerRow.LastCellNum;
            List<int> array = new List<int>();
            for (int i = cellfirst; i < celllast; i++)
            {
                string str = headerRow.GetCell(i)._ToStrTrim();
                if (str != "")
                {
                    DataColumn column = new DataColumn(str);
                    table.Columns.Add(column);
                    array.Add(i);
                }
            }
            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                    continue;
                DataRow dataRow = table.NewRow();
                for (int j = 0; j < array.Count; j++)
                {
                    if (row.GetCell(array[j]) != null)
                    {
                        if (row.GetCell(array[j]).CellType == CellType.Numeric)
                        {
                            if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(array[j])))
                                dataRow[j] = row.GetCell(array[j]).DateCellValue;
                            else
                                dataRow[j] = row.GetCell(array[j]).NumericCellValue;


                        }
                        else
                            dataRow[j] = row.GetCell(array[j]).ToString();
                    }
                }
                table.Rows.Add(dataRow);
            }

            return table;
        }

        public void InsertDataTable(DataTable dt, int rowIndex = 0, int cellIndex = 0)
        {
            int currentRowIndex = rowIndex;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                InsertText(currentRowIndex, cellIndex + i, dt.Columns[i].ColumnName);
            }

            int sheetIndex = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                currentRowIndex++;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    InsertText(currentRowIndex, cellIndex + j, dt.Rows[i][j].ToString());
                }

                if (currentRowIndex == 65535 - rowIndex)
                {
                    sheetIndex++;
                    currentRowIndex = rowIndex;
                    sheet = workbook03.CreateSheet(workbook03.GetSheetAt(0).SheetName + "_" + sheetIndex);

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        InsertText(currentRowIndex, cellIndex + k, dt.Columns[k].ColumnName);
                    }
                }
            }
        }

        public void InsertDataTable123(DataTable dt, bool flag, int firstrow = 0, int lastrow = 0)
        {
            int currentRowIndex = 0;
            int cellIndex = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                InsertText(0, i, dt.Columns[i].ColumnName);
            }

            for (int i = firstrow; i < lastrow; i++)
            {
                currentRowIndex++;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    InsertText(currentRowIndex, cellIndex + j, dt.Rows[i][j].ToString());
                }
            }
        }

        public void InsertDataTable(DataSet ds, int rowIndex = 0, int cellIndex = 0)
        {
            int currentRowIndex = rowIndex;
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                InsertText(currentRowIndex, cellIndex + i, ds.Tables[0].Columns[i].ColumnName);
            }
            int sheetIndex = 0;

            for (int m = 0; m < ds.Tables.Count; m++)
            {
                for (int i = 0; i < ds.Tables[m].Rows.Count; i++)
                {
                    currentRowIndex++;
                    for (int j = 0; j < ds.Tables[m].Columns.Count; j++)
                    {
                        InsertText(currentRowIndex, cellIndex + j, ds.Tables[m].Rows[i][j].ToString());
                    }

                    if (currentRowIndex == 65535 - rowIndex)
                    {
                        sheetIndex++;
                        currentRowIndex = rowIndex;
                        sheet = workbook03.CreateSheet(workbook03.GetSheetAt(0).SheetName + "_" + sheetIndex);

                        for (int k = 0; k < ds.Tables[m].Columns.Count; k++)
                        {
                            InsertText(currentRowIndex, cellIndex + k, ds.Tables[m].Columns[k].ColumnName);
                        }
                    }
                }
            }
        }

        public void InsertText(int rowIndex, int cellIndex, string textValue, ICellStyle cellStyle = null)
        {
            IRow row = sheet.GetRow(rowIndex);
            if (row == null)
                row = sheet.CreateRow(rowIndex);
            ICell cell = row.GetCell(cellIndex);
            if (cell == null)
                cell = row.CreateCell(cellIndex);
            cell.SetCellValue(textValue);
            if (cellStyle != null)
                cell.CellStyle = cellStyle;
        }

        public void SaveFile(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook03.Write(file);
            file.Close();
        }
    }
}
