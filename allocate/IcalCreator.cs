using System;
using System.Collections.Generic;
using System.IO;

namespace allocate
{
  public class ICalCreator
  {
    private StreamWriter writer; 
    private List<ICalEvent> events; 
    public ICalCreator (string filePath)
    {
      writer = new StreamWriter (filePath);
    }

    public void CreateICal(List<ICalEvent> events) { 
      this.events = events; 
      CreateHeader(); 
      WriteEvents();
      CreateFooter();
      writer.Close();
    }

    private void CreateHeader() { 
      writer.WriteLine("BEGIN:VCALENDAR");
      writer.WriteLine("VERSION:2.0");
    }

    private void CreateFooter() { 
      writer.WriteLine("END:VCALENDAR");
      writer.WriteLine("#VCALENDAR");
    }

    private void WriteEvents() { 
      foreach (ICalEvent evnt in events) { 
        WriteEvent(evnt);
      }
    }

    private void WriteEvent(ICalEvent evnt) { 
      writer.WriteLine("BEGIN:VEVENT");
      writer.WriteLine("DTSTART;"+CreateIcalTime(evnt.StartDate, evnt.StartTime)); 
      writer.WriteLine("DTEND;"+CreateIcalTime(evnt.StartDate, evnt.EndTime));
      writer.WriteLine("RRULE:FREQ=WEEKLY;BYDAY="+ evnt.Day);
      writer.WriteLine("DESCRIPTION:"+evnt.Description); 
      writer.WriteLine("LOCATION:"+evnt.Location); 
      writer.WriteLine("SEQUENCE:0");
      writer.WriteLine("STATUS:CONFIRMED");
      writer.WriteLine("SUMMARY:"+evnt.Description); 
      writer.WriteLine("END:VEVENT");
    }

    private string CreateIcalTime(DateTime date, DateTime time) { 
      return string.Format("TZID=Australia/Sydney:{0}T{1}00",date.ToString("yyyyMMdd"), time.ToString("HHmm"));
    }



  }
}

