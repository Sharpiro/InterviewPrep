﻿using InterviewPrep.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace InterviewPrep.CoreTests
{
    [TestClass]
    public class HashTableTests
    {
        [TestMethod]
        public void GetHashTest()
        {
            var hasher = new CustomHashTable();
            var key = 1;
            var hash = hasher.GetHash(key);
            Assert.AreEqual(1, hash);
            key = 127;
            hash = hasher.GetHash(key);
            Assert.AreEqual(127, hash);
            key = 1023;
            hash = hasher.GetHash(key);
            Assert.AreEqual(127, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTest()
        {
            var hasher = new CustomHashTable();
            hasher.Add(1, 1);
            hasher.Add(1, 2);
        }

        [TestMethod]
        public void GetTest()
        {
            var hasher = new CustomHashTable();
            const int key = 128;
            const int value = 12;
            hasher.Add(key, value);
            var result = hasher.Get(key);
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RangeTest()
        {
            var hasher = new CustomHashTable();
            for (var i = 0; i < 128; i++)
            {
                hasher.Add(i, i);
            }
            hasher.Add(129, 129);
        }

        [TestMethod]
        public void LengthTest()
        {
            var hasher = new CustomHashTable();
            Assert.AreEqual(0, hasher.Length);
            hasher.Add(1, 1);
            Assert.AreEqual(1, hasher.Length);
        }
    }
}