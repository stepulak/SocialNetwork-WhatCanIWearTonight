using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using Microsoft.Ajax.Utilities;

namespace WebApiLayer.Controllers
{
    public class UserController : ApiController
    {
        public UserFacade UserFacade { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime BornBefore { get; set; }
        public DateTime BornAfter { get; set; }
        
        [HttpGet, Route("api/users/query")]
        public async Task<IEnumerable<UserDto>> Query(string sort = null, bool asc = true,
            string username = null, string email = null, string gender = null, string bornBefore = null, 
            string bornAfter = null)
        {
            var filter = CreateFilterDto(sort, asc, username, email, gender, bornBefore, bornAfter);
            return (await UserFacade.GetAllUsersAsync(filter)).Items;
        }

        public async Task<UserDto> Get(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var user = await UserFacade.GetUserAsync(guid);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return user;
        }
        
        [HttpPost, Route("api/users/register")]
        public async Task<string> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid) //TODO: add annotations to DTOs
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var userId = await UserFacade.RegisterUser(userDto);
            if (userId.Equals(Guid.Empty))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return $"Created user with id:  {userId}";
        }
        
        [HttpPost, Route("api/users/unregister")]
        public async Task<string> Unregister([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid) //TODO: add annotations to DTOs
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var result = await UserFacade.UnregisterUser(userDto);
            if (!result)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return $"Unregistered user with id:  {userDto.Id}";
        }

        public async Task<string> Put(Guid id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var result = await UserFacade.EditUserAsync(userDto);
            if (!result)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return $"Updated user with id: {id}";
        }


        private UserFilterDto CreateFilterDto(string sort, bool asc,string username, string email, string gender,
            string bornBefore, string bornAfter)
        {
            return new UserFilterDto
            {
                SortCriteria = sort,
                SortAscending = asc,
                Username = username,
                Email = email,
                Gender = ParseStringToGenderEnum(gender),
                BornBefore = ParseStringToDateTime(bornBefore, DateTime.MinValue),
                BornAfter = ParseStringToDateTime(bornAfter, DateTime.MaxValue)
            };
        }

        private Gender ParseStringToGenderEnum(string gender)
        {
            return !Enum.TryParse(gender, true, out Gender genderFilter) ? Gender.NoInformation : genderFilter;
        }

        private DateTime ParseStringToDateTime(string dateTime, DateTime defaultValue)
        {
            return DateTime.TryParse(dateTime, out DateTime result) ? defaultValue : result;
        }
    }
}