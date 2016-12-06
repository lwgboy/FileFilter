using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace MD5Ceshi
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void SourceButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sourcefilepath.Text = dialog.SelectedPath;
            }
        }
        private void StartButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(targetpathfile.Text) || string.IsNullOrWhiteSpace(sourcefilepath.Text))
            {
                MessageBox.Show(this,"请选择【要筛选的文件夹】或者【筛选后的文件夹】!!!");
                return;
            }

            progressBar.Value = 0;
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(progressBar.SetValue);
            if (!Directory.Exists(targetpathfile.Text))
                Directory.CreateDirectory(targetpathfile.Text);
            var fils = Directory.GetFiles(sourcefilepath.Text);
            Dictionary<string, string> filinfo = new Dictionary<string, string>();
            bool isreplace = MessageBox.Show("如果有相同命名的文件，是否替换？", "替换文件", MessageBoxButton.YesNo,
                    MessageBoxImage.Information) == MessageBoxResult.Yes;
            for (int i = 1; i <= fils.Length; i++)
            {
                var t = fils[i - 1];
                var md5 = GetMD5HashFromFile(t);
                if (!string.IsNullOrWhiteSpace(md5) && !filinfo.Keys.Contains(md5))
                {
                    var item = t;
                    filinfo.Add(md5, item);
                    var filepath = System.IO.Path.Combine(targetpathfile.Text, System.IO.Path.GetFileName(item));
                    if (File.Exists(filepath))
                    {
                        if (isreplace)
                            File.Copy(item, filepath, true);
                    }
                    else
                    {
                        File.Copy(item, filepath, true);
                    }
                }
                Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { RangeBase.ValueProperty, Convert.ToDouble((i * 100) / fils.Length) });

            }
            System.Windows.MessageBox.Show(this,"筛选成功");
        }

        private void TargeButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "要导出的文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                targetpathfile.Text = dialog.SelectedPath;
            }
        }

        public static string GetMD5HashFromFile(string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("GetMD5HashFromFile() fail,error:" + ex.Message);
                return "";
            }

        }
    }
}
