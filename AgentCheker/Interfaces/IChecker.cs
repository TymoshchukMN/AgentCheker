using System.Collections.Generic;
using AgentChecker.DataBase;

namespace AgentCheker.Interfaces
{
    public interface IChecker
    {
        void CheckPCs(
            List<PC> dcPCs,
            List<PC> esetPCs,
            ref List<PC> dcPingResult,
            ref List<PC> esetPingResult);
    }
}
