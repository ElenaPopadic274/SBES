using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IWCFService
    {

        [OperationContract]
        bool PokreniTimer();

        [OperationContract]
        bool ZaustaviTimer();

        [OperationContract]
        bool PonistiTimer();

        [OperationContract]
        bool PostaviTimer(byte[] CipheredTimerMax);

        [OperationContract]
        string OcitajTimer();

        [OperationContract]
        bool ManagePermission(bool isAdd, string rolename, params string[] permissions);

        [OperationContract]
        bool ManageRoles(bool isAdd, string rolename);

    }
}
