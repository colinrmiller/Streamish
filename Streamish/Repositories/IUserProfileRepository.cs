using Streamish.Models;
using System.Collections.Generic;

namespace Streamish.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile video);
        void Delete(int id);
        List<UserProfile> GetAll();
        //UserProfile GetById(int id);
        void Update(UserProfile video);
        UserProfile GetByIdWIthVideos(int id);
    }
}