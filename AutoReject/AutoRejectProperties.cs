using System.Configuration;

namespace AutoReject
{
    public class AutoRejectProperties
    {
        public int DataReject { get; }
        public AutoRejectProperties()
        {
            var config = ConfigurationManager.AppSettings;
            var res = config["DateReject"];
            DataReject = int.Parse(res);
        }
    }
}