using f14.IO;
using f14.xunit;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterSearchAPI.Models;
using TwitterSearchAPI.Parsers;
using Xunit;
using Xunit.Abstractions;

namespace TwitterSearchAPI.Test
{
    public class ParsersTest : XUnitTestBase
    {
        public ParsersTest(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public void TweetParser_Parse()
        {
            string testHtml = FileIO.ReadToEnd("samples/tweets_html.txt");

            var parser = new TweetParser<Tweet>();
            var tweets = parser.Parse(testHtml);

            Assert.NotNull(tweets);
            Assert.NotEmpty(tweets);
        }

        [Fact]
        public void UserProfileParser_Parse()
        {
            string testHtml = FileIO.ReadToEnd("samples/profiles_html.txt");

            var parser = new UserParser<UserProfile>();
            var profiles = parser.Parse(testHtml);

            Assert.NotNull(profiles);
            Assert.NotEmpty(profiles);
        }
    }
}
