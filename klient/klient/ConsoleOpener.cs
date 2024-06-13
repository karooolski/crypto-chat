using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace klient
{
    internal class ConsoleOpener
    {
        public void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
