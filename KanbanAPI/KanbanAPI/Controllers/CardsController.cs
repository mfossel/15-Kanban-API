using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KanbanAPI;
using AutoMapper;
using KanbanAPI.Models;
using System.Collections;

namespace KanbanAPI.Controllers
{
    public class CardsController : ApiController
    {
        private KanbanEntities db = new KanbanEntities();

        // GET: api/Cards
        public IEnumerable<CardsModel> GetCards()
        {
            return Mapper.Map<IEnumerable<CardsModel>>(db.Cards);
        }

        // GET: api/Cards/5
        [ResponseType(typeof(CardsModel))]
        public IHttpActionResult GetCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CardsModel>(card));
        }

        // PUT: api/Cards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCard(int id, CardsModel card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.CardId)
            {
                return BadRequest();
            }


            var dbCard = db.Cards.Find(id);
            dbCard.Update(card);
            db.Entry(dbCard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cards
        [ResponseType(typeof(CardsModel))]
        public IHttpActionResult PostCard(CardsModel card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbCard = new Card(card);

            db.Cards.Add(dbCard);
            db.SaveChanges();

            card.CreatedDate = dbCard.CreatedDate;
            card.CardId = dbCard.CardId;

            return CreatedAtRoute("DefaultApi", new { id = card.CardId }, card);
        }

           // DELETE: api/Cards/5
        [ResponseType(typeof(Card))]
        public IHttpActionResult DeleteCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(int id)
        {
            return db.Cards.Count(e => e.CardId == id) > 0;
        }
    }
}