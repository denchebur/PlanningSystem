using System.Threading.Tasks;

namespace AutoReject
{
    public interface IAutoRejectService
    {
        Task AutoRejectStart();
    }
}