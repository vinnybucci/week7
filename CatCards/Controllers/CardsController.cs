using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatCards.DAO;
using CatCards.Models;
using CatCards.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICatCardDAO cardDAO;
        private readonly ICatFactService catFactService;
        private readonly ICatPicService catPicService;

        public CardsController(ICatCardDAO _cardDAO, ICatFactService _catFact, ICatPicService _catPic)
        {
            catFactService = _catFact;
            catPicService = _catPic;
            cardDAO = _cardDAO;
        }
        [HttpGet]
        public List<CatCard> ListCards()
        {
            return cardDAO.GetAllCards();
            
        }
        [HttpGet("{id}")]
        public ActionResult<CatCard> GetCatCard(int id)
        {
            CatCard card = cardDAO.GetCard(id);

            if (card != null)
            {
                return card;
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("random")]
        public ActionResult<CatCard> GetRandomCat()
        {
            CatCard card = new CatCard();
            card.CatFact = catFactService.GetFact().Text;
            card.ImgUrl = catPicService.GetPic().File;
            return card;

        }
        [HttpPost]
        public ActionResult<CatCard> SaveCard(CatCard card)
        {
            if (card.CatCardId == null)
            {
                return BadRequest();
            }
            CatCard added = cardDAO.SaveCard(card);
            return Created($"/Card/{added.CatCardId}", added);
        }
        [HttpPut("{id}")]
        public  ActionResult<Boolean> UpdateCatCard(int id,CatCard card)
        {
            CatCard location = cardDAO.GetCard(id);
            if (location == null)
            {
                return NotFound($"CatCard id {id} doesn't exist");
            }

            bool result = cardDAO.UpdateCard( card);
            return result;
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            CatCard card = cardDAO.GetCard(id);
            if (card == null)
            {
                return NotFound("Card does not exist");
            }
            cardDAO.RemoveCard(id);
            return NoContent();
        }

    }
}
