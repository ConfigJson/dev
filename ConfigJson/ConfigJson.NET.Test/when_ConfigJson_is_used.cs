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

        [TestMethod]
        public void it_should_have_the_following_characteristics()
        {
            const string expectedDefaultBaseDir = @"C:\Users\";
            //this is the default location where files are saved. you can change that
            // the app file is stored in application root directory, you cant change that. Or can you? yes you can
            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.ConfigLocation, expectedDefaultBaseDir);

            // by default it doesn't run in memory
            Assert.IsFalse(MyConfigurationObject1.Config.Advanced.RunInMemory);
            /* ******************>   #1  <**********************************/
            // lets store data in memory instead of file system for unit testing

            MyConfigurationObject1.Config.Advanced.RunInMemory = true;

            // even though one configuration is set to run on memory, others are not
            Assert.IsTrue(MyConfigurationObject1.Config.Advanced.RunInMemory);
            Assert.IsFalse(MyConfigurationObject2.Config.Advanced.RunInMemory);
            Assert.IsFalse(MyConfigurationObject3.Config.Advanced.RunInMemory);

            MyConfigurationObject2.Config.Advanced.RunInMemory = true;
            MyConfigurationObject3.Config.Advanced.RunInMemory = true;

            Assert.IsTrue(MyConfigurationObject1.Config.Advanced.RunInMemory);
            Assert.IsTrue(MyConfigurationObject2.Config.Advanced.RunInMemory);
            Assert.IsTrue(MyConfigurationObject3.Config.Advanced.RunInMemory);

            //by default no overwrite is allowed
            Assert.IsFalse(MyConfigurationObject1.Config.Advanced.AllowOverwrite);
            Assert.IsFalse(MyConfigurationObject2.Config.Advanced.AllowOverwrite);
            Assert.IsFalse(MyConfigurationObject3.Config.Advanced.AllowOverwrite);

            MyConfigurationObject1.Config.Advanced.AllowOverwrite = true;
            MyConfigurationObject2.Config.Advanced.AllowOverwrite = true;
            MyConfigurationObject3.Config.Advanced.AllowOverwrite = true;

            // by default, selection of config file is determined by the first active one rather than selected based on a selector
            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.Selector, null);
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.Selector, null);
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.Selector, null);

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.FileName, @"debug.config.ConfigJsonNET.Test.SampleConfigClass1.json");
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.FileName, @"debug.config.ConfigJsonNET.Test.SampleConfigClass2.json");
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.FileName, @"debug.config.ConfigJsonNET.Test.SampleConfigClass3.json");

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.SetUpFile, @"app.config.ConfigJsonNET.Test.SampleConfigClass1.json");
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.SetUpFile, @"app.config.ConfigJsonNET.Test.SampleConfigClass2.json");
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.SetUpFile, @"app.config.ConfigJsonNET.Test.SampleConfigClass3.json");

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.IsActive, true);
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.IsActive, true);
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.IsActive, true);

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.Selector, "debug");
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.Selector, "debug");
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.Selector, "debug");

            /* ******************>   #2  <**********************************/
            // lets select the second listing instead. this will override the selection based on what is active
            MyConfigurationObject1.Config.Advanced.Selector = "release";
            MyConfigurationObject3.Config.Advanced.Selector = "release";

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.FileName, @"release.config.ConfigJsonNET.Test.SampleConfigClass1.json");
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.FileName, @"debug.config.ConfigJsonNET.Test.SampleConfigClass2.json");
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.FileName, @"release.config.ConfigJsonNET.Test.SampleConfigClass3.json");

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.BaseDir, expectedDefaultBaseDir);

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.IsActive, false);
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.IsActive, true);
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.IsActive, false);

            Assert.AreEqual(MyConfigurationObject1.Config.Advanced.CurrentSelection.Selector, "release");
            Assert.AreEqual(MyConfigurationObject2.Config.Advanced.CurrentSelection.Selector, "debug");
            Assert.AreEqual(MyConfigurationObject3.Config.Advanced.CurrentSelection.Selector, "release");

            var stringPropertyScenario = MyConfigurationObject1.Data.SampleStringProperty;
            var classPropertyScenario = MyConfigurationObject1.Data.SampleClassProperty;
            var listOfClassPropertyScenario = MyConfigurationObject1.Data.SampleListOfClassProperty;

            // no files exist  so it will create and assign default values
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            MyConfigurationObject1.Config.Advanced.AllowOverwrite = false;
            MyConfigurationObject2.Config.Advanced.AllowOverwrite = false;
            MyConfigurationObject3.Config.Advanced.AllowOverwrite = false;

            // an attempt to write to the file will not work
            MyConfigurationObject1.Config.Advanced.PersistedData = _configObj1;

            stringPropertyScenario = MyConfigurationObject1.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject1.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject1.Data.SampleListOfClassProperty;
            // no reaction
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);
            /* ******************>   #3  <**********************************/
            // lets turn on overwrite
            MyConfigurationObject1.Config.Advanced.AllowOverwrite = true;

            stringPropertyScenario = MyConfigurationObject1.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject1.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject1.Data.SampleListOfClassProperty;

            // we still get default because we have not written to the file
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            /* ******************>   #4  <**********************************/
            // lets write to the file
            MyConfigurationObject1.Config.Advanced.PersistedData = _configObj1;
            MyConfigurationObject3.Config.Advanced.PersistedData = _configObj3;

            stringPropertyScenario = MyConfigurationObject1.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject1.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject1.Data.SampleListOfClassProperty;

            // now we get what we wrote back
            Assert.AreEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);

            // but not with the other configurations
            // we still get default because we have not written to the file
            Assert.AreEqual(MyConfigurationObject2.Data, null);

            Assert.AreEqual(MyConfigurationObject3.Data, null);

            // changing the original object does not propagate to the files
            _configObj1.SampleStringProperty = "1";
            _configObj1.SampleClassProperty.SomeProperty = "1";
            _configObj1.SampleListOfClassProperty.First().SomeProperty = "1";

            // so the effect is inequality
            Assert.AreNotEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);

            // overwrite is still on
            Assert.IsTrue(MyConfigurationObject1.Config.Advanced.AllowOverwrite);

            // so lets overwrite with updated value
            MyConfigurationObject1.Config.Advanced.PersistedData = _configObj1;
            // still no reaction because we have not reloaded the data
            Assert.AreNotEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);

            // lets reload
            stringPropertyScenario = MyConfigurationObject1.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject1.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject1.Data.SampleListOfClassProperty;

            // now we have a reaction
            Assert.AreEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);

            // lets turn off overwrite
            MyConfigurationObject1.Config.Advanced.AllowOverwrite = false;

            // again changing the original object does not propagate to the files
            _configObj1.SampleStringProperty = "2";
            _configObj1.SampleClassProperty.SomeProperty = "2";
            _configObj1.SampleListOfClassProperty.First().SomeProperty = "2";

            // so the effect is inequality
            Assert.AreNotEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);

            // so lets overwrite with updated value
            MyConfigurationObject1.Config.Advanced.PersistedData = _configObj1;
            // for a different reason, still no reaction
            Assert.AreNotEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);

            // even if we reload
            stringPropertyScenario = MyConfigurationObject1.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject1.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject1.Data.SampleListOfClassProperty;

            // there will still be no reaction because we have turned off overwrite
            Assert.AreNotEqual(stringPropertyScenario, _configObj1.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, _configObj1.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, _configObj1.SampleListOfClassProperty.First().SomeProperty);
        }

        private readonly SampleConfigClass1 _configObj1 = new SampleConfigClass1()
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

        private readonly SampleConfigClass2 _configObj2 = new SampleConfigClass2()
        {
            SampleClassProperty = new SomeClass()
            {
                SomeProperty = "me2"
            },
            SampleListOfClassProperty = new List<SomeClass>() {
                               new SomeClass()
                           {
                               SomeProperty = "hey2"
                           }
            },
            SampleStringProperty = "hello2"
        };

        private readonly SampleConfigClass3 _configObj3 = new SampleConfigClass3()
        {
            SampleClassProperty = new SomeClass()
            {
                SomeProperty = "me3"
            },
            SampleListOfClassProperty = new List<SomeClass>() {
                               new SomeClass()
                           {
                               SomeProperty = "hey3"
                           }
            },
            SampleStringProperty = "hello3"
        };
    }
}