using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using System.IO;
using System.Collections.Generic;
using Telerik.Windows.Data;
using System.Text;

namespace PoliceSMS.ViewModel
{

    public class ExportCommand : ICommand
    {
        private readonly ExportingModel model;

        public ExportCommand(ExportingModel model)
        {
            this.model = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.model.Export(parameter);
        }
    }

    public class ExportingModel : ViewModelBase
    {
        public ExportingModel()
        {
            this.ExportCommand = new ExportCommand(this);
        }

        private ExportCommand _exportCommand = null;

        public ExportCommand ExportCommand
        {
            get
            {
                return this._exportCommand;
            }
            set
            {
                if (this._exportCommand != value)
                {
                    this._exportCommand = value;
                    OnPropertyChanged("ExportCommand");
                }
            }
        }

        public void Export(object parameter)
        {
            RadGridView grid = parameter as RadGridView;
            if (grid != null)
            {
                grid.ElementExporting -= this.ElementExporting;
                grid.ElementExporting += this.ElementExporting;

                string extension = "";
                ExportFormat format = ExportFormat.Html;

                switch (SelectedExportFormat)
                {
                    case "Excel": extension = "xls";
                        format = ExportFormat.Html;
                        break;
                    case "ExcelML": extension = "xml";
                        format = ExportFormat.ExcelML;
                        break;
                    case "Word": extension = "doc";
                        format = ExportFormat.Html;
                        break;
                    case "Csv": extension = "csv";
                        format = ExportFormat.Csv;
                        break;
                }

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = extension;
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, SelectedExportFormat);
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        GridViewExportOptions exportOptions = new GridViewExportOptions();
                        exportOptions.Format = format;
                        exportOptions.ShowColumnFooters = false;
                        exportOptions.ShowColumnHeaders = true;
                        exportOptions.ShowGroupFooters = true;
                        exportOptions.Encoding = Encoding.Unicode;

                        grid.Export(stream, exportOptions);
                    }
                }
            }
        }

        public void Export(List<RadGridView> parameter)
        {
            string extension = "";
            ExportFormat format = ExportFormat.Html;

            switch (SelectedExportFormat)
            {
                case "Excel": extension = "xls";
                    format = ExportFormat.Html;
                    break;
                case "ExcelML": extension = "xml";
                    format = ExportFormat.ExcelML;
                    break;
                case "Word": extension = "doc";
                    format = ExportFormat.Html;
                    break;
                case "Csv": extension = "csv";
                    format = ExportFormat.Csv;
                    break;
            }
            if (parameter != null)
            {

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = extension;
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, SelectedExportFormat);
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {

                    using (Stream stream = dialog.OpenFile())
                    {

                        foreach (RadGridView grid in parameter)
                        {
                            grid.ElementExporting -= this.ElementExporting;
                            grid.ElementExporting += this.ElementExporting;


                            GridViewExportOptions exportOptions = new GridViewExportOptions();
                            exportOptions.Format = format;
                            exportOptions.ShowColumnFooters = false;
                            exportOptions.ShowColumnHeaders = true;
                            exportOptions.ShowGroupFooters = false;
                            exportOptions.Encoding = Encoding.Unicode;

                            grid.Export(stream, exportOptions);
                        }
                    }
                }
            }
        }
        public void ExportWithOptions(object parameter, GridViewExportOptions exportOptions)
        {
            RadGridView grid = parameter as RadGridView;
            if (grid != null)
            {
                grid.ElementExporting -= this.ElementExporting;
                grid.ElementExporting += this.ElementExporting;

                string extension = "";

                switch (SelectedExportFormat)
                {
                    case "Excel": extension = "xls";
                        break;
                    case "ExcelML": extension = "xml";
                        break;
                    case "Word": extension = "doc";
                        break;
                    case "Csv": extension = "csv";
                        break;
                }

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = extension;
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, SelectedExportFormat);
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        grid.Export(stream, exportOptions);
                    }
                }
            }
        }

        private string createHeaderStr(Header header)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("<tr style=\"background: #D3D3D3; color: #000000; font-size: 16; font-weight: Bold\">");
            foreach (var item in header.cells)
            {
                sb.Append(string.Format("<td style=\"text-align:center;\" colspan=\"{0}\">{1}</td>", item.ColSpan, item.Name));
            }
            
            sb.Append("</tr>");

            return sb.ToString();
        }

        public void ExportWithHeader(object parameter, Header header)
        {
            RadGridView grid = parameter as RadGridView;
            if (grid != null)
            {
                grid.ElementExporting -= this.ElementExporting;
                grid.ElementExporting += this.ElementExporting;

                string extension = "";
                ExportFormat format = ExportFormat.Html;

                switch (SelectedExportFormat)
                {
                    case "Excel": extension = "xls";
                        format = ExportFormat.Html;
                        break;
                    case "ExcelML": extension = "xml";
                        format = ExportFormat.ExcelML;
                        break;
                    case "Word": extension = "doc";
                        format = ExportFormat.Html;
                        break;
                    case "Csv": extension = "csv";
                        format = ExportFormat.Csv;
                        break;
                }

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = extension;
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, SelectedExportFormat);
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {

                        GridViewExportOptions exportOptions = new GridViewExportOptions();
                        exportOptions.Format = format;
                        exportOptions.ShowColumnFooters = false;
                        exportOptions.ShowColumnHeaders = true;
                        exportOptions.ShowGroupFooters = true;
                        exportOptions.Encoding = Encoding.UTF8;

                        grid.Export(stream, exportOptions);

                        byte[] bs = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(bs, 0, bs.Length);
                        string str = Encoding.UTF8.GetString(bs, 0, bs.Length);
                        string str1 = str.Insert(str.IndexOf('>', 0) + 1, createHeaderStr(header));
                        byte[] bys = Encoding.UTF8.GetBytes(str1);
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Write(bys, 0, bys.Length);
                        stream.Flush();
                    }

                }
            }
        }

        public void ExportWithHeader(List<object> parameter, List<Header> headers, List<bool> showFooter)
        {
            string extension = "";
            ExportFormat format = ExportFormat.Html;

            switch (SelectedExportFormat)
            {
                case "Excel": extension = "xls";
                    format = ExportFormat.Html;
                    break;
                case "ExcelML": extension = "xml";
                    format = ExportFormat.ExcelML;
                    break;
                case "Word": extension = "doc";
                    format = ExportFormat.Html;
                    break;
                case "Csv": extension = "csv";
                    format = ExportFormat.Csv;
                    break;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = extension;
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, SelectedExportFormat);
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < parameter.Count; i++)
                    {
                        string header = createHeaderStr(headers[i]);
                        RadGridView grid = parameter[i] as RadGridView;
                        if (grid != null)
                        {
                            grid.ElementExporting -= this.ElementExporting;
                            grid.ElementExporting += this.ElementExporting;



                            GridViewExportOptions exportOptions = new GridViewExportOptions();
                            exportOptions.Format = format;
                            exportOptions.ShowColumnFooters = showFooter[i];
                            exportOptions.ShowColumnHeaders = true;
                            exportOptions.ShowGroupFooters = true;
                            exportOptions.Encoding = Encoding.UTF8;

                            using (MemoryStream ms = new MemoryStream())
                            {
                                grid.Export(ms, exportOptions);

                                byte[] bs = new byte[ms.Length];
                                ms.Seek(0, SeekOrigin.Begin);
                                ms.Read(bs, 0, bs.Length);
                                string str = Encoding.UTF8.GetString(bs, 0, bs.Length);
                                string str1 = str.Insert(str.IndexOf('>', 0) + 1, header);

                                sb.Append(str1);
                            }

                        }
                    }
                    byte[] bysall = Encoding.UTF8.GetBytes(sb.ToString());
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Write(bysall, 0, bysall.Length);
                    stream.Flush();
                }
            }
        }

        IEnumerable<string> _exportFormats;
        public IEnumerable<string> ExportFormats
        {
            get
            {
                if (_exportFormats == null)
                {
                    _exportFormats = new string[] { "Excel", "Word" };
                }

                return _exportFormats;
            }
        }

        string _selectedExportFormat;
        public string SelectedExportFormat
        {
            get
            {
                return _selectedExportFormat;
            }
            set
            {
                if (!object.Equals(_selectedExportFormat, value))
                {
                    _selectedExportFormat = value;

                    OnPropertyChanged("SelectedExportFormat");
                }
            }
        }

        private void ElementExporting(object sender, GridViewElementExportingEventArgs e)
        {
            e.Width *= 1.4;
            if (e.Element == ExportElement.HeaderRow || e.Element == ExportElement.FooterRow
                || e.Element == ExportElement.GroupFooterRow)
            {

                e.Background = HeaderBackground;
                e.Foreground = HeaderForeground;
                e.FontSize = 16;
                e.FontWeight = FontWeights.Bold;
            }
            else if (e.Element == ExportElement.Row)
            {
                e.Background = RowBackground;
                e.Foreground = RowForeground;
            }
            else if (e.Element == ExportElement.Cell &&
                e.Value != null && e.Value.Equals("Chocolade"))
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;
                e.Foreground = Colors.Blue;
            }
            else if (e.Element == ExportElement.GroupHeaderRow)
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;
                e.Height = 24;
            }
            else if (e.Element == ExportElement.GroupHeaderCell &&
                e.Value != null && e.Value.Equals("Chocolade"))
            {
                e.Value = "MyNewValue";
            }
            else if (e.Element == ExportElement.GroupFooterCell)
            {
                GridViewDataColumn column = e.Context as GridViewDataColumn;
                QueryableCollectionViewGroup qcvGroup = e.Value as QueryableCollectionViewGroup;

                if (column != null && qcvGroup != null && column.AggregateFunctions.Count > 0)
                {
                    e.Value = GetAggregates(qcvGroup, column);
                }
            }
        }

        private string GetAggregates(QueryableCollectionViewGroup group, GridViewDataColumn column)
        {
            List<string> aggregates = new List<string>();

            foreach (AggregateFunction f in column.AggregateFunctions)
            {
                foreach (AggregateResult r in group.AggregateResults)
                {
                    if (f.FunctionName == r.FunctionName && r.FormattedValue != null)
                    {
                        aggregates.Add(r.FormattedValue.ToString());
                    }
                }
            }

            return String.Join(",", aggregates.ToArray());
        }

        private Color _headerBackground = Colors.LightGray;
        public Color HeaderBackground
        {
            get
            {
                return this._headerBackground;
            }
            set
            {
                if (this._headerBackground != value)
                {
                    this._headerBackground = value;
                    OnPropertyChanged("HeaderBackground");
                }
            }
        }

        private Color _rowBackground = Colors.White;
        public Color RowBackground
        {
            get
            {
                return this._rowBackground;
            }
            set
            {
                if (this._rowBackground != value)
                {
                    this._rowBackground = value;
                    OnPropertyChanged("RowBackground");
                }
            }
        }

        Color _headerForeground = Colors.Black;
        public Color HeaderForeground
        {
            get
            {
                return this._headerForeground;
            }
            set
            {
                if (this._headerForeground != value)
                {
                    this._headerForeground = value;
                    OnPropertyChanged("HeaderForeground");
                }
            }
        }

        Color _rowForeground = Colors.Black;
        public Color RowForeground
        {
            get
            {
                return this._rowForeground;
            }
            set
            {
                if (this._rowForeground != value)
                {
                    this._rowForeground = value;
                    OnPropertyChanged("RowForeground");
                }
            }
        }

    }

    public class Header
    {
        public HeaderCell[] cells { get; set; }
    }

    public class HeaderCell
    {
        public string Name { get; set; }

        public int ColSpan { get; set; }
    }
    
}
