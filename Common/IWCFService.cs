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
        bool PokreniTimer(int key);

        [OperationContract]
        bool ZaustaviTimer(int key);

        [OperationContract]
        bool PonistiTimer(int key, Stopwatch st);

        [OperationContract]
        bool PostaviTimer(int key, Stopwatch st);

        [OperationContract]
        bool OcitajTimer(int key);

        [OperationContract]
        void ManagePermission(bool isAdd, string rolename, params string[] permissions);

        [OperationContract]
        void ManageRoles(bool isAdd, string rolename);

    }
}
