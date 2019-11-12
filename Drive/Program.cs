using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Controllers.DriveController();
            int i = 0;
            while (i < 3)
            {
                c.SubirDB();
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));
                i++;
            }
        }
    }
}
