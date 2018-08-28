using NUnit.Framework;
using System;
using System.Text;

namespace Belatrix.Console
{
    [TestFixture]
    class JobLoggerCustomTestCase
    {
        enum LogType { Message = 1, Warning, Error };

        [TestCase]
        public void SaveOnFile()
        {
            StringBuilder messageTest = new StringBuilder();
            messageTest.Append("MESSAGE FILE TEST # ");

            Random rnd = new Random();
            messageTest.Append(rnd.Next(1, 500).ToString());

            LogItem LI = LogFactory.getLogItem("FILE");
            LI.Mensaje = messageTest.ToString();
            LI.Tipo = (int)LogType.Error; //Para el caso se envia un error

            Assert.AreEqual("Ok", LI.SaveLog());
        }

        [TestCase]
        public void SaveOnDatabase()
        {
            StringBuilder messageTest = new StringBuilder();
            messageTest.Append("MESSAGE DB TEST # ");

            Random rnd = new Random();
            messageTest.Append(rnd.Next(1, 500).ToString());

            LogItem LI = LogFactory.getLogItem("BD");
            LI.Mensaje = messageTest.ToString();
            LI.Tipo = (int)LogType.Warning; //Para el caso se envia un warning

            Assert.AreEqual("Ok", LI.SaveLog());
        }
    }
}
