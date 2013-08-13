using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace allocate
{
  public static class WebUtilities
  {
    private static int timeout = 1000000000;

    public static string HttpPost (string URI, string Parameters, CookieContainer cookies)
    {
      System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.HttpWebRequest.Create (URI);
      req.CookieContainer = cookies;
      req.Proxy = null; 
      req.KeepAlive = true;
      req.Timeout = timeout; 
      req.ContentType = "application/x-www-form-urlencoded";
      req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0) Gecko/20100101 Firefox/4.0";
      req.Method = "POST";
      req.AllowAutoRedirect = false;
      byte [] bytes = System.Text.Encoding.ASCII.GetBytes (Parameters);
      req.ContentLength = bytes.Length;
      System.IO.Stream os = req.GetRequestStream ();
      os.Write (bytes, 0, bytes.Length); 
      os.Close ();
      HttpWebResponse response = (HttpWebResponse)req.GetResponse ();
      if (response == null)
        return null;
      if (response.StatusCode == HttpStatusCode.Found) { 
        foreach (Cookie cookie in response.Cookies) { 
          cookies.Add(cookie); 
        }
      }
      System.IO.StreamReader sr = new System.IO.StreamReader (response.GetResponseStream ());
      return sr.ReadToEnd ().Trim ();
    }

    public static string HttpGet (string URI, CookieContainer cookies)
    {
      System.Net.HttpWebRequest req = (HttpWebRequest)WebRequest.Create (URI);
      req.CookieContainer = cookies;
      req.Timeout = timeout;
      req.KeepAlive = true;
      req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0) Gecko/20100101 Firefox/4.0";
      req.AllowAutoRedirect = false; 
      System.Net.HttpWebResponse resp = (HttpWebResponse)req.GetResponse ();
      foreach (Cookie cookie in resp.Cookies) { 
        cookies.Add(cookie); 
      }
      System.IO.StreamReader sr = new System.IO.StreamReader (resp.GetResponseStream ());
      return sr.ReadToEnd ().Trim ();
    }

    public static string BuildParameters (NameValueCollection parameters)
    { 
      string result = ""; 
      foreach (string parameter in parameters.AllKeys) { 
        result += parameter + "=" + parameters[parameter] + "&"; 
      }
      result = result.TrimEnd ('&'); 
      return result; 
    }
  }
}

