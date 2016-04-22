using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceOMG
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        
        [OperationContract]
        List<d> GetOdataCollectionByAuthByHttpExample(int posision);


        [OperationContract]
        void CreateBpmEntityByOdataHttpExample(string Name, string MobilePhone, string Dear, string JobTitle, DateTime BirthDate);

        [OperationContract]
        void UpdateExistingBpmEnyityByOdataHttpExample(string Id, string Name, string MobilePhone, string Dear, string JobTitle, DateTime BirthDate);

        [OperationContract]
        bool DeleteBpmEntityByOdataHttpExample(string Id);
        // TODO: Добавьте здесь операции служб
    }


    
}
