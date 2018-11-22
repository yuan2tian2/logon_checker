////////////////////////////////////////////////////////////////////////////////////////////////////
/// (C) Copyright KUBO, Yoshihito 2018 All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Security.Principal;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>ログオンとログオフのログ</summary>
public class LogonRecord : IComparable<LogonRecord>
{
    /// <summary>イベント発生日時</summary>
    public DateTime EventDatetime{get;set;}
    
    /// <summary>ユーザＩＤ</summary>
    public string UserId{get;set;}
    
    /// <summary>マシン名</summary>
    public string MachineName{get;set;}
    
    /// <summary>ログプロバイダ名</summary>
    public string ProviderName{get; set;}
    
    /// <summary>イベントID</summary>
    public int EventId{get;set;}
    
    /// <summary>メッセージ</summary>
    public string Message{get;set;}
    //----------------------------------------------------------------------------------------------
    /// <summary>ディフォルトコンストラクタ</summary>
    public LogonRecord()
    {
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>コンストラクタ</summary>
    /// <param name="arg">イベントログレコード</param>
    public LogonRecord(EventRecord arg)
    {
        // ユーザIDをSIDからアカウント名に変換する
        try
        {
            NTAccount account = (NTAccount)arg.UserId.Translate(typeof(NTAccount));
            this.UserId = account.ToString();
        }
        catch(Exception e)
        {
            this.UserId = e.Message;
        }
        this.EventDatetime = (DateTime)arg.TimeCreated;
        this.MachineName = arg.MachineName.ToString();
        this.ProviderName = arg.ProviderName;
        this.Message = arg.FormatDescription();
        this.EventId = arg.Id;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>別のオブジェクトとの大小を比較する(発生時刻順)</summary>
    public int CompareTo(LogonRecord another)
    {
        return this.EventDatetime.CompareTo(another.EventDatetime);
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>このオブジェクトの文字列表現を返す</summary>
    /// <return>このオブジェクトの文字列表現</return>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("MachineName:");
        sb.Append(MachineName);
        sb.Append(Constants.DELIMITER);
        sb.Append("UserId:");
        sb.Append(UserId);
        sb.Append(Constants.DELIMITER);
        sb.Append(EventDatetime.ToString(Constants.DATETIME_FORMAT));
        sb.Append(Constants.DELIMITER);
        sb.Append(Message);
        return sb.ToString();
    }
}
