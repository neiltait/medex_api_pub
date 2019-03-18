using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;
using Xunit.Abstractions;

namespace MedicalExaminer.API.Tests
{
    /// <summary>
    ///     Validate Route Variable Tests
    /// </summary>
    public class ValidateRouteVariablesTests
    {
        /// <summary>
        ///     Initialise the test.
        /// </summary>
        /// <param name="testOutputHelper">Output helper for writing output to the tests.</param>
        public ValidateRouteVariablesTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        /// <summary>
        ///     Get all controllers from the domain.
        /// </summary>
        /// <returns>A List of types matching our controller class</returns>
        private IEnumerable<Type> AllControllers()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();

            foreach (var assembly in assemblies)
                if (!assembly.FullName.StartsWith("Microsoft"))
                    try
                    {
                        var typesInAssembly = assembly.GetTypes()
                            .Where(myType =>
                                myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Controller)));

                        types.AddRange(typesInAssembly);
                    }
                    catch
                    {
                        _testOutputHelper.WriteLine("Unable to get types from: " + assembly.FullName);
                    }

            return types;
        }

        /// <summary>
        ///     Validate All Route Variables Match The Method Parameters
        /// </summary>
        [Fact]
        public void ValidateAllRouteVariablesMatchTheMethodParameters()
        {
            var controllers = AllControllers();
            var allValid = true;

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(HttpMethodAttribute), true).Length > 0);

                var controllerHttpAttributes = controller.GetCustomAttributes(typeof(HttpMethodAttribute), true);
                var controllerRouteAttributes = controller.GetCustomAttributes(typeof(RouteAttribute), true);

                foreach (var action in actions)
                {
                    var parameters = action.GetParameters();

                    foreach (var parameter in parameters)
                        if (parameter.GetCustomAttributes(typeof(FromBodyAttribute), false).Length == 0)
                        {
                            var foundInAttribute = false;
                            var actionHttpAttributes = action.GetCustomAttributes(typeof(HttpMethodAttribute), true);
                            var actionRouteAttributes = action.GetCustomAttributes(typeof(RouteAttribute), true);

                            var httpAttributes = actionHttpAttributes.Concat(controllerHttpAttributes);
                            var routeAttributes = actionRouteAttributes.Concat(controllerRouteAttributes);

                            foreach (HttpMethodAttribute httpAttribute in httpAttributes)
                                if (httpAttribute.Template?.Contains($"{{{parameter.Name}") == true)
                                    foundInAttribute = true;

                            foreach (RouteAttribute routeAttribute in routeAttributes)
                                if (routeAttribute.Template?.Contains($"{{{parameter.Name}") == true)
                                    foundInAttribute = true;

                            if (!foundInAttribute)
                            {
                                _testOutputHelper.WriteLine(
                                    $"Failed to find {parameter.Name} in http attributes for Action:{action.Name} in Controller: {controller.Name} or missing [FromBody] attribute.");
                                allValid = false;
                            }
                        }
                }
            }

            Assert.True(allValid);
        }
    }
}