using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace NetworkGraph.Models
{
    public class ConsoleCommand
    {

        public String input {get; set;}
        public String output { get; set;}


        public ConsoleCommand()
        {
            input = "dir \"C:\\\"";
            output = "output";
        }  
      
        public void ExecuteCommand(String input)
        {

            output = "";
                        
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c" + " " + input);
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            // The following commands are needed to redirect the standard output.
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            // Get the output into a string
            string result;
            try
            {
                while ((result = proc.StandardOutput.ReadLine()) != null)
                {
                    output += result + "\r\n";
                }
            } // here I expect it to update the text box line by line in real time
            // but it does not.
            catch (Exception e)
            {
                output = e.Message;
            }


        }




    }
}