using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{

    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            List<UserProfile> profiles = new List<UserProfile>();

            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        select up.*,
                            v.Id VideoId,
                            v.Title,
                            v.Description,
                            v.Url, 
                            v.DateCreated VideoDateCreated,
                            c.Id CommentId,
                            c.Message,
                            c.UserProfileID CommentUserProfileId
                        From UserProfile up
                            LEFT JOIN Video v ON v.UserProfileId = up.Id
                            LEFT JOIN Comment c ON c.VideoId = v.Id";


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var existingProfile = profiles.FirstOrDefault(p => p.Id == DbUtils.GetInt(reader, "Id"));
                            if (existingProfile == null)
                            {
                                existingProfile = new UserProfile
                                {
                                    Id = DbUtils.GetInt(reader, "Id"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                            }

                            int videoId = DbUtils.GetInt(reader, "VideoId");
                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                var existingVideo = existingProfile.Videos.FirstOrDefault(v => v.Id == videoId);
                                if (existingVideo == null)
                                {
                                    existingVideo = new Video
                                    {
                                        Id = videoId,
                                        Title = DbUtils.GetString(reader, "Title"),
                                        Description = DbUtils.GetString(reader, "Description"),
                                        DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                        Url = DbUtils.GetString(reader, "Url"),
                                        Comments = new List<Comment>(),
                                    };
                                    ;
                                    existingProfile.Videos.Add(existingVideo);
                                }

                                if (DbUtils.IsNotDbNull(reader, "CommentId"))
                                {
                                    existingVideo.Comments.Add(new Comment
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentId"),
                                        Message = DbUtils.GetString(reader, "Message"),
                                        UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                    });
                                }
                            }
                            profiles.Add(existingProfile);
                        }
                    }
                }
            }
            return profiles;
        }
        //public List<UserProfile> GetAll()
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                SELECT up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
        //                       up.ImageUrl AS UserProfileImageUrl,

        //                       v.Id AS VideoId, v.Title, v.Description, v.Url, 
        //                       v.DateCreated AS VideoDateCreated, v.UserProfileId As VideoUserProfileId,

        //                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId

        //                FROM UserProfile up
        //                       LEFT JOIN Video v ON v.UserProfileId = up.Id
        //                       LEFT JOIN Comment c on c.VideoId = v.id
        //                ORDER BY  v.DateCreated
        //                ";

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {

        //                var userProfiles = new List<UserProfile>();
        //                while (reader.Read())
        //                {
        //                    var userProfileId = DbUtils.GetInt(reader, "UserProfileId");

        //                    var existingUserProfile = userProfiles.FirstOrDefault(profile => profile.Id == userProfileId);
        //                    if (existingUserProfile == null)
        //                    {
        //                        existingUserProfile = new UserProfile()
        //                        {
        //                            Id = userProfileId,
        //                            Name = DbUtils.GetString(reader, "Name"),
        //                            Email = DbUtils.GetString(reader, "Email"),
        //                            DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
        //                            ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
        //                            Videos = new List<Video>(),
        //                            //Id = videoId,
        //                            //Title = DbUtils.GetString(reader, "Title"),
        //                            //Description = DbUtils.GetString(reader, "Description"),
        //                            //DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
        //                            //Url = DbUtils.GetString(reader, "Url"),
        //                            //UserProfileId = DbUtils.GetInt(reader, "UserProfileUserProfileId"),
        //                            //UserProfile = new UserProfile()
        //                            //{
        //                            //    Id = DbUtils.GetInt(reader, "UserProfileUserProfileId"),
        //                            //    Name = DbUtils.GetString(reader, "Name"),
        //                            //    Email = DbUtils.GetString(reader, "Email"),
        //                            //    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
        //                            //    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
        //                            //},
        //                            //Comments = new List<Comment>()
        //                        };

        //                        if (DbUtils.IsNotDbNull(reader, "VideoId"))
        //                        {
        //                            int videoId = DbUtils.GetInt(reader, "VideoId");

        //                            existingUserProfile.Videos.Add(new Video()
        //                            {
        //                                Id = videoId,
        //                                Title = DbUtils.GetString(reader, "Title"),
        //                                Description = DbUtils.GetString(reader, "Description"),
        //                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
        //                                Url = DbUtils.GetString(reader, "Url"),
        //                                Comments = new List<Comment>(),
        //                            });

        //                            if (DbUtils.IsNotDbNull(reader, "CommentId"))
        //                            {
        //                                Video video = existingUserProfile.Videos.FirstOrDefault(v => v.Id == videoId);

        //                                video.Comments.Add(new Comment() {
        //                                    Id = DbUtils.GetInt(reader, "CommentId"),
        //                                    Message = DbUtils.GetString(reader, "Message"),
        //                                    VideoId = videoId,
        //                                    UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
        //                                });

        //                            }
        //                        }

        //                        userProfiles.Add(existingUserProfile);
        //                    }

        //                    if (DbUtils.IsNotDbNull(reader, "VideoId"))
        //                    {
        //                        existingUserProfile.Videos.Add(new Video()
        //                        {
        //                            Id = DbUtils.GetInt(reader, "CommentId"),
        //                            Message = DbUtils.GetString(reader, "Message"),
        //                            UserProfileId = videoId,
        //                            UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
        //                        });
        //                    }
        //                }

        //                return userProfiles;
        //            }
        //        }
        //    }
        //}


        //group code
        public UserProfile GetByIdWIthVideos(int id)
        {

            UserProfile profile = null;
            using (var conn = Connection )
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        select up.*,
                            v.Id VideoId,
                            v.Title,
                            v.Description,
                            v.Url, 
                            v.DateCreated VideoDateCreated,
                            c.Id CommentId,
                            c.Message,
                            c.UserProfileID CommentUserProfileId
                        From UserProfile up
                            LEFT JOIN Video v ON v.UserProfileId = up.Id
                            LEFT JOIN Comment c ON c.VideoId = v.Id
                        WHere up.Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (profile == null)
                            {
                                profile = new UserProfile
                                {
                                    Id = DbUtils.GetInt(reader, "Id"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                            }

                            var videoId = DbUtils.GetInt(reader, "VideoId");
                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                var existingVideo = profile.Videos.FirstOrDefault(v => v.Id == videoId);
                                if (existingVideo == null)
                                {
                                    existingVideo = new Video
                                    {
                                        Id = videoId,
                                        Title = DbUtils.GetString(reader, "Title"),
                                        Description = DbUtils.GetString(reader, "Description"),
                                        DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                        Url = DbUtils.GetString(reader, "Url"),
                                        Comments = new List<Comment>(),
                                    };
;
                                    profile.Videos.Add(existingVideo);
                                }

                                if (DbUtils.IsNotDbNull(reader, "CommentId"))
                                {
                                    existingVideo.Comments.Add(new Comment
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentId"),
                                        Message = DbUtils.GetString(reader, "Message"),
                                        UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return profile;

        }
        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (Name, Email, DateCreated, ImageUrl, UserProfileId)
                        OUTPUT INSERTED.ID
                        VALUES (@Name, @Email, @DateCreated, @ImageUrl, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                           SET Name = @Name,
                               Email = @Email,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}