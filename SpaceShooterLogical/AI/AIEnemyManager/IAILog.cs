using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShip.Factory;
namespace SpaceShip.AI
{
    public interface IAILog
    {
        void RegisterAIShip(AIShipBase shipBase);

        void LogoutAIShip(AIShipBase shipBase);

        void RegisterEnviroment(EnviromentInBody enviromentInBody);

        void LogoutEnviroment(EnviromentInBody enviromentInBody);

        List<AIShipBase> getLeaderShips();

    }
}
