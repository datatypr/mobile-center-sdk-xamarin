﻿using NUnit.Framework;
using Xamarin.UITest;

namespace Contoso.Forms.Test.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void TestEnablingAndDisablingServices()
        {
            ServiceStateHelper.app = app;
            app.Tap(TestStrings.GoToTogglePageButton);

            /* Test setting enabling all services */
            ServiceStateHelper.MobileCenterEnabled = true;
            Assert.IsTrue(ServiceStateHelper.MobileCenterEnabled);
            ServiceStateHelper.AnalyticsEnabled = true;
            Assert.IsTrue(ServiceStateHelper.AnalyticsEnabled);
            ServiceStateHelper.CrashesEnabled = true;
            Assert.IsTrue(ServiceStateHelper.CrashesEnabled);

            /* Test that disabling MobileCenter disables everything */
            ServiceStateHelper.MobileCenterEnabled = false;
            Assert.IsFalse(ServiceStateHelper.MobileCenterEnabled);
            Assert.IsFalse(ServiceStateHelper.AnalyticsEnabled);
            Assert.IsFalse(ServiceStateHelper.CrashesEnabled);

            /* Test disabling individual services */
            ServiceStateHelper.MobileCenterEnabled = true;
            Assert.IsTrue(ServiceStateHelper.MobileCenterEnabled);
            ServiceStateHelper.AnalyticsEnabled = false;
            Assert.IsFalse(ServiceStateHelper.AnalyticsEnabled);
            ServiceStateHelper.CrashesEnabled = false;
            Assert.IsFalse(ServiceStateHelper.CrashesEnabled);

            /* Test that enabling MobileCenter enabling everything, regardless of previous states */
            ServiceStateHelper.MobileCenterEnabled = true;
            Assert.IsTrue(ServiceStateHelper.MobileCenterEnabled);
            Assert.IsTrue(ServiceStateHelper.AnalyticsEnabled);
            Assert.IsTrue(ServiceStateHelper.CrashesEnabled);
        }

        [Test]
        public void SendEvents()
        {
            app.Tap(TestStrings.GoToAnalyticsPageButton);
            app.Tap(TestStrings.SendEventButton);
            app.Tap(TestStrings.AddPropertyButton);
            app.Tap(TestStrings.AddPropertyButton);
            app.Tap(TestStrings.AddPropertyButton);
            app.Tap(TestStrings.AddPropertyButton);
            app.Tap(TestStrings.AddPropertyButton);
            app.Tap(TestStrings.SendEventButton);
            /* TODO This test is incomplete */
        }

        [Test]
        public void DivideByZero()
        {
            /* Crash the application with a divide by zero exception and then restart*/
            app.Tap(TestStrings.GoToCrashesPageButton);
            app.Tap(TestStrings.DivideByZeroCrashButton);
            app = AppInitializer.StartApp(platform);
            app.Tap(TestStrings.GoToCrashResultsPageButton);

            /* Ensure that the callbacks were properly called */
            CrashResultsHelper.app = app;
            Assert.IsTrue(CrashResultsHelper.SendingErrorReportWasCalled);
            Assert.IsTrue(CrashResultsHelper.SentErrorReportWasCalled);
            Assert.IsFalse(CrashResultsHelper.FailedToSendErrorReportWasCalled);
            Assert.IsTrue(CrashResultsHelper.ShouldProcessErrorReportWasCalled);
            Assert.IsTrue(CrashResultsHelper.ShouldAwaitUserConfirmationWasCalled);
            Assert.IsTrue(CrashResultsHelper.GetErrorAttachmentWasCalled);
            /* TODO verify the last session error report */
        }
    }
}
