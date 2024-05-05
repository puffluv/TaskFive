using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TaskFive
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                Id = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;

            SendMessage(" " + user.Name + " присоединился к чату. Добро пожаловать!", 0);
            users.Add(user);
            return user.Id;

        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage($" Нас покинул {user.Name}. Пока-пока!", 0);
            }
        }

        public void SendMessage(string message, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    answer += " - " + user.Name + ": ";
                }

                answer += message;

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MessageCallback(answer);
            }
        }
        
    }
}
