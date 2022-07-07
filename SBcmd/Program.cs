using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SBcmd
{
  internal class Program
  {
    static void Main(string[] args)
    {
      // Log Any Commands
      Console.WriteLine(args);
      
      if (args.Length < 1) return;

      string cmd = args[0];
      switch (cmd)
      {
        case "addpath":
          AddPath();
          break;
        case "list":
          List();
          break;
        default:
          Console.WriteLine("Unknown command: " + cmd);
          break;
      }
    }

    private static void List()
    {
      
    }

    public static bool IsAdministrator()
    {
      WindowsIdentity identity = WindowsIdentity.GetCurrent();
      WindowsPrincipal principal = new WindowsPrincipal(identity);
      return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void AddPath()
    {
      // run as Admin
      if (!Program.IsAdministrator())
      {
        // Restart and run as admin
        var exeName = Process.GetCurrentProcess().MainModule.FileName;
        ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
        startInfo.Verb = "runas";
        startInfo.Arguments = "addpath";
        Process.Start(startInfo);
        Console.WriteLine("Set as Admin");
        return;
      }

      var scope = EnvironmentVariableTarget.Machine; // or User
      string oldValue = Environment.GetEnvironmentVariable("PATH", scope);

      // Get this pogram path
      var programPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
      var programDir = System.IO.Path.GetDirectoryName(programPath);

      string newValue = oldValue + ";" + programDir;

      if (oldValue.Contains(programDir))
      {
        Console.WriteLine("Already added");
        return;
      }
      
      Environment.SetEnvironmentVariable("PATH", newValue, scope);
      Console.WriteLine("Path added");
    }
    
    
  }
}
