using System;
using CsQuery; 
using System.Collections.Specialized; 
using System.Net; 
using CommandLine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization; 


namespace allocate
{
  public class AllocateScraper {
    private string sessionId; 
    private List<ICalEvent> events; 
    private string loginUrl = "http://allocate.swin.edu.au/aplus/apstudent?fun=login"; 
    private string timetableUrl = "http://allocate.swin.edu.au/aplus/apstudent?fun=show_all_allocations&ss={0}";
    private CookieContainer cookieJar; 
    private ScraperOptions options;


    public AllocateScraper(ScraperOptions options)
    {
      this.options = options; 
      cookieJar = new CookieContainer(); 
      events = new List<ICalEvent> ();
    }

    public List<ICalEvent> GetEvents() { 
      return events; 
    }

    public void LogIn() { 
      NameValueCollection loginParams = new NameValueCollection(); 
      loginParams.Add("student_code",options.username); 
      loginParams.Add("password",options.password); 
      string loginResponse = WebUtilities.HttpPost(loginUrl,WebUtilities.BuildParameters(loginParams),cookieJar); 
      Regex session = new Regex("ss=([0-9]*)");
      MatchCollection matches = session.Matches(loginResponse); 
      foreach (Match match in matches) { 
        if (match.Length > 1) {
          sessionId = match.Groups[1].Value;
          break;
        }
      }
      if (sessionId == string.Empty) { 
        throw new Exception("Loggin in failed, please make sure that your login details are correct.");
      }
    }

    public void GetTimeTable() { 
      string timetable = WebUtilities.HttpGet(GetTimetableUrl(),cookieJar);
      CQ timetablePage = timetable; 
      foreach (var row in timetablePage["tr[bgcolor=#eeeeee]"]) { 
        CQ dataRow = row.InnerHTML;
        int columnNumber = 0;
        ICalEvent allocateEvent = new ICalEvent();
        foreach (var column in dataRow["td"]) {
          switch (columnNumber) { 
            case 0: 
              allocateEvent.Description = column.InnerText.Split('\n')[1];
              break;
            case 2: 
              allocateEvent.Description += " - "+ column.InnerText;
              break;
            case 4:
              allocateEvent.Day = column.InnerText.Substring(0,2).ToUpper();
              break;
            case 5:
              allocateEvent.StartTime = ParseTime(column.InnerText);
              break;
            case 7:
              allocateEvent.Location = column.InnerText; 
              break;
            case 9:
              allocateEvent.Duration  = int.Parse(column.InnerText);
              break;
            case 10:
              allocateEvent.StartDate = ParseWeeks(column.InnerText);
              break;
          }
          columnNumber++;
        }
        events.Add(allocateEvent); 
      }
    }

    public DateTime ParseTime(string time) { 
      return DateTime.Parse(time); 
    }

    private DateTime ParseWeeks(string weeks) { 
      DateTime parsedDate;
      string[] firstWeek = weeks.Split(',')[0].Split('-')[0].Split('/');
      string date = firstWeek[0].PadLeft(2,'0') + "/" + firstWeek[1].PadLeft(2,'0') + "/" + DateTime.Now.Year;
      if (!DateTime.TryParseExact(date,"dd/MM/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None,out parsedDate)) {
        throw new Exception("Parsing weeks failed"); 
      }
      return parsedDate;
    }

    private string GetTimetableUrl() { 
      return string.Format (timetableUrl, sessionId);
    }

  }
}

