using System.Collections.Generic;
using AgentChecker.DataBase;

namespace AgentCheker.Interfaces
{
    public interface IChecker
    {
        void CheckPCs(
            List<PC> dcPCs,
            List<PC> esetPCs,
            List<PC> dcPingResult,
            List<PC> esetPingResult);
    }
}
