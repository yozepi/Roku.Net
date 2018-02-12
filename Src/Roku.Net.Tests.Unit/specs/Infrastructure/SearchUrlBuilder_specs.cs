using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku.Infrastructure;
using FluentAssertions;
using yozepi.Roku;

namespace Roku.Net.Tests.Unit.specs.Infrastructure
{
    [TestClass]
    public class SearchUrlBuilder_specs : nSpecTestHarness
    {
        [TestMethod]
        public void SearchUrlBuilderSpecs()
        {
            this.RunMySpecs();
        }

        SearchUrlBuilder subject;


        void BuildSearchUrlFor_method()
        {
            string result = null;
            Uri rokuUri = new Uri("http://198.1.0.1");

            describe["validation handling"] = () =>
            {
                before = () =>
                {
                    subject.Keyword = "anykeyword";
                };

                describe["when the keyword is omitted"] = () =>
                {
                    it["should throw ArgumentNullException"] = () =>
                    {
                        subject.Keyword = null;
                        act.ShouldThrow<ArgumentNullException>();
                    };
                };

                describe["when the season value is wrong"] = () =>
                {
                    it["should throw ArgumentOutOfRangeException"] = () =>
                    {
                        subject.Season = -1;
                        act.ShouldThrow<ArgumentOutOfRangeException>();
                    };
                };
            };

            describe["the url result"] = () =>
            {
                string keyword = "War for the Planent of the Apes";

                before = () => subject.Keyword = keyword;

                it["should contain the escaped keyword in the query string"] = () =>
                {
                    result.Should().Contain($"?keyword={Uri.EscapeDataString(keyword)}");
                };

                describe["when the search type is included"] = () =>
                {
                    SearchType? searchType = null;

                    before = () =>
                    {
                        searchType = SearchType.Person;
                        subject.SearchType = searchType;
                    };

                    it["should include the type in the querystring"] = () =>
                    {
                        result.Should().Contain($"&type={ConversionUtils.SearchTypeToQuerystringValue(searchType.Value)}");
                    };

                    describe["when season is also provided"] = () =>
                    {
                        int season = 2;
                        before = () => subject.Season = season;

                        describe["when the search type is tv show"] = () =>
                        {
                            before = () => subject.SearchType = SearchType.TVShow;
                            it["should include the season in the querystring"] = () =>
                            {
                                result.Should().Contain($"&season={season}");
                            };
                        };

                        describe["when the search type any other value"] = () =>
                        {
                            before = () => subject.SearchType = SearchType.Movie;
                            it["should not include the season in the querystring"] = () =>
                            {
                                result.Should().NotContain($"&season={season}");
                            };
                        };
                    };
                };

                describe["when the appId is included"] = () =>
                {
                    int appId = 12345;

                    before = () => subject.AppId = appId;
                    it["should include the appId in the querystring"] = () =>
                    {
                        result.Should().Contain($"&provider-id={appId}");
                    };

                    describe["when launch app is also requested"] = () =>
                    {
                        before = () => subject.Launch = true;
                        it["should include the launch request in the querystring"] = () =>
                        {
                            result.Should().Contain("&launch=true");
                        };
                    };
                };
            };

            before = () => subject = new SearchUrlBuilder();
            act = () => result = subject.BuildSearchUrlFor(rokuUri);

        }
    }
}
