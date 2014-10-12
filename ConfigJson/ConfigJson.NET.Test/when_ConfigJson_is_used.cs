using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ConfigJsonNET.Test
{
    [TestClass]
    public class when_ConfigJson_is_used
    {
        // tell ConfigJson  the type of object you wish to use
        // todo: what to do about multiple specifications
        public class MyConfigurationObject : ConfigJson<SampleConfigClass>
        {
        }

        [TestMethod]
        public void it_should_have_the_following_characteristics()
        {
            const string expectedDefaultBaseDir = @"C:\Users\";
            //this is the default location where files are saved. you can change that
            // the app file is stored in application root directory, you cant change that. Or can you? yes you can
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.ConfigLocation, expectedDefaultBaseDir);

            // by default it doesn't run in memory
            Assert.IsFalse(MyConfigurationObject.Config.Advanced.RunInMemory);
            /* ******************>   #1  <**********************************/
            // lets store data in memory instead of file system for unit testing
            MyConfigurationObject.Config.Advanced.RunInMemory = true;

            //by default no overwrite is allowed
            Assert.IsFalse(MyConfigurationObject.Config.Advanced.AllowOverwrite);

            // by default, selection of config file is determined by the first active one rather than selected based on a selector
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.Selector, null);

            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.FileName, @"debug.config.ConfigJsonNET.Test.SampleConfigClass.json");
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.IsActive, true);
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.Selector, "debug");
            /* ******************>   #2  <**********************************/
            // lets select the second listing instead. this will override the selection based on what is active
            MyConfigurationObject.Config.Advanced.Selector = "release";

            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.FileName, @"release.config.ConfigJsonNET.Test.SampleConfigClass.json");
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.IsActive, false);
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.CurrentSelection.Selector, "release");

            var stringPropertyScenario = MyConfigurationObject.Data.SampleStringProperty;
            var classPropertyScenario = MyConfigurationObject.Data.SampleClassProperty;
            var listOfClassPropertyScenario = MyConfigurationObject.Data.SampleListOfClassProperty;

            // no files exist  so it will create and assign default values
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            // an attempt to write to the file will not work
            MyConfigurationObject.Config.Advanced.PersistedData = _configObj;

            stringPropertyScenario = MyConfigurationObject.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Data.SampleListOfClassProperty;
            // no reaction
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);
            /* ******************>   #3  <**********************************/
            // lets turn on overwrite
            MyConfigurationObject.Config.Advanced.AllowOverwrite = true;

            stringPropertyScenario = MyConfigurationObject.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Data.SampleListOfClassProperty;

            // we still get default because we have not written to the file
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            /* ******************>   #4  <**********************************/
            // lets write to the file
            MyConfigurationObject.Config.Advanced.PersistedData = _configObj;

            stringPropertyScenario = MyConfigurationObject.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Data.SampleListOfClassProperty;

            // now we get what we wrote back
            Assert.AreEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);

            // changing the original object does not propagate to the files
            _configObj.SampleStringProperty = "1";
            _configObj.SampleClassProperty.SomeProperty = "1";
            _configObj.SampleListOfClassProperty.First().SomeProperty = "1";

            // so the effect is inequality
            Assert.AreNotEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);

            // overwrite is still on
            Assert.IsTrue(MyConfigurationObject.Config.Advanced.AllowOverwrite);

            // so lets overwrite with updated value
            MyConfigurationObject.Config.Advanced.PersistedData = _configObj;
            // still no reaction because we have not reloaded the data
            Assert.AreNotEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);

            // lets reload
            stringPropertyScenario = MyConfigurationObject.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Data.SampleListOfClassProperty;

            // now we have a reaction
            Assert.AreEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);

            // lets turn off overwrite
            MyConfigurationObject.Config.Advanced.AllowOverwrite = false;

            // again changing the original object does not propagate to the files
            _configObj.SampleStringProperty = "2";
            _configObj.SampleClassProperty.SomeProperty = "2";
            _configObj.SampleListOfClassProperty.First().SomeProperty = "2";

            // so the effect is inequality
            Assert.AreNotEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);

            // so lets overwrite with updated value
            MyConfigurationObject.Config.Advanced.PersistedData = _configObj;
            // for a different reason, still no reaction
            Assert.AreNotEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);

            // even if we reload
            stringPropertyScenario = MyConfigurationObject.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Data.SampleListOfClassProperty;

            // there will still be no reaction because we have turned off overwrite
            Assert.AreNotEqual(stringPropertyScenario, _configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj.SampleListOfClassProperty.First().SomeProperty);
        }

        private readonly SampleConfigClass _configObj = new SampleConfigClass()
        {
            SampleClassProperty = new SomeClass()
            {
                SomeProperty = "me"
            },
            SampleListOfClassProperty = new List<SomeClass>() {
                               new SomeClass()
                           {
                               SomeProperty = "hey"
                           }
            },
            SampleStringProperty = "hello"
        };
    }
}