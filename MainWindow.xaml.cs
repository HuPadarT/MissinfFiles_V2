using Microsoft.WindowsAPICodePack.Dialogs;
using MissingFiles_V2.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MissingFiles_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _srv = "192.168.67.17";
        private Int64 _filesCount;
        private Int64 _dbRows;
        private Int64 _found;
        private Int64 _missing;
        private int _prt = 3312;
        private string _dB_felh = "tpa";
        private string _dB_jsz = "6J3v3z";
        private string _dB_name = "franke_easy_final";
        private string _dB_Table = "w2s_einkauf";
        //private string repString = "";
        private string[] extension = new string[] { ".tif", ".pdf", ".doc", ".fax", ".txt" };
        private ObservableCollection<string> _errorlist_dir = new ObservableCollection<string>();
        private ObservableCollection<string> _found_files = new ObservableCollection<string>();
        private string _baseDir;
        private string _logFileDir;
        private bool _keres;
        private List<string> datarows = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            LogFileDir = @"C:\temp";
            elerut.Text = "Choose target directory.";
            this.DataContext = this;
        }

        #region program members
        public string LogFileDir
        {
            get
            {
                return this._logFileDir;
            }
            set
            {
                this._logFileDir = value;
                this.RaisePropertyChanged("LogFileDir");
            }
        }

        public int Port
        {
            get
            {
                return this._prt;
            }
            set
            {
                this._prt = value;
                this.RaisePropertyChanged("Port");
            }
        }

        public string DB_Table
        {
            get
            {
                return this._dB_Table;
            }
            set
            {
                this._dB_Table = value;
                this.RaisePropertyChanged("DB_Table");
            }
        }

        public string DB_name
        {
            get
            {
                return this._dB_name;
            }
            set
            {
                this._dB_name = value;
                this.RaisePropertyChanged("DB_name");
            }
        }

        public Int64 FilesCount
        {
            get
            {
                return this._filesCount;
            }
            set
            {
                this._filesCount = value;
                this.RaisePropertyChanged("FilesCount");
            }
        }

        public Int64 Found
        {
            get
            {
                return this._found;
            }
            set
            {
                this._found = value;
                this.RaisePropertyChanged("Found");
            }
        }

        public Int64 Missing
        {
            get
            {
                return this._missing;
            }
            set
            {
                this._missing = value;
                this.RaisePropertyChanged("Missing");
            }
        }

        public Int64 DbRows
        {
            get
            {
                return this._dbRows;
            }
            set
            {
                this._dbRows = value;
                this.RaisePropertyChanged("DbRows");
            }
        }

        public ObservableCollection<string> Errorlist_dir
        {
            get { return this._errorlist_dir; }
        }

        private void Add_Errorlist_dir(string item)
        {
            if (item != null)
            {
                Errorlist_dir.Add(item);
                RaisePropertyChanged("Errorlist_dir");
            }
        }

        public ObservableCollection<string> Found_files
        {
            get { return this._found_files; }
        }

        private void Add_Found_files(string item)
        {
            if (item != null)
            {
                Found_files.Add(item);
                RaisePropertyChanged("Found_files");
            }
        }

        public string Srv
        {
            get
            {
                return this._srv;
            }
            set
            {
                this._srv = value;
                this.RaisePropertyChanged("srv");
            }
        }

        public string BaseDir
        {
            get
            {
                return this._baseDir;
            }
            set
            {
                this._baseDir = value;
                this.RaisePropertyChanged("baseDir");
            }
        }

        public bool Keres
        {
            get
            {
                return this._keres;
            }
            set
            {
                this._keres = value;
                this.RaisePropertyChanged("keres");
            }
        }
        #endregion

        #region mouse click command
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void execClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void execBrowse(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dlg = new CommonOpenFileDialog())
            {

                string currentDirectory = @"&:\";
                dlg.Title = "Select folder";
                dlg.InitialDirectory = currentDirectory;

                dlg.AddToMostRecentlyUsedList = false;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = currentDirectory;
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;
                dlg.IsFolderPicker = true;
                CommonFileDialogResult result = dlg.ShowDialog();
                if (result.ToString().ToLower() == "ok")
                {
                    BaseDir = dlg.FileName;
                    //BaseDir = Path.GetFullPath(temp).Replace(Path.GetFileName(temp), string.Empty);
                    this.startdir.Text = BaseDir;
                    // MessageBox.Show("Kiválasztott file neve: " + dlg.FileName);
                    elerut.Text = "Press start button.";
                }
            }
        }

        private void execLogFilePath(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dlg = new CommonOpenFileDialog())
            {

                string currentDirectory = @"&:\";
                dlg.Title = "Select folder";
                dlg.InitialDirectory = currentDirectory;

                dlg.AddToMostRecentlyUsedList = false;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = currentDirectory;
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;
                dlg.IsFolderPicker = true;
                CommonFileDialogResult result = dlg.ShowDialog();
                if (result.ToString().ToLower() == "ok")
                {
                    LogFileDir = dlg.FileName;
                    LogFile.Text = LogFileDir;
                }
            }
        }

        private void execStart(object sender, RoutedEventArgs e)
        {
            Keres = true;
            if (_dB_Table == string.Empty)
            {
                elerut.Text = "Please enter data table name!";
                return;
            }
            if (_dB_name == string.Empty)
            {
                elerut.Text = "Please enter data base name!";
                return;
            }

            _found_files.Clear();
            _errorlist_dir.Clear();
            elerut.Text = "Reading Data Table...";
            Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.AppStarting;
                    this.Opacity = 0.84;
                });

                GetDbData();

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    elerut.Text = "Reading files...";
                }));

                List<string> files = Directory.EnumerateFiles(BaseDir, "*.*", SearchOption.AllDirectories).Where(
                                    s => s.EndsWith(".tif")
                                    || s.EndsWith(".pdf")
                                    //|| s.EndsWith(".doc")
                                    //|| s.EndsWith(".fax")
                                    //|| s.EndsWith(".txt")
                                    ).ToList();

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    elerut.Text = "Comparing files...";
                }));

                foreach (string d in datarows)
                {
                    bool megvan = false;
                    foreach (string f in files)
                    {
                        if (f.Contains(d))
                        {
                            //benne van
                            megvan = true;
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                            {
                                Add_Found_files(d);
                                Found = _found_files.Count();
                            }));
                            files.Remove(f);
                            break;
                        }
                    }
                    if (!megvan)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            Add_Errorlist_dir(d);
                            Missing = _errorlist_dir.Count();
                        }));
                    }
                }

            }).ContinueWith((a) =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        Keres = false;
                        if (Missing > 0) File.WriteAllLines(LogFileDir + @"\MissingFilesHunter.log", Errorlist_dir);
                        else File.WriteAllText(LogFileDir + @"\MissingFilesHunter.log", DateTime.UtcNow.ToString() + " No missing file.");
                        elerut.Text = "Finished.";
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                        });
                        this.Opacity = 1;
                        MessageBox.Show("Finished!");
                    }));
            });
        }

        private void execMoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void execTransparencyMenuItem(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).IsChecked)
                this.Opacity = 0.7;
            else
                this.Opacity = 1;
        }
        #endregion

        private void GetDbData()
        {

            string ConnString = "datasource=" + _srv + ";port=" + _prt + ";uid=" + _dB_felh + ";password=" + _dB_jsz;
            string Query = "SELECT path FROM " + _dB_name + "." + _dB_Table + ";";
            Console.WriteLine("Database downloading, please wait...");//"Az adatbázis letöltése, kérem várjon..."
            try
            {
                using (MySqlConnection myConnDb = new MySqlConnection(ConnString))
                {
                    using (MySqlCommand CommDb = new MySqlCommand(Query, myConnDb))
                    {
                        MySqlDataReader myReader;
                        myConnDb.Open();
                        myReader = CommDb.ExecuteReader();
                        while (myReader.Read())
                        {
                            datarows.Add(myReader.GetString("path").Replace(@"\\", @"\"));
                        }
                        myReader.Close();
                        myReader.Dispose();
                    }
                }
                DbRows = datarows.Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
