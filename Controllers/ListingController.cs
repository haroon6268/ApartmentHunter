using Apartments.Models;
using ApartmentsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingController : ControllerBase{
        private readonly ListingService _listingService;
        private readonly UserService _userService;
        private readonly ImageService _imageService;

        public ListingController(ListingService service, UserService userService){
            _listingService = service;
            _userService = userService;}
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int page, int skip, double n, double e, double s, double w, bool vp, int? min = 0, int? max = 0, int beds = 0) {
            var data = await _listingService.GetAsync();
            if(page != 0){
                if(skip == 0){
                    skip = 10;
                }
                data = data.Skip(page-1 * skip).Take(10).ToList();
            }
            if(vp){
                data = data.Where(x => (x.lat <= n && x.lat >= s) && (x.lng >= w && x.lng <= e) ).ToList();
            }
            if(min != 0){
                data = data.Where(x => x.Rent > min).ToList();
            }
            if(max != 0){
                data = data.Where(x => x.Rent < max).ToList();
            }
            if(beds!=0){
                data = data.Where(x => x.beds >= beds).ToList();
            }

            return Ok(new {data = data});
        }

    
        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Listing>> Get(string id){
            var listing = await _listingService.GetAsync(id);
            if(listing is null){
                return NotFound();
            }
            return Ok(new{listing});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(Listing newListing){
            var email = Request.Headers["email"];
            newListing.Creator = email;
           
           
            await _listingService.CreateAsync(newListing);
            var user = await _userService.GetAsync(email);
            user.listings.Add(newListing.Id);
            await _userService.UpdateAsync(email, user);
            
            return CreatedAtAction(nameof(Get), new{id = newListing.Id}, newListing);
        
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Listing updatedListing){
            var listing = await _listingService.GetAsync(id);
            if(listing is null){
                return NotFound();
            }
            updatedListing.Id = listing.Id;
            await _listingService.UpdateAsync(id, updatedListing);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete (string id){
            var listing = await _listingService.GetAsync(id);
            if(listing is null){
                return NotFound();
            }
            await _listingService.RemoveAsync(id);
            return NoContent();
        }
    }

}