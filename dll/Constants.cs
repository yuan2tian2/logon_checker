////////////////////////////////////////////////////////////////////////////////////////////////////
/// (C) Copyright HARADA, Takahiko 2018 All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>定数定義</summary>
public class Constants
{
    /// <summary>プログラム名</summary>
    public const string PROGRAM_NAME = "PC稼働時間リスト作成";
    
    /// <summary>バージョン</summary>
    public const string VERSION = "1.2.0";
    
    /// <summary>日付時刻書式</summary>
    public const string DATETIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";
    
    /// <summary>クエリ用日付書式</summary>
    public const string QUERY_DATETIME_FORM = "yyyy\'-\'MM\'-\'dd\'T\'HH\':\'mm\':\'ss\'Z\'";
    
    /// <summary>イベントログを検索するクエリ文字列</summary>
    public const string QUERY_STRING = "<QueryList><Query Id=\"0\"" 
                + "Path=\"Microsoft-Windows-User Profile Service/Operational\">"
                + "<Select Path=\"Microsoft-Windows-User Profile Service/Operational\">"
                + "*[System[(EventID=2 or EventID=4) "
                + "and TimeCreated[@SystemTime&gt;=\'{0}\' and @SystemTime&lt;=\'{1}\']]]"
                + "</Select></Query></QueryList>";
    
    /// <summary>イベントログ名</summary>
    public const string LOG_NAME = "Application";
    
    /// <summary>区切り文字</summary>
    public const string DELIMITER = "\t";
    
    /// <summary>XMLのヘッダ</summary>
    public const string XML_HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<Events>\r\n";
    
    // <summary>XMLのフッタ</summary>
    public const string XML_FOOTER = "</Events>";
    //---------------------------------------------------------------------------------------------
    /// <summary>コンストラクタ</summary>
    private Constants()
    {
        throw new InvalidOperationException();
    }
}
