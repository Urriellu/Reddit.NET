﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit.NET;
using Reddit.NET.Models.Structures;
using RestSharp;
using System.Collections.Generic;

namespace Reddit.NETTests.ModelTests
{
    [TestClass]
    public class ModerationTests : BaseTests
    {
        public ModerationTests() : base() { }

        [TestMethod]
        public void GetLog()
        {
            ModActionContainer log = reddit.Models.Moderation.GetLog(null, null, testData["Subreddit"]);

            Assert.IsNotNull(log);
        }

        [TestMethod]
        public void ModQueueReports()
        {
            PostContainer modQueue = reddit.Models.Moderation.ModQueue("reports", null, null, "links", testData["Subreddit"]);

            Validate(modQueue, true);
        }

        [TestMethod]
        public void ModQueueSpam()
        {
            PostContainer modQueue = reddit.Models.Moderation.ModQueue("spam", null, null, "comments", testData["Subreddit"]);

            Validate(modQueue, true);
        }

        [TestMethod]
        public void ModQueue()
        {
            PostContainer modQueue = reddit.Models.Moderation.ModQueue("modqueue", null, null, "links", testData["Subreddit"]);

            Validate(modQueue, true);
        }

        [TestMethod]
        public void ModQueueUnmoderated()
        {
            PostContainer modQueue = reddit.Models.Moderation.ModQueue("unmoderated", null, null, "links", testData["Subreddit"]);

            Validate(modQueue, true);
        }

        [TestMethod]
        public void ModQueueEdited()
        {
            PostContainer modQueue = reddit.Models.Moderation.ModQueue("edited", null, null, "links", testData["Subreddit"]);

            Validate(modQueue, true);
        }

        [TestMethod]
        public void Stylesheet()
        {
            string css = "";
            try
            {
                css = reddit.Models.Moderation.Stylesheet(testData["Subreddit"]);
            }
            catch (System.Net.WebException ex)
            {
                if (!ex.Data.Contains("res")
                    || ((IRestResponse)ex.Data["res"]).StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw ex;
                }
                else
                {
                    Assert.Inconclusive("Subreddit does not contain a stylesheet.  Please create one and retest.");
                }
            }

            Assert.IsNotNull(css);
        }

        // Requires existing subreddit with mod privilages.  --Kris
        // TODO - Move this to workflow tests since it hits non-moderation endpoints.  --Kris
        [TestMethod]
        public void Approve()
        {
            Post post = reddit.Models.Listings.New(null, null, true, testData["Subreddit"]).Data.Children[0].Data;

            reddit.Models.Moderation.Approve(post.Name);

            post = reddit.Models.LinksAndComments.Info(post.Name).Posts[0];

            Assert.IsNotNull(post);
            Assert.IsTrue(post.Approved);
        }
    }
}
