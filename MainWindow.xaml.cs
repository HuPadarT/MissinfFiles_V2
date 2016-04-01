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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _srv = "192.168.67.17";
        private static Int64 _missing = 0;
        private static Int64 _fullcnt = 0;
        private static Int64 _dbRowsCount = 0;
        private static Int64 _filesInFolder = 0;
        private int _prt = 3312;
        private string _dB_felh = "tpa";
        private string _progressLine = string.Empty;
        private string _dB_jsz = "6J3v3z";
        private string _dB_name = "franke_easy_final";
        private string _dB_Table = "w2s_einkauf";
        //private string repString = "";
        private string[] extension = new string[] { ".tif", ".pdf", ".doc", ".fax", ".txt" };
        private static ObservableCollection<string> _errorlist_dir = new ObservableCollection<string>();
        private static ObservableCollection<string> _found_files = new ObservableCollection<string>();
        private string _baseDir;
        private string _logFileDir;
        private static bool _keres;
        private static List<string> datarows = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            LogFileDir = @"C:\temp";
            ProgressLine = "Choose target directory.";
        }

        #region program members
        public string ProgressLine
        {
            get
            {
                return this._progressLine;
            }
            set
            {
                this._progressLine = value;
                this.RaisePropertyChanged("ProgressLine");
            }
        }

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

        public Int64 DbRowsCount
        {
            get
            {
                return _dbRowsCount;
            }
            set
            {
                _dbRowsCount = value;
                RaisePropertyChanged("DbRowsCount");
            }
        }

        public Int64 Missing
        {
            get
            {
                return _missing;
            }
            set
            {
                _missing = value;
                RaisePropertyChanged("Missing");
            }
        }

        public Int64 Fullcnt
        {
            get
            {
                return _fullcnt;
            }
            set
            {
                _fullcnt = value;
                RaisePropertyChanged("Fullcnt");
            }
        }

        public Int64 FilesInFolder
        {
            get
            {
                return _filesInFolder;
            }
            set
            {
                _filesInFolder = value;
                RaisePropertyChanged("FilesInFolder");
            }
        }

        public ObservableCollection<string> Errorlist_dir
        {
            get { return _errorlist_dir; }
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
            get { return _found_files; }
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
                return _keres;
            }
            set
            {
                _keres = value;
                RaisePropertyChanged("keres");
            }
        }
        #endregion

        private string BrowseFolder()
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
                    //BaseDir = Path.GetFullPath(temp).Replace(Path.GetFileName(temp), string.Empty);
                    // MessageBox.Show("Kiválasztott file neve: " + dlg.FileName);
                    return dlg.FileName;
                }
                return "";
            }
        }

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
            string dir = BrowseFolder();
            this.startdir.Text = dir;
            ProgressLine = "Press start button.";
        }

        private void execLogFilePath(object sender, RoutedEventArgs e)
        {
            string dir = BrowseFolder();
            LogFileDir = dir;

        }

        private void execStart(object sender, RoutedEventArgs e)
        {
            Keres = true;
            InitCounters();
            if (_dB_Table == string.Empty)
            {
                ProgressLine = "Please enter data table name!";
                return;
            }
            if (_dB_name == string.Empty)
            {
                ProgressLine = "Please enter data base name!";
                return;
            }

            ProgressLine = "Reading Data Table...";
            string f_name = BaseDir.Replace("\\", "_") + ".txt";
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
                    ProgressLine = "Reading files...";
                }));

                List<string> files = new List<string>();

                string f_list = LogFileDir + "\\msh_v2_lists_temp\\" + f_name;
                try
                {
                    if (!File.Exists(f_list))
                    {
                        if (!Directory.Exists(LogFileDir + "\\msh_v2_lists_temp\\"))
                            Directory.CreateDirectory(LogFileDir + "\\msh_v2_lists_temp\\");
                        files = Directory.EnumerateFiles(BaseDir, "*.*", SearchOption.AllDirectories).Where(
                                            s => s.EndsWith(".tif")
                                            || s.EndsWith(".pdf")
                                            || s.EndsWith(".blob")
                                            //|| s.EndsWith(".doc") // if we needed other files
                                            //|| s.EndsWith(".fax")
                                            //|| s.EndsWith(".txt")
                                            ).ToList();
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            ProgressLine = "Writing lines into file...";
                        }));
                        File.WriteAllLines(f_list, files);
                    }
                    else
                    {
                        foreach (string l in File.ReadAllLines(f_list))
                        {
                            files.Add(l);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    ProgressLine = "Comparing files...";
                    FilesInFolder = files.Count();
                }));

                int mCnt = 0;
                foreach (string d in datarows)
                {
                    #region new method
                    //string f_name = BaseDir + "\\" + d;
                    //if(File.Exists(BaseDir+d)) // if file exist in target directory
                    //{
                    //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    //    {
                    //        ++fCnt;
                    //        Add_Found_files(d);
                    //        Found = fCnt;
                    //    }));
                    //}
                    //else  // if not exist
                    //{
                    //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    //    {
                    //        ++mCnt;
                    //        Add_Errorlist_dir(d);
                    //        Missing = mCnt;
                    //    }));
                    //}
                    #endregion

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
                            }));
                            files.Remove(f);
                            break;
                        }
                    }
                    if (!megvan)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            ++mCnt;
                            Add_Errorlist_dir(d);
                            Missing = mCnt;
                        }));
                    }
                    ++Fullcnt;
                }

            }).ContinueWith((a) =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        Keres = false;
                        string[] tmp = f_name.Split('_');
                        string temp = tmp[tmp.Count() - 1];
                        string log_f_name = LogFileDir +"\\Missing_Files_Hunter_" + Path.ChangeExtension(temp,".log");
                        if (Missing > 0) File.WriteAllLines(log_f_name, Errorlist_dir);
                        else File.WriteAllText(log_f_name, DateTime.UtcNow.ToString() + " No missing file.");
                        ProgressLine = "Finished.";
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                        });
                        this.Opacity = 1;
                        MessageBox.Show("Finished!");
                    }));
            });
        }

        private void InitCounters()
        {
            FilesInFolder = 0;
            Missing = 0;
            DbRowsCount = 0;
            Fullcnt = 0;
            datarows.Clear();
            _found_files.Clear();
            _errorlist_dir.Clear();
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DbRowsCount = datarows.Count();
                });
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
