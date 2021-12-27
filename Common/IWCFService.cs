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
        void AddUser(string username, string password);

        [OperationContract]
        bool PokreniTimer();

        [OperationContract]
        bool ZaustaviTimer();

        [OperationContract]
        bool PonistiTimer();

        [OperationContract]
        bool PostaviTimer();

        [OperationContract]
        bool OcitajTimer();

        [OperationContract]
        void ManagePermission(bool isAdd, string rolename, params string[] permissions);

        [OperationContract]
        void ManageRoles(bool isAdd, string rolename);

    }
}
