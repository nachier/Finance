using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finance.Web.ViewModels;
//using RabbitMQ.Client;

namespace Finance.Web.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(MessageViewModel model)
        {
            ViewBag.message = SendMessageToRabbitMQ("192.168.0.222", "admin", "admin", model.Content);
            return View();
        }


        private string SendMessageToRabbitMQ(string host, string user, string pass, string message)
        {
            string result = "发送成功";
            //try
            //{
            //    var factory = new ConnectionFactory
            //    {
            //        HostName = host,
            //        UserName = user,
            //        Password = pass,
            //        Port = 5672,
            //        AutomaticRecoveryEnabled = true,
            //        TopologyRecoveryEnabled = true
            //    };
            //    using (IConnection connection = factory.CreateConnection())
            //    {
            //        using (var model = connection.CreateModel())
            //        {
            //            model.ExchangeDeclare("hs", "topic", durable: true, autoDelete: false, arguments: null);
            //            model.QueueDeclare("queue_45142132", durable: true, autoDelete: false, exclusive: false, arguments: null); //持久化队列                
            //            model.QueueBind("queue_45142132", "hs", "45142132", null);
            //            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message);
            //            IBasicProperties props = model.CreateBasicProperties();
            //            props.ContentType = "text/plain";
            //            props.DeliveryMode = 2;
            //            model.BasicPublish("hs", "45142132", props, messageBodyBytes);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = "发送失败" + ex.Message;
            //}
            return result;
        }


    }
}