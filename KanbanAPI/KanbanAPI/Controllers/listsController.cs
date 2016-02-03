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
using KanbanAPI.Models;
using AutoMapper;


namespace KanbanAPI.Controllers
{
    public class listsController : ApiController
    {
        private KanbanEntities db = new KanbanEntities();

        // GET: api/lists
        public IEnumerable<listModel> Getlists()
        {
            return Mapper.Map<IEnumerable<listModel>>(db.lists);
        }

        //GET api/lists/5/cards
        [Route("api/lists/{ListId}/cards")]
        public IEnumerable<CardsModel> GetCardsForList(int listId)
        {
            var cards = db.Cards.Where(l => l.ListId == listId);

            return Mapper.Map<IEnumerable<CardsModel>>(cards);

        }

        // GET: api/lists/5
        [ResponseType(typeof(listModel))]
        public IHttpActionResult Getlist(int id)
        {
            list list = db.lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<listModel>(list));
        }

        // PUT: api/lists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlist(int id, listModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list.ListId)
            {
                return BadRequest();
            }

            
            var dbList = db.lists.Find(id);
            dbList.Update(list);
            db.Entry(dbList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!listExists(id))
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

        // POST: api/lists
        [ResponseType(typeof(listModel))]
        public IHttpActionResult Postlist(listModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


           var dbList = new list(list);


            db.lists.Add(dbList);
            db.SaveChanges();

            list.CreatedDate = dbList.CreatedDate;
            list.ListId = dbList.ListId;

            return CreatedAtRoute("DefaultApi", new { id = dbList.ListId }, list);
        }

        // DELETE: api/lists/5
        [ResponseType(typeof(listModel))]
        public IHttpActionResult Deletelist(int id)
        {
            list list = db.lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            db.lists.Remove(list);
            db.SaveChanges();

            return Ok(Mapper.Map<listModel>(list));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool listExists(int id)
        {
            return db.lists.Count(e => e.ListId == id) > 0;
        }
    }
}