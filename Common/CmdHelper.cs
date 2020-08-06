using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CmdHelper
    {
        public static void Execute(string path, string command)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WorkingDirectory = path;
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = command;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
