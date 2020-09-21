using System;
using System.Threading;
using Photos.Controllers;

namespace Photos
{

    class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                try
                {
                    //Log.Debug("Entra");
                    //new ConfigurationController().Configurar().Wait();
                    new ChangeWallpaperController().Start().Wait();
                    //new TesterController().Run().Wait();
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        Log.Error(e);
                    }
                }
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            Log.Info("Sale");
        }
    }
}
