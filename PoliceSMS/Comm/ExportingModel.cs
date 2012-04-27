using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Data;
using System.IO;
using System.Windows;
using System;
using System.Windows.Input;

#if !SILVERLIGHT
using Microsoft.Win32;
#else
using System.Windows.Controls;
using System.Text;
#endif

namespace PoliceSMS.Comm
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
                        exportOptions.ShowColumnFooters = true;
                        exportOptions.ShowColumnHeaders = true;
                        exportOptions.ShowGroupFooters = true;
                        exportOptions.Encoding = Encoding.Unicode;
                            
                        grid.Export(stream, exportOptions);
                    }
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
                    _exportFormats = new string[] { "Excel", "ExcelML", "Word", "Csv" };
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

                if (column != null && qcvGroup != null && column.AggregateFunctions.Count() > 0)
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
}