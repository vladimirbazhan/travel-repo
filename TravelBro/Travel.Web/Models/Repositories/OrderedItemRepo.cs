﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;
using WebGrease.Css.Extensions;

namespace WebApplication1.Models.Repositories
{
    public class OrderedItemRepo<TEntity> : Repository<TEntity> where TEntity : Entity, ITripEntity, IOrderedEntity
    {
        public OrderedItemRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
        }

        public override TEntity Insert(TEntity entity)
        {
            RecalculateTripItemsOrder(entity.TripId, entity.Order, true);
            return base.Insert(entity);
        }

        public override bool Update(TEntity entity)
        {
            RecalculateTripItemsOrder(entity.TripId, entity.Order, true);
            return base.Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            RecalculateTripItemsOrder(entity.TripId, entity.Order, false);
            base.Delete(entity);
        }

        public override void Delete(int id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                RecalculateTripItemsOrder(entity.TripId, entity.Order, false);
                Delete(entity);
            }
        }

        private void RecalculateTripItemsOrder(int tripId, int currentItemOrder, bool increase)
        {
            Trip trip = parent.Repo<TripRepo>().Get(tripId);
            trip.Visits.Where(x => x.Order >= currentItemOrder).ForEach(x => x.Order += increase ? 1 : -1 );
            trip.Routes.Where(x => x.Order >= currentItemOrder).ForEach(x => x.Order += increase ? 1 : -1 );
        }

    }
}