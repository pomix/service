using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Services.Client;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Collections;
using System.Xml;

namespace WcfServiceOMG
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {
        private const string serverUri = "http://178.159.246.209:1410/0/ServiceModel/EntityDataService.svc/";
        private const string authServiceUtri = "http://178.159.246.209:1410/ServiceModel/AuthService.svc/Login";

        // Ссылки на пространства имен XML.
        private static readonly XNamespace ds = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static readonly XNamespace dsmd = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        private static readonly XNamespace atom = "http://www.w3.org/2005/Atom";
        // Cookie аутентификации bpm'online.

        public void CreateBpmEntityByOdataHttpExample(string Name, string MobilePhone, string Dear, string JobTitle, DateTime BirthDate)
        {
            // Создание сообщения xml, содержащего данные о создаваемом объекте.
            var content = new XElement(dsmd + "properties",
                           new XElement(ds + "Name", Name),
                           new XElement(ds + "Dear", Dear),
                           new XElement(ds + "MobilePhone", MobilePhone),
                           new XElement(ds + "JobTitle", JobTitle),
                           new XElement(ds + "BirthDate", BirthDate)
                           );
            var entry = new XElement(atom + "entry",
                        new XElement(atom + "content",
                        new XAttribute("type", "application/xml"), content));
            Console.WriteLine(entry.ToString());
            // Создание запроса к сервису, который будет добавлять новый объект в коллекцию контактов.
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri + "ContactCollection/");
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            request.Method = "POST";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            // Запись xml-сообщения в поток запроса.
            using (var writer = XmlWriter.Create(request.GetRequestStream()))
            {
                entry.WriteTo(writer);
            }
            // Получение ответа от сервиса о результате выполнения операции.
            using (WebResponse response = request.GetResponse())
            {
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Created)
                {
                   
                }
            }
        }
        public List<d> GetOdataCollectionByAuthByHttpExample(int position)
        {
            // Создание запроса на аутентификацию.

            // Получение ответа от сервера. Если аутентификация проходит успешно, в объекте bpmCookieContainer будут 
            // помещены cookie, которые могут быть использованы для последующих запросов.

            // Создание запроса на получение данных от сервиса OData.
            var dataRequest = HttpWebRequest.Create(serverUri + "ContactCollection?$skip=" + position + "&$orderby=Name desc")
                                        as HttpWebRequest;
            // Для получения данных используется HTTP-метод GET.
            dataRequest.Method = "GET";
            // Добавление полученных ранее аутентификационных cookie в запрос на получение данных.
            dataRequest.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            // Получение ответа от сервера.
            using (var dataResponse = (HttpWebResponse)dataRequest.GetResponse())
            {
                // Загрузка ответа сервера в xml-документ для дальнейшей обработки.
                XDocument xmlDoc = XDocument.Load(dataResponse.GetResponseStream());
                // Получение коллекции объектов контактов, соответствующих условию запроса.
                var contacts = from entry in xmlDoc.Descendants(atom + "entry")
                               select new
                               {
                                   Id = new Guid(entry.Element(atom + "content")
                                              .Element(dsmd + "properties")
                                              .Element(ds + "Id").Value),
                                   Name = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "Name").Value,
                                   MobilePhone = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "MobilePhone").Value,
                                   Dear = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "Dear").Value,
                                   JobTitle = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "JobTitle").Value,
                                   BirthDate = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "BirthDate").Value
                               };
                List<d> _list = new List<d>();
                foreach (var contact in contacts)
                {
                    _list.Add(new d
                    {
                        Name = contact.Name,
                        Id = contact.Id,
                        MobilePhone = contact.MobilePhone,
                        Dear = contact.Dear,
                        JobTitle = contact.JobTitle,
                        BirthDate = Convert.ToDateTime(contact.BirthDate).ToString("d")
                    });
                }
                return _list;
            }
        }
        public void UpdateExistingBpmEnyityByOdataHttpExample(string Id, string Name, string MobilePhone, string Dear, string JobTitle, DateTime BirthDate)
        {
            
            // Создание сообщения xml, содержащего данные об изменяемом объекте.
            var content = new XElement(dsmd + "properties",
                          new XElement(ds + "Name", Name),
                          new XElement(ds + "Dear", Dear),
                          new XElement(ds + "MobilePhone", MobilePhone),
                          new XElement(ds + "JobTitle", JobTitle),
                          new XElement(ds + "BirthDate", BirthDate)
                          );
            var entry = new XElement(atom + "entry",
                    new XElement(atom + "content",
                            new XAttribute("type", "application/xml"),
                            content)
                    );
            // Создание запроса к сервису, который будет изменять данные объекта.
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri
                    + "ContactCollection(guid'" + Id + "')");
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            request.Method = "PUT";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            // Запись сообщения xml в поток запроса.
            using (var writer = XmlWriter.Create(request.GetRequestStream()))
            {
                entry.WriteTo(writer);
            }
            // Получение ответа от сервиса о результате выполнения операции.
            using (WebResponse response = request.GetResponse())
            {
                // Обработка результата выполнения операции.
            }
        }

        public bool DeleteBpmEntityByOdataHttpExample(string Id)
        {
            
            // Создание запроса к сервису, который будет удалять данные.
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri
                    + "ContactCollection(guid'" + Id + "')");
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            request.Method = "DELETE";
            // Получение ответа от сервися о результате выполненя операции.
            try
            {
                WebResponse response = request.GetResponse();
                return true;
            }
            catch(WebException ex)
            {
                return false;
            }
            
        }
    }
   public class d
    {
        public Guid Id {get;set;}
        public string Name {get;set;}
        public string MobilePhone {get;set;}
        public string Dear {get;set;}
        public string JobTitle {get;set;}
        public string BirthDate {get;set;}
    }
}
