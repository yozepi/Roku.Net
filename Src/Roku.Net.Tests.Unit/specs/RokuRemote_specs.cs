using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku;
using FluentAssertions;
using yozepi.Roku.Infrastructure;
using Moq;
using System.Net;

namespace Roku.Net.Tests.Unit.specs
{
    [TestClass]
    public class RokuRemote_specs : nSpecTestHarness
    {
        [TestMethod]
        public void RokuRemoteSpecs()
        {
            this.RunMySpecs();
        }

        RokuRemote subject;
        Mock<IRokuRequestFactory> requestFactory;
        Mock<IRokuRequest> rokuRequest;

        Uri rokuUri = new Uri("http://198.1.0.1");

        void GetActiveAppAsync_method()
        {
            ActiveAppInfo result = null;
            describe["when the command is successful"] = () =>
            {
                it["should indicate a success"] = () =>
                {
                    result.IsSuccess.Should().BeTrue();
                };

                it["should return the currently active app"] = () =>
                {
                    result.App.Should().NotBeNull();
                };

                before = () =>
                {
                    var rokuResponse = TestHelper.BuildRokuResponse("ActiveApp.xml");
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.ActiveAppUrlFor(rokuUri), "GET"))
                        .ReturnsAsync(() => rokuResponse);
                };

