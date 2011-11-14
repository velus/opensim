/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using Nini.Config;
using NUnit.Framework;
using OpenMetaverse;
using OpenSim.Framework;
using OpenSim.Region.CoreModules.Avatar.Friends;
using OpenSim.Region.Framework.Scenes;
using OpenSim.Tests.Common;
using OpenSim.Tests.Common.Mock;

namespace OpenSim.Region.CoreModules.Avatar.Friends.Tests
{
    [TestFixture]
    public class FriendsModuleTests
    {
        private FriendsModule m_fm;
        private TestScene m_scene;

        [SetUp]
        public void Init()
        {
            IConfigSource config = new IniConfigSource();
            config.AddConfig("Modules");
            // Not strictly necessary since FriendsModule assumes it is the default (!)
            config.Configs["Modules"].Set("FriendsModule", "FriendsModule");
            config.AddConfig("Friends");
            config.Configs["Friends"].Set("Connector", "OpenSim.Services.FriendsService.dll");
            config.AddConfig("FriendsService");
            config.Configs["FriendsService"].Set("StorageProvider", "OpenSim.Data.Null.dll");

            m_scene = SceneHelpers.SetupScene();
            m_fm = new FriendsModule();
            SceneHelpers.SetupSceneModules(m_scene, config, m_fm);
        }

        [Test]
        public void TestNoFriends()
        {
            TestHelpers.InMethod();
//            log4net.Config.XmlConfigurator.Configure();

            UUID userId = TestHelpers.ParseTail(0x1);

            ScenePresence sp = SceneHelpers.AddScenePresence(m_scene, userId);
            
            Assert.That(((TestClient)sp.ControllingClient).OfflineNotificationsReceived.Count, Is.EqualTo(0));
            Assert.That(((TestClient)sp.ControllingClient).OnlineNotificationsReceived.Count, Is.EqualTo(0));
        }
    }
}