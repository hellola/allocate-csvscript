using System;
using System.Globalization;

namespace allocate
{
  public class ICalEvent
  {
    private string description; 
    public string Description { 
      get { 
        return description;
      }
      set { 
        description = value; 
        description = description.Replace(Environment.NewLine," ");
      }
    } 
    public DateTime StartTime; 
    public DateTime StartDate; 
    public DateTime EndTime;
    public string Location; 
    public string Weeks; 
    public string Day; 

    private int duration; 
    public int Duration {
      get { 
        return duration; 
      }
      set { 
        duration = value; 
        EndTime = StartTime.AddMinutes(duration); 
      }
    } 

    public ICalEvent ()
    {
    }
  }
}

