using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using NPOI;
using NPOI.XSSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.Util;
using NPOI.XSSF.UserModel;
using System.IO;

namespace CatcherPlus
{
    class ExcelHelper
    {
        
        private string FileName;

        private XSSFWorkbook wbook = new XSSFWorkbook();
        private ISheet sheet1;
        private IRow row;
        private int currentRow;

        public ExcelHelper(string FileName)
        {
            this.FileName = FileName + ".xlsx";
            InitExcel();
        }

        private void InitExcel()
        {
            sheet1 = wbook.CreateSheet("评论");
            List<string> title = new List<string>();
            foreach (var str in MainWin.Columns)
            {
                title.Add(str);
            }

            this.SetTitle(title);
        }

        public bool AddRow(Common.Cmt rowData)
        {
            //int col = 0;
            IRow row = sheet1.CreateRow(currentRow);

            row.CreateCell(0).SetCellValue(rowData.name);
            row.CreateCell(1).SetCellValue(rowData.date);
            row.CreateCell(2).SetCellValue(rowData.location);
            row.CreateCell(3).SetCellValue(rowData.up);
            row.CreateCell(4).SetCellValue(rowData.content);
            
            currentRow++;
            /*
            foreach (var str in rowdata)
            {
                row.CreateCell(col).SetCellValue(str);
                col++;
            }
            currentRow++;*/
            return true;
        }

        /*
        public bool AddRow(List<string> rowdata)
        {
            int col = 0;
            IRow row = sheet1.CreateRow(currentRow);
            foreach (var str in rowdata)
            {
                row.CreateCell(col).SetCellValue(str);
                col++;
            }
            currentRow++;
            return true;
        }
        */
        public void SetTitle(List<string> title)
        {
            row = sheet1.CreateRow(0);
            
            int col = 0;
            foreach (var str in title)
            {
                row.CreateCell(col).SetCellValue(str);
                col++;
            }

            this.currentRow = 1;
        }

        public void Save()
        {
            try
            {
                FileStream stream = File.OpenWrite(FileName);
                wbook.Write(stream);
                stream.Close();
            }catch (Exception err)
            {
                MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
