////////////////////////////////////////////////////////////////////////////////////////////////////
/// (C) Copyright HARADA, Takahiko 2018 All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summay>Windowsのログオン時刻を調査するツール</summary>
public class LogonChecker
{
    /// <summay>このクラスの唯一のインスタンス</summary>
    private static LogonChecker instance = new LogonChecker();
    //----------------------------------------------------------------------------------------------
    /// <summary>コンストラクタ</summary>
    private LogonChecker()
    {
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>このクラスの唯一のインスタンスを返す</summary>
    public static LogonChecker getInstance()
    {
        return instance;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>エントリーポイント(コマンドライン)</summary>
    /// <param name="args">ダミー</param>
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("ログオン時間チェッカー START");
            LogonChecker app = getInstance();
            List<LogonAndLogoff> list = app.getLogonAndLogoffRecords();
            foreach(LogonAndLogoff rec in list)
            {
                Console.WriteLine(rec.ToString());
            }
            Console.WriteLine("ログオン時間チェッカー END");
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Press Enter");
        Console.ReadLine();
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>ログオン、ログオフ時刻情報のリストを取得する</summary>
    /// <return>ログオン、ログオフ時刻情報のリスト</return>
    public List<LogonAndLogoff> getLogonAndLogoffRecords()
    {
        return getLogonAndLogoffRecords(DateTime.MinValue, DateTime.MaxValue);
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>ログオン、ログオフ時刻情報のリストを取得する</summary>
    /// <param name="from">検索期間の開始</param>
    /// <param name="to">検索基幹の終了</param>
    /// <return>ログオン、ログオフ時刻情報のリスト</return>
    public List<LogonAndLogoff> getLogonAndLogoffRecords(DateTime from, DateTime to)
    {
        EventReader reader = EventReader.getInstance();
        List<LogonRecord> records = reader.GetLogonData(from, to);
        records.Sort();
        List<LogonAndLogoff> list = new List<LogonAndLogoff>();
        DateTime max = DateTime.MinValue;
        DateTime min = DateTime.MaxValue;
        foreach(LogonRecord record in records)
        {
            if(record.EventId == 2)
            {
                min = record.EventDatetime;
            }
            else if(record.EventId == 4)
            {
                max = record.EventDatetime;
                LogonAndLogoff dailyResult = new LogonAndLogoff();
                dailyResult.UserId = record.UserId;
                dailyResult.StartDatetime = min;
                dailyResult.EndDatetime = max;
                if(min < DateTime.MaxValue && max > DateTime.MinValue)
                {
                    list.Add(dailyResult);
                }
                min = DateTime.MinValue;
                max = DateTime.MaxValue;
            }
        }
        return list;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>XML形式のドキュメントを作成する</summary>
    /// <return>XMLドキュメント</return>
    public XmlDocument GetXml()
    {
        return GetXml(DateTime.MinValue, DateTime.MaxValue);
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>XML形式のドキュメントを作成する</summary>
    /// <return>XMLドキュメント</return>
    public XmlDocument GetXml(DateTime from, DateTime to)
    {
        XmlDocument xml = new XmlDocument();
        EventReader reader = EventReader.getInstance();
        List<string> xmlList = reader.GetXmlList(from , to);
        StringBuilder sb = new StringBuilder();
        sb.Append(Constants.XML_HEADER);
        foreach(string element in xmlList)
        {
            sb.Append(element);
            sb.Append("\r\n");
        }
        sb.Append(Constants.XML_FOOTER);
        xml.LoadXml(sb.ToString());
        return xml;
    }
}
