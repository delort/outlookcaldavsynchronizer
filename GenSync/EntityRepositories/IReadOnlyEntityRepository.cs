// This file is Part of CalDavSynchronizer (http://outlookcaldavsynchronizer.sourceforge.net/)
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
using System.Collections.Generic;
using System.Threading.Tasks;
using GenSync.Logging;

namespace GenSync.EntityRepositories
{
  /// <summary>
  /// All readoperations that a repository has to support
  /// </summary>
  public interface IReadOnlyEntityRepository<TEntity, TEntityId, TEntityVersion, TContext>
  {
    Task<IReadOnlyList<EntityVersion<TEntityId, TEntityVersion>>> GetVersions (IEnumerable<IdWithAwarenessLevel<TEntityId>> idsOfEntitiesToQuery, TContext context);
    Task<IReadOnlyList<EntityVersion<TEntityId, TEntityVersion>>> GetAllVersions (IEnumerable<TEntityId> idsOfknownEntities, TContext context);
    Task<IReadOnlyList<EntityWithId<TEntityId, TEntity>>> Get (ICollection<TEntityId> ids, ILoadEntityLogger logger, TContext context);
    void Cleanup (IReadOnlyDictionary<TEntityId, TEntity> entities);
  }
}