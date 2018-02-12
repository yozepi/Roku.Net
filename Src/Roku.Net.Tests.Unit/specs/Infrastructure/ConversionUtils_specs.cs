using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku;
using yozepi.Roku.Infrastructure;
using FluentAssertions;

namespace Roku.Net.Tests.Unit.specs.Infrastructure
{
    [TestClass]
    public class ConversionUtils_specs : nSpecTestHarness
    {
        [TestMethod]
        public void ConversionUtilsSpecs()
        {
            this.RunMySpecs();
        }

        void SearchTypeToQuerystringValue_method()
        {
            SearchType searchType = SearchType.Channel;
            string result = null;

            describe[$"when search type is {SearchType.Channel}"] = () =>
            {
                before = () => searchType = SearchType.Channel;
                it[$"should return \"{ConversionUtils.CHANNEL_QUERYVALUE}\""] = () =>
                    result.Should().Be(ConversionUtils.CHANNEL_QUERYVALUE);
            };

            describe[$"when search type is {SearchType.Game}"] = () =>
            {
                before = () => searchType = SearchType.Game;
                it[$"should return \"{ConversionUtils.GAME_QUERYVALUE}\""] = () =>
                    result.Should().Be(ConversionUtils.GAME_QUERYVALUE);
            };

            describe[$"when search type is {SearchType.Movie}"] = () =>
            {
                before = () => searchType = SearchType.Movie;
                it[$"should return \"{ConversionUtils.MOVIE_QUERYVALUE}\""] = () =>
                    result.Should().Be(ConversionUtils.MOVIE_QUERYVALUE);
            };

            describe[$"when search type is {SearchType.Person}"] = () =>
            {
                before = () => searchType = SearchType.Person;
                it[$"should return \"{ConversionUtils.PERSON_QUERYVALUE}\""] = () =>
                    result.Should().Be(ConversionUtils.PERSON_QUERYVALUE);
            };

            describe[$"when search type is {SearchType.TVShow}"] = () =>
            {
                before = () => searchType = SearchType.TVShow;
                it[$"should return \"{ConversionUtils.TVSHOW_QUERYVALUE}\""] = () =>
                    result.Should().Be(ConversionUtils.TVSHOW_QUERYVALUE);
            };

            act = () => result = ConversionUtils.SearchTypeToQuerystringValue(searchType);
        }
    }
}
