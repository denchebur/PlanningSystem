using System;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using NLog.Config;

namespace Host
{
    class Program
    {       
         
        static void Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            NLog.LogManager.Configuration = config;
            Console.OutputEncoding = Encoding.UTF8;
            var ctx = new ApplicationContext();
            try
            {
                var serviceHost = ctx.ServiceHost;
                serviceHost.Open();
                Console.WriteLine(serviceHost.BaseAddresses.FirstOrDefault());                   
                Console.WriteLine(serviceHost.State.ToString());
                ctx.AutoRejectService.AutoRejectStart();
                //Task.Factory.StartNew(() => ctx.AutoRejectService.AutoReject());
                Console.ReadLine();
                serviceHost.Close();
            }
            catch (TimeoutException timeProblem)
            {
                    
                Console.WriteLine(timeProblem.ToString());
                Console.ReadLine();
            }
            catch (CommunicationException commProblem)
            {
                    
                Console.WriteLine(commProblem.ToString());
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                    
                Console.ReadLine();
            }
            
        }
    }
}