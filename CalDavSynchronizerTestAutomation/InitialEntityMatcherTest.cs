﻿// This file is Part of CalDavSynchronizer (http://outlookcaldavsynchronizer.sourceforge.net/)
// Copyright (c) 2015 Gerhard Zehetbauer
// Copyright (c) 2015 Alexander Nimmervoll
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Linq;
using CalDavSynchronizer.Implementation;
using CalDavSynchronizerTestAutomation.Infrastructure;
using GenSync.Logging;
using NUnit.Framework;

namespace CalDavSynchronizerTestAutomation
{
  [TestFixture]
  public class InitialEntityMatcherTest
  {
    [Test]
    [Ignore ("Automated tests have to be run via TestAutomationPlugin")]
    public void FindsMatchingEntites ()
    {
      OutlookTestContext.DeleteAllOutlookEvents();

      var id1 = OutlookTestContext.CreateEventInOutlook ("first", DateTime.Now.AddDays (11), DateTime.Now.AddDays (20));
      var id2 = OutlookTestContext.CreateEventInOutlook ("second", DateTime.Now.AddDays (-11), DateTime.Now.AddDays (-9));
      var id3 = OutlookTestContext.CreateEventInOutlook ("third", DateTime.Now.AddDays (9), DateTime.Now.AddDays (11));

      var entityRelationStorage = new InMemoryEntityRelationStorage();
      var calDavServer = new InMemoryCalDAVServer();

      var synchronizer = OutlookTestContext.CreateEventSynchronizer (
          SynchronizationMode.ReplicateOutlookIntoServer,
          calDavServer,
          entityRelationStorage);


      OutlookTestContext.WaitForTask (synchronizer.SynchronizeNoThrow(NullSynchronizationLogger.Instance));

      var entityRelationDatas = entityRelationStorage.LoadEntityRelationData().ToArray();
      Assert.That (entityRelationDatas.Length, Is.EqualTo (3));

      var newEntityRelationStorage = new InMemoryEntityRelationStorage();

      synchronizer = OutlookTestContext.CreateEventSynchronizer (
          SynchronizationMode.MergeInBothDirections,
          calDavServer,
          newEntityRelationStorage);

      OutlookTestContext.WaitForTask (synchronizer.SynchronizeNoThrow (NullSynchronizationLogger.Instance));

      var newRelations = newEntityRelationStorage.LoadEntityRelationData().ToArray();
      // If the InitialEntityMatcher would not work, it would not recognize matching events 
      // and number of events would be doubled, since each repository contains 3 events and mode is MergeInBothDirections
      Assert.That (newRelations.Length, Is.EqualTo (3));

      // the new found relkations must be the same as the existing ones
      foreach (var newRelation in newRelations)
      {
        Assert.That (
            entityRelationDatas.FirstOrDefault (o => o.AtypeId == newRelation.AtypeId
                                                     && o.AtypeVersion == newRelation.AtypeVersion
                                                     && o.BtypeId == newRelation.BtypeId
                                                     && o.BtypeVersion == newRelation.BtypeVersion),
            Is.Not.Null);
      }
    }
  }
}