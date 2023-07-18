using System.Collections.Generic;
using AgentChecker.DataBase;

namespace AgentCheker.Interfaces
{
    public interface IChecker
    {
        List<PC> CheckPCs(List<PC> dcPCs, List<PC> esetPCs);
    }
}
