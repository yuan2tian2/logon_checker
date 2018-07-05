////////////////////////////////////////////////////////////////////////////////////////////////////
/// (C) Copyright HARADA, Takahiko 2018 All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Text;
////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>イベントログリーダー</summary>
public class EventReader
{
    /// <summary>このクラスの唯一のインスタンス</summary>
    private static EventReader instance = new EventReader();
    //---------------------------------------------------------------------------------------------
    /// <summary>コンストラクタ</summary>
    private EventReader()
    {
        
    }
    //---------------------------------------------------------------------------------------------
    /// <summary>このクラスの唯一のインスタンスを返す</summary>
    /// <return>このクラスの唯一のインスタンス</return>
    public static EventReader getInstance()
    {
        return instance;
    }
    //---------------------------------------------------------------------------------------------
    /// <summary>イベントログを取得しリストに格納して返す</summary>
    /// <param name="from">検索期間の開始年月日(UTC)</param>
    /// <param name="to">検索期間の終了年月日(UTC)</param>
    /// <return>ログオン、ログオフログのリスト</return>
    public List<LogonRecord> GetLogonData(DateTime from, DateTime to)
    {
        List<LogonRecord> list = new List<LogonRecord>();
        EventLogQuery query = CreateEventLogQuery(from, to);
        using(EventLogReader reader = new EventLogReader(query))
        {
            EventRecord  record;
            while((record = reader.ReadEvent()) != null)
            {
                LogonRecord datum = new LogonRecord(record);
                list.Add(datum);
            }
        }
        return list;
    }
    //---------------------------------------------------------------------------------------------
    /// <summary>検索を実行してXML文字列をListに格納する</summary>
    /// <param name="from">検索期間の開始年月日(UTC)</param>
    /// <param name="to">検索期間の終了年月日(UTC)</param>
    /// <return>検索結果のList</return>
    public List<string> GetXmlList(DateTime from, DateTime to)
    {
        List<string> list = new List<string>();
        EventLogQuery query = CreateEventLogQuery(from, to);
        using(EventLogReader reader = new EventLogReader(query))
        {
            EventRecord  record;
            while((record = reader.ReadEvent()) != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\t");
                sb.Append(record.ToXml());
                string renderingInfo = EditRenderingInfo(record);
                sb.Replace("</Event>", renderingInfo);
                sb.Append("\t</Event>\r\n");
                list.Add(sb.ToString());
            }
        }
        return list;
    }
    //---------------------------------------------------------------------------------------------
    /// <summary>追加のメッセージを編集する</summary>
    /// <param name="record">イベントログのレコード</param>
    /// <return>追加メッセージ文字列</return>
    protected string EditRenderingInfo(EventRecord  record)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("\r\n");
        sb.Append("\t\t<RenderingInfo Culture='ja-JP'>\r\n");
        sb.Append("\t\t\t<Message>");
        sb.Append(record.FormatDescription());
        sb.Append("</Message>\r\n");
        sb.Append("\t\t\t<Level>");
        sb.Append(record.LevelDisplayName);
        sb.Append("</Level>\r\n");
        sb.Append("\t\t\t<Task>");
        sb.Append(record.TaskDisplayName);
        sb.Append("</Task>\r\n");
        sb.Append("\t\t\t<Opcode>");
        sb.Append(record.OpcodeDisplayName);
        sb.Append("</Opcode>\r\n");
        sb.Append("\t\t\t<Channel>Microsoft-Windows-User Profile Service/Operational</Channel>\r\n");
        sb.Append("\t\t\t<Provider>Microsoft-Windows-User Profile Service</Provider>\r\n");
        sb.Append("\t\t\t<Keywords></Keywords>\r\n");
        sb.Append("\t\t</RenderingInfo>\r\n");
        return sb.ToString();
    }
    //---------------------------------------------------------------------------------------------
    /// <summary>イベントログのクエリオブジェクトを作成する</summary>
    /// <param name="from">検索期間の開始年月日(UTC)</param>
    /// <param name="to">検索期間の終了年月日(UTC)</param>
    /// <return>イベントログのクエリ</return>
    protected EventLogQuery CreateEventLogQuery(DateTime from, DateTime to)
    {
        string fromString = from.ToString(Constants.QUERY_DATETIME_FORM);
        string toString = to.ToString(Constants.QUERY_DATETIME_FORM);
        string query = String.Format(Constants.QUERY_STRING, fromString, toString);
        return new EventLogQuery(Constants.LOG_NAME, PathType.LogName, query);
    }
}
