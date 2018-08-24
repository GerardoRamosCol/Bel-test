using NUnit.Framework;
using System;
using System.Text;

namespace Belatrix.Console
{
    [TestFixture]
    class JobLoggerCorrectedTestCase
    {
        [TestCase]
        public void SaveOnFile()
        {
            JobLoggerCorrected logger = new JobLoggerCorrected(true, false, false, true, false, false);
            StringBuilder messageTest = new StringBuilder();
            messageTest.Append("MESSAGE FILE TEST # ");

            Random rnd = new Random();
            messageTest.Append(rnd.Next(1,500).ToString());

            Assert.AreEqual("Ok", logger.LogMessage(messageTest.ToString()));
        }

        [TestCase]
        public void SaveOnDatabase()
        {
            JobLoggerCorrected logger = new JobLoggerCorrected(false, false, true, true, false, false);
            StringBuilder messageTest = new StringBuilder();
            messageTest.Append("MESSAGE DB TEST # ");

            Random rnd = new Random();
            messageTest.Append(rnd.Next(1, 500).ToString());

            Assert.AreEqual("Ok", logger.LogMessage(messageTest.ToString()));
        }
    }
}
