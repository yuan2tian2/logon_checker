////////////////////////////////////////////////////////////////////////////////////////////////////
/// (C) Copyright HARADA, Takahiko 2018 All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Text;
////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>ログオンとログオフのペア</summary>
public class LogonAndLogoff
{
    /// <summary>ユーザＩＤ</summary>
    public String UserId{get;set;}
    
    /// <summary>ログオン日時</summary>
    public DateTime StartDatetime{get;set;}
    
    /// <summary>ログオフ日時</summary>
    public DateTime EndDatetime{get;set;}
    
    //----------------------------------------------------------------------------------------------
    /// <summary>このオブジェクトの文字列表現を返す</summary>
    /// <return>このオブジェクトの文字列表現</return>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(UserId);
        sb.Append(Constants.DELIMITER);
        sb.Append(StartDatetime.ToString(Constants.DATETIME_FORMAT));
        sb.Append(Constants.DELIMITER);
        sb.Append(EndDatetime.ToString(Constants.DATETIME_FORMAT));
        return sb.ToString();
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>このオブジェクトのデータを文字列配列に格納して返す</summary>
    /// <return>文字列配列に格納したデータ</return>
    public string[] ToArray()
    {
        string[] array = new string[4];
        array[0] = UserId;
        array[1] = StartDatetime.ToString(Constants.DATETIME_FORMAT);
        array[2] = EndDatetime.ToString(Constants.DATETIME_FORMAT);
        array[3] = RunningTime.ToString();
        return array;
    }
    //----------------------------------------------------------------------------------------------
    /// <summary>稼動時間を返す</summary>
    /// <return>稼動時間</return>
    public TimeSpan RunningTime
    {
        get
        {
            return EndDatetime - StartDatetime;
        }
    }
    
}
