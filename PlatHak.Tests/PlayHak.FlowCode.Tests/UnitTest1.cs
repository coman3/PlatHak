﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlatHak.FlowCode;
using PlatHak.FlowCode.Data;
using PlatHak.FlowCode.FlowItems;

namespace PlayHak.FlowCode.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckOutputOfBasicFlowCode()
        {

            var flowCodeBuilder = new FlowCodeBuilder();
            flowCodeBuilder.FlowItems.Add(new UsingItem("System"));
            flowCodeBuilder.FlowItems.Add(new NamespaceItem("PlatHak")
            {
                Items = new List<FlowItem>
                {
                    new ClassItem(AccessLevel.Public, "Program")
                    {
                        Items = new List<FlowItem>
                        {
                            new PropertyItem(AccessLevel.Public, FlowItems.String, "TestString")
                        }
                    }
                }
            });
            Debug.WriteLine(flowCodeBuilder);
        }
    }
}