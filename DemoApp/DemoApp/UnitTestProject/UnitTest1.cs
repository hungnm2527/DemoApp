using DemoApp;
using DemoApp.Controllers;
using DemoApp.Models;
using DemoApp.Models.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestProject
{
    public class UnitTest1
    {
        protected readonly HttpClient _client;

        public UnitTest1()
        {
        }
        [Fact]
        public async Task Test1Async()
        {
            var controller = new OrdersController();

            // Act
            var result = await controller.Index("","",3,"");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderView>>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
        }
    }
}