                context["when the Roku is on the home screen"] = () =>
                {
                    it["the app id should be 0 and the app name should be \"Roku\""] = () =>
                    {
                        result.App.Id.Should().Be(0);
                        result.App.Text.Should().Be("Roku");
                    };

                    before = () =>
                    {
                        var rokuResponse = TestHelper.BuildRokuResponse("OnHomeScreen.xml");
                        rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.ActiveAppUrlFor(rokuUri), "GET"))
                            .ReturnsAsync(() => rokuResponse);
                    };
                };

                context["when the Roku is in screensaver mode"] = () =>
                {
                    it["should return screensaver information along with the app information"] = () =>
                    {
                        result.ScreenSaver.Should().NotBeNull();
                        result.App.Should().NotBeNull();
                    };

                    before = () =>
                    {
                        var rokuResponse = TestHelper.BuildRokuResponse("OnScreenSaver.xml");
                        rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.ActiveAppUrlFor(rokuUri), "GET"))
                            .ReturnsAsync(() => rokuResponse);
                    };
                };
            };

            describe["when the command fails"] = () =>
            {
                it["should indicate a failure"] = () =>
                {
                    result.IsSuccess.Should().BeFalse();
                    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                };

                before = () =>
                {
                    var rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.BadRequest);
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.ActiveAppUrlFor(rokuUri), "GET"))
                        .ReturnsAsync(() => rokuResponse);
                };

            };
            before = () =>
            {
                InitializeMocks();
            };

            actAsync = async () =>
            {
                InitializeSubject();
                result = await subject.GetActiveAppAsync();
            };
        }

        void GetAppIconAsync_method()
        {
            AppIcon result = null;
            int appId = 12345;

            describe["when the command succeeds"] = () =>
            {
                string contentType = "image/jpeg";

                it["should indicate success"] = () =>
                {
                    result.IsSuccess.Should().BeTrue();
                };

                it["should return the app Id"] = () =>
                {
                    result.AppId.Should().Be(appId);
                };

                it["should return the content type"] = () =>
                {
                    result.ContentType.Should().Be(contentType);
                };

                it["should return the image bytes"] = () =>
                {
                    result.Image.Should().NotBeEmpty();
                };

                before = () =>
                {
                    var rokuResponse = TestHelper.BuildRokuResponse("Netflix.jpg", contentType: contentType);
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.AppIconUrlFor(rokuUri, appId), "GET"))
                        .ReturnsAsync(() => rokuResponse);
                };
            };

            describe["when the command fails"] = () =>
            {
                it["should indicate a failure"] = () =>
                {
                    result.IsSuccess.Should().BeFalse();
                    result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
                };

                before = () =>
                {
                    var rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.Forbidden);
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.AppIconUrlFor(rokuUri, appId), "GET"))
                        .ReturnsAsync(() => rokuResponse);
                };
            };

            before = () =>
            {
                InitializeMocks();
            };

            actAsync = async () =>
            {
                InitializeSubject();
                result = await subject.GetAppIconAsync(appId);
            };

        }

        void KeyDownAsync_method()
        {
            ICommandResponse result = null;
            Func<Task> commandAction = null;
            IRokuResponse rokuResponse = null;

            describe["when the request succeeds"] = () =>
            {
                string expectedLastKeyPressed = null;

                it["should indicate a success"] = () =>
                {
                    result.IsSuccess.Should().BeTrue();
                };

                it["should keep the last key sent"] = () =>
                {
                    subject._lastKeyPressed.Should().NotBeNull();
                };

                before = () => rokuResponse = TestHelper.BuildRokuResponse();

                describe["when a command key is sent"] = () =>
                {
                    CommandKeys commandKey = CommandKeys.Home;

                    it["should convert the key sent to it's querystring equivelant"] = () =>
                    {
                        subject._lastKeyPressed.Should().Be(expectedLastKeyPressed);
                    };

                    before = () =>
                    {
                        expectedLastKeyPressed = commandKey.ToRouteValue();
                        commandAction = async () => result = await subject.KeyDownAsync(commandKey);
                    };
                };

                describe["when a character key is sent"] = () =>
                {
                    char charKey = ' ';

                    it["should convert the key sent to it's querystring equivelant"] = () =>
                    {
                        subject._lastKeyPressed.Should().Be(charKey.ToRouteValue());
                    };

                    before = () =>
                        commandAction = async () => result = await subject.KeyDownAsync(charKey);
                };

            };

            describe["when the request fails"] = () =>
            {
                it["should indicate a failure"] = () =>
                {
                    result.IsSuccess.Should().BeFalse();
                };

                before = () => rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.Forbidden);

            };

            before = () =>
            {
                commandAction = async () => result = await subject.KeyDownAsync('A');

                InitializeMocks();
                rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.KeyDownUrlFor(rokuUri, It.IsAny<string>()), "POST"))
                    .ReturnsAsync(() => rokuResponse);
            };

            actAsync = async () =>
            {
                InitializeSubject();
                await commandAction();
            };


        }

        void KeyUpAsync_method()
        {
            context["when a key-down command was not previously sent"] = () =>
            {
                it["should throw an InvalidOperationException"] = () =>
                {
                    var call = new Func<Task>(() => subject.KeyUpAsync());
                    call.ShouldThrow<InvalidOperationException>();
                };

                before = () => subject._lastKeyPressed = null;
            };

            context["when a key-down command has been previously set"] = () =>
            {
                ICommandResponse result = null;
                IRokuResponse rokuResponse = null;

                describe["when the request succeeds"] = () =>
                {
                    it["should indicate a success"] = () =>
                    {
                        result.IsSuccess.Should().BeTrue();
                    };
                    before = () => rokuResponse = TestHelper.BuildRokuResponse();
                };

                describe["when the request fails"] = () =>
                {
                    it["should indicate a failure"] = () =>
                    {
                        result.IsSuccess.Should().BeFalse();
                    };
                    before = () => rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.UnsupportedMediaType);
                };

                before = () =>
                {
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.KeyUpUrlFor(rokuUri, CommandKeys.Backspace.ToRouteValue()), "POST"))
                        .ReturnsAsync(() => rokuResponse);
                };

                actAsync = async () =>
                {
                    subject._lastKeyPressed = CommandKeys.Backspace.ToRouteValue();
                    result = await subject.KeyUpAsync();
                };
            };


            before = () =>
            {
                InitializeMocks();
            };

            act = () =>
            {
                InitializeSubject();
                //result = await subject.KeyDownAsync(key);
            };
        }

        void KeypressAsync_method()
        {
            ICommandResponse result = null;
            string resultUrl = null;
            Func<Task> subjectAction = null;
            IRokuResponse rokuResponse = null;

            describe["when the request succeeds"] = () =>
            {

                it["should indicate a success"] = () =>
                {
                    result.IsSuccess.Should().BeTrue();
                };

                before = () =>
                {
                    rokuResponse = TestHelper.BuildRokuResponse();
                    subjectAction = async () => result = await subject.KeypressAsync('z');
                };

                describe["when a command key is sent"] = () =>
                {
                    CommandKeys commandKey = CommandKeys.Home;

                    it["should convert the key sent to it's querystring equivelant"] = () =>
                    {
                        resultUrl.Should().Contain(commandKey.ToRouteValue());
                    };

                    before = () =>
                        subjectAction = async () => result = await subject.KeypressAsync(commandKey);

                };

                describe["when a character key is sent"] = () =>
                {
                    char charKey = '"';

                    it["should convert the key sent to it's querystring equivelant"] = () =>
                    {
                        resultUrl.Should().Contain(charKey.ToRouteValue());
                    };

                    before = () =>
                        subjectAction = async () => result = await subject.KeypressAsync(charKey);

                };
            };

            describe["when the request fails"] = () =>
            {

                it["should indicatate a failure"] = () =>
                {
                    result.IsSuccess.Should().BeFalse();
                };

                before = () =>
                {
                    rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.ServiceUnavailable);
                    subjectAction = async () => result = await subject.KeypressAsync('z');
                };
            };

            before = () =>
            {
                InitializeMocks();
                rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.KeyPressUrlFor(rokuUri, It.IsAny<string>()), "POST"))
                  .ReturnsAsync((string url, string keyString) =>
                  {
                      resultUrl = url;
                      return rokuResponse;
                  });
            };

            actAsync = async () =>
            {
                InitializeSubject();
                await subjectAction();
            };
        }

        void LaunchAppAsync_method()
        {
            ICommandResponse result = null;
            int appId = 9999;

            describe["a successful launch command"] = () =>
            {
                it["should indicate a success"] = () =>
                {
                    result.IsSuccess.Should().BeTrue();
                };

                before = () =>
                {
                    var rokuResponse = TestHelper.BuildRokuResponse();
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.LaunchAppUrlFor(rokuUri, appId), "POST"))
                        .ReturnsAsync(() => rokuResponse);
                };
            };

            describe["an uncuccessful launch command"] = () =>
            {
                it["should indicate a failure"] = () =>
                {
                    result.IsSuccess.Should().BeFalse();
                };

                before = () =>
                {
                    var rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.InternalServerError);
                    rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.LaunchAppUrlFor(rokuUri, appId), "POST"))
                        .ReturnsAsync(() => rokuResponse);
                };
            };

            before = () =>
            {
                InitializeMocks();
            };
            actAsync = async () =>
        {
            InitializeSubject();
            result = await subject.LaunchAppAsync(appId);
        };
        }

        void SearchAsync_method()
        {
            ICommandResponse result = null;
            string keyword = "Attack of the Amazons";
            RokuInfo info = null;
            IRokuResponse rokuResponse = null;

            context["when the device doesn't support search"] = () =>
            {
                it["should throw InvalidOperationException"] = () =>
                {
                    info.IsSearchEnabled = false;
                    var call = new Func<Task>(() => subject.SearchAsync(keyword));
                    call.ShouldThrow<InvalidOperationException>();
                };
                before = () => rokuResponse = TestHelper.BuildRokuResponse();
            };

            describe["when the request succeeds"] = () =>
            {

                it["should indicate a success"] = () =>
                {
                    result.IsSuccess.Should().BeTrue();
                };

                before = () => rokuResponse = TestHelper.BuildRokuResponse();
            };

            describe["when the request fails"] = () =>
            {

                it["should indicate a failure"] = () =>
                {
                    result.IsSuccess.Should().BeFalse();
                };

                before = () => rokuResponse = TestHelper.BuildRokuResponse(statusCode: HttpStatusCode.NotFound);
            };

            before = () =>
            {
                info = new RokuInfo { IsSearchEnabled = true };
                InitializeMocks();
                rokuRequest.Setup(m => m.GetResponseAsync(It.IsAny<string>(), "POST"))
                    .ReturnsAsync(() => rokuResponse);
            };

            actAsync = async () =>
            {
                InitializeSubject();
                subject.Info = info;
                result = await subject.SearchAsync(keyword);
            };
        }


        #region support methods:

        void InitializeMocks()
        {
            rokuRequest = new Mock<IRokuRequest>();

            requestFactory = new Mock<IRokuRequestFactory>();
            requestFactory.Setup(m => m.Create())
                .Returns(() => rokuRequest.Object);
        }

        void InitializeSubject()
        {
            subject = new RokuRemote(requestFactory.Object, rokuUri, null, null);
        }

        #endregion //support methods:
    }
}
