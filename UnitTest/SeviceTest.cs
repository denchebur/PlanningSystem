using System;
using Contract;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino;
using Service.Providers;
using Service.Services;

namespace UnitTest
{
    [TestFixture]
    public class Tests
    {
        private PlanTaskService planTaskServiceTests;
        private IPlanTaskOperationProvider _operationProvider;
        [OneTimeSetUp]
        public void SetUp()
        {
            _operationProvider = MockRepository.Mock<IPlanTaskOperationProvider>();
        }
        [Test]
        public void ServiceGetTaskTest()
        {
            PlanTaskApi task = new PlanTaskApi()
            {
                Id = "1",
                Name = "TEST",
                Description = "TEST",
                Date = "1.01.2022 10:10:10",
                Priority = "Low",
                Status = "Completed"
            };
            //planTaskService.Expect(t => t.GetTask("1")).Return(task);
            
            //Assert.AreEqual(task, );
        }
        
    }
}