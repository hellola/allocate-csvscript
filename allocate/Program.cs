using System;
using CommandLine;

namespace allocate
{
  public class ScraperOptions { 
    [Option('u',"username",Required=true, HelpText = "Your SIMS ID")]
    public string username { get; set;} 
    [Option('p',"password", Required=true, HelpText="Your SIMS password")]
    public string password { get; set; } 

	[HelpOption]
	public string GetUsage()  { 
	  return CommandLine.Text.HelpText.AutoBuild(this);
	}

    public ScraperOptions() { 
    }
  }
  class MainClass
  {
    public static void Main (string[] args)
    {
      ScraperOptions options = new ScraperOptions(); 
      if (CommandLine.Parser.Default.ParseArguments(args,options)) { 
        AllocateScraper scraper = new AllocateScraper (options);
        Console.WriteLine("Logging in");
        scraper.LogIn(); 
        Console.WriteLine("Getting your timetable");
        scraper.GetTimeTable();
        ICalCreator creator = new ICalCreator ("calendar.ical");
        Console.WriteLine("Saving your timetable");
        creator.CreateICal(scraper.GetEvents());
        Console.WriteLine("Processed Successfully!");
      }
    }
  }
}
