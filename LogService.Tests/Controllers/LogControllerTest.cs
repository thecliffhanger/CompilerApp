using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogService;
using LogService.Controllers;
using Entities;

namespace LogService.Tests.Controllers
{
    [TestClass]
    public class LogControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            LogController controller = new LogController();

            // Act
            IEnumerable<LogItem> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            LogController controller = new LogController();

            // Act
            LogItem result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            LogController controller = new LogController();

            // Act
            controller.Post(new LogItem());

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            LogController controller = new LogController();

            // Act
            controller.Put(5, new LogItem());

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            LogController controller = new LogController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
