using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            //this is the default location where files are saved. you can change that
            // the app file is stored in application root directory, you cant change that. Or can you? yes you can
            Assert.AreEqual(MyConfigurationObject.Config.Advanced.ConfigLocation, @"C:\Users\");

            // by default it doesnt run in memory
            Assert.IsFalse(MyConfigurationObject.Config.Advanced.RunInMemory);

            // lets store data in memory instead of file system for unit testing
            MyConfigurationObject.Config.Advanced.RunInMemory = true;

            //by default no overwrite is allowed
            Assert.IsFalse(MyConfigurationObject.Config.Advanced.AllowOverwrite);

            var stringPropertyScenario = MyConfigurationObject.Config.Data.SampleStringProperty;
            var classPropertyScenario = MyConfigurationObject.Config.Data.SampleClassProperty;
            var listOfClassPropertyScenario = MyConfigurationObject.Config.Data.SampleListOfClassProperty;

            // no files exist  so it will create and assign default values
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            // an attempt to write to the file will not work
            MyConfigurationObject.Config.Advanced.PersistedData = configObj;

            stringPropertyScenario = MyConfigurationObject.Config.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Config.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Config.Data.SampleListOfClassProperty;
            // no reaction
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            // lets turn on overwrite
            MyConfigurationObject.Config.Advanced.AllowOverwrite = true;

            stringPropertyScenario = MyConfigurationObject.Config.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Config.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Config.Data.SampleListOfClassProperty;

            // we still get dafault because we have not written to the file
            Assert.AreEqual(stringPropertyScenario, null);
            Assert.AreEqual(classPropertyScenario.SomeProperty, null);
            Assert.AreEqual(listOfClassPropertyScenario.Count, 0);

            // lets write to the file
            MyConfigurationObject.Config.Advanced.PersistedData = configObj;

            stringPropertyScenario = MyConfigurationObject.Config.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Config.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Config.Data.SampleListOfClassProperty;

            // now we get what we wrote back
            Assert.AreEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);

            // changing the original object does not propagate to the files
            configObj.SampleStringProperty = "1";
            configObj.SampleClassProperty.SomeProperty = "1";
            configObj.SampleListOfClassProperty.First().SomeProperty = "1";

            // so the effect is inequality
            Assert.AreNotEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);

            // overwrite is still on
            Assert.IsTrue(MyConfigurationObject.Config.Advanced.AllowOverwrite);

            // so lets overwrite with updated value
            MyConfigurationObject.Config.Advanced.PersistedData = configObj;
            // still no reaction because we have not reloaded the data
            Assert.AreNotEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);

            // lets reload
            stringPropertyScenario = MyConfigurationObject.Config.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Config.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Config.Data.SampleListOfClassProperty;

            // now we have a reaction
            Assert.AreEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);

            // lets turn off overwrite
            MyConfigurationObject.Config.Advanced.AllowOverwrite = false;

            // again changing the original object does not propagate to the files
            configObj.SampleStringProperty = "2";
            configObj.SampleClassProperty.SomeProperty = "2";
            configObj.SampleListOfClassProperty.First().SomeProperty = "2";

            // so the effect is inequality
            Assert.AreNotEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);

            // so lets overwrite with updated value
            MyConfigurationObject.Config.Advanced.PersistedData = configObj;
            // for a different reason, still no reaction
            Assert.AreNotEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);

            // even if we reload
            stringPropertyScenario = MyConfigurationObject.Config.Data.SampleStringProperty;
            classPropertyScenario = MyConfigurationObject.Config.Data.SampleClassProperty;
            listOfClassPropertyScenario = MyConfigurationObject.Config.Data.SampleListOfClassProperty;

            // there will still be no reaction because we have turned off overwrite
            Assert.AreNotEqual(stringPropertyScenario, configObj.SampleStringProperty);
            Assert.AreNotEqual(classPropertyScenario.SomeProperty, configObj.SampleClassProperty.SomeProperty);
            Assert.AreNotEqual(listOfClassPropertyScenario.First().SomeProperty, configObj.SampleListOfClassProperty.First().SomeProperty);
        }


        public class SomeClass
        {
            public string SomeProperty { set; get; }
        }

        public class SampleConfigClass
        {
            public SampleConfigClass()
            {
                SampleClassProperty = new SomeClass();
                SampleListOfClassProperty = new List<SomeClass>();
            }

            public string SampleStringProperty { set; get; }

            public SomeClass SampleClassProperty { set; get; }

            public List<SomeClass> SampleListOfClassProperty { set; get; }
        }

        private SampleConfigClass configObj = new SampleConfigClass()
        {
            SampleClassProperty = new SomeClass()
            {
                SomeProperty = "me"
            },
            SampleListOfClassProperty = new List<SomeClass>() {
                               new SomeClass()
                           {
                               SomeProperty = "hey"
                           } },
            SampleStringProperty = "hello"
        };

    }
}