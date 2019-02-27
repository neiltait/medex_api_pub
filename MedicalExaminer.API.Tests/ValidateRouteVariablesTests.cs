﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Xunit;
using Xunit.Abstractions;

namespace MedicalExaminer.API.Tests
{
    /// <summary>
    /// Validate Route Variable Tests
    /// </summary>
    public class ValidateRouteVariablesTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        /// <summary>
        /// Get all controllers from the domain.
        /// </summary>
        /// <returns>A List of types matching our controller class</returns>
        private IEnumerable<Type> AllControllers()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                if (!assembly.FullName.StartsWith("Microsoft")){
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
                }
            }

            return types;
        }

        /// <summary>
        /// Initialise the test.
        /// </summary>
        /// <param name="testOutputHelper">Output helper for writing output to the tests.</param>
        public ValidateRouteVariablesTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Validate All Route Variables Match The Method Parameters
        /// </summary>
        [Fact]
        public void ValidateAllRouteVariablesMatchTheMethodParameters()
        {
            var controllers = AllControllers();
            var allValid = true;

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods().Where(m => m.GetCustomAttributes(typeof(HttpMethodAttribute), true).Length > 0);

                foreach (var action in actions)
                {
                    var parameters = action.GetParameters();

                    foreach (var parameter in parameters)
                    {
                        var foundInAttribute = false;
                        var httpAttributes = action.GetCustomAttributes(typeof(HttpMethodAttribute), true);

                        foreach (HttpMethodAttribute httpAttribute in httpAttributes)
                        {
                            if (httpAttribute.Template?.Contains($"{{{parameter.Name}") == true)
                            {
                                foundInAttribute = true;
                            }
                        }

                        if (!foundInAttribute)
                        {
                            _testOutputHelper.WriteLine($"Failed to find {parameter.Name} in http attributes for Action:{action.Name} in Controller: {controller.Name}");
                            allValid = false;
                        }
                    }
                }
            }

            Assert.True(allValid);
        }
    }
}
