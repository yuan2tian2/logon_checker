////////////////////////////////////////////////////////////////////////////////////////////////////
/// (C) Copyright HARADA, Takahiko All rights Reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// ＰＣ稼働時間リスト作成ＧＵＩ制御
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>ログオンログオフデータ</summary>
    private List<LogonAndLogoff> list = null;
     //----------------------------------------------------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        lblVersion.Content = String.Format("Version {0}", Constants.VERSION);
        this.ResizeMode = ResizeMode.CanMinimize;
        btnExport.IsEnabled = false;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>イベントログの読込</summary>
    /// <param name="sender">イベント送信オブジェクト</param>
    /// <param name="e">イベントパラメータ</param>
    private void btnRead_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            DateTime from = GetFromDateTime();
            DateTime to = GetToDateTime();
            LogonChecker checker = LogonChecker.getInstance();
            list = checker.getLogonAndLogoffRecords(from, to);
            dataGrid.ItemsSource = list;
            btnExport.IsEnabled = true;
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>検索期間の終了年月日(UTC)を算出する</summary>
    /// <return>検索期間の終了年月日(UTC)</return>
    private DateTime GetToDateTime()
    {
        TimeZoneInfo timezone = TimeZoneInfo.Local;
        DateTime to = dateTo.SelectedDate ?? DateTime.Now;
        DateTime toDate = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59, 99);
        toDate -= timezone.BaseUtcOffset;
        return toDate;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>検索期間の開始年月日(UTC)を算出する</summary>
    /// <return>検索期間の開始年月日(UTC)</return>
    private DateTime GetFromDateTime()
    {
        TimeZoneInfo timezone = TimeZoneInfo.Local;
        DateTime from = dateFrom.SelectedDate ?? DateTime.MinValue;
        DateTime fromDate = DateTime.MinValue;
        if(from > DateTime.MinValue)
        {
            fromDate = from - timezone.BaseUtcOffset;
        }
        return fromDate;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>生ログXML出力</summary>
    /// <param name="sender">イベント送信オブジェクト</param>
    /// <param name="e">イベントパラメータ</param>
    private void btnXml_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XMLファイル|*.xml|すべてのファイル|*.*";
            if(dialog.ShowDialog() == true)
            {
                DateTime from = GetFromDateTime();
                DateTime to = GetToDateTime();
                LogonChecker checker = LogonChecker.getInstance();
                XmlDocument xml = checker.GetXml(from, to);
                string filePath = dialog.FileName;
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "    ";
                using(FileStream stream = new FileStream(filePath, FileMode.Create))
                using(XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    xml.Save(writer);
                }
                MessageBox.Show("生ログをXMLファイルに保存しました。");
            }
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>タブ区切りテキストファイルにエクスポートする</summary>
    /// <param name="sender">イベント送信オブジェクト</param>
    /// <param name="e">イベントパラメータ</param>
    private void btnExport_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if(list == null)
            {
                MessageBox.Show("ログが読み込まれていません");
                return;
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "名前をつけて保存";
            dialog.Filter = "テキストファイル|*.txt|すべてのファイル|*.*";
            if(dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                using(StreamWriter writer = new StreamWriter(dialog.FileName, false, encoding))
                {
                    foreach(LogonAndLogoff entity in list)
                    {
                        writer.WriteLine(join(entity.ToArray(), "\t"));
                    }
                }
                MessageBox.Show("ファイルに出力しました。");
            }
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    //----------------------------------------------------------------------------------------------
    /// <summmary>文字列配列を区切り文字で連結する</summary>
    /// <param name="array">結合対象の文字列配列</param>
    /// <param name="delimiter">区切り文字</param>
    /// <return>結合した文字列</return>
    private string join(string[] array, string delimiter)
    {
        StringBuilder sb = new StringBuilder();
        string delim = "";
        foreach(string value in array)
        {
            sb.Append(delim);
            sb.Append(value);
            delim = delimiter;
        }
        return sb.ToString();
    }
}
