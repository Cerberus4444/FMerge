using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace FMerge
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> FilesToMove = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(txt_MainFolderPath.Text))
            {
                if (Directory.GetDirectories(txt_MainFolderPath.Text).Length > 0)
                {
                    foreach (string d in Directory.GetDirectories(txt_MainFolderPath.Text))
                    {
                        getFilesRecursively(d);
                    }

                    if (FilesToMove.Count > 0)
                    {
                        foreach (string f in FilesToMove)
                        {
                            if (File.Exists(f))
                            {
                                if (File.Exists(txt_MainFolderPath.Text + f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar))))
                                {
                                    for (int i = 1; i < 1000000; i++)
                                    {
                                        string fname = txt_MainFolderPath.Text + f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar));

                                        if (!File.Exists(fname.Substring(0, fname.LastIndexOf(".")) + "(" + i + ")" + fname.Substring(fname.LastIndexOf("."))))
                                        {
                                            File.Move(f, fname.Substring(0, fname.LastIndexOf(".")) + "(" + i + ")" + fname.Substring(fname.LastIndexOf(".")));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    File.Move(f, txt_MainFolderPath.Text + f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar)));
                                }
                            }
                        }

                        MessageBox.Show("All files successfully added to: " + Environment.NewLine + txt_MainFolderPath.Text, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Oh no...I promise I crawled through each and every subfolder, but I didn't find any files (that matched your filter settings) :-O", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    if (MessageBox.Show("Shall I now delete all subfolders?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (string d in Directory.GetDirectories(txt_MainFolderPath.Text))
                        {
                            Directory.Delete(d, true);
                        }

                        MessageBox.Show("They shall rot in your recycle bin :-P", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Err... there are no subfolders in here :-/", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("You might at least give me a valid folder path to work with! >:-(", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void getFilesRecursively(String directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                if (chk_FileType_mkv.IsChecked.Value)
                {
                    if (file.Substring(file.LastIndexOf(".")) == ".mkv")
                    {
                        FilesToMove.Add(file);
                    }
                }
                else
                {
                    FilesToMove.Add(file);
                }
            }

            foreach (string d in Directory.GetDirectories(directory))
            {
                getFilesRecursively(d);
            }
        }
    }
}
