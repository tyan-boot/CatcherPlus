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
    //部分代码参考于 <优途科技>
    //http://blog.csdn.net/gisfarmer/article/details/3738959/

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
            /*
            app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = false;
            this.wBook = app.Workbooks.Add(true);
            this.wSheet = wBook.Worksheets[1];*/
        }

        public bool AddRow(List<string> rowdata)
        {
            int col = 0;
            IRow row = sheet1.CreateRow(currentRow);
            foreach (var str in rowdata)
            {
                row.CreateCell(col).SetCellValue(str);
                //this.wSheet.Cells[this.currentRow + 1, col + 1] = str;
                col++;
            }
            currentRow++;
            return true;
        }

        public void SetTitle(List<string> title)
        {
            row = sheet1.CreateRow(0);
            
            int col = 0;
            foreach (var str in title)
            {
                row.CreateCell(col).SetCellValue(str);
                //this.wSheet.Cells[1, col+1] = str;
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
                /*
                this.app.DisplayAlerts = false;
                app.AlertBeforeOverwriting = false;
                wBook.Save();
                app.Save("test.xlsx");
                app.SaveWorkspace("test.xlsx");
                app.Quit();
                app = null;*/
            }catch (Exception err)
            {
                MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return false;
            }

        }
    }
}
