SELECT v.Id, v.Title, v.Description, v.Url, v.DateCreated , v.UserProfileId,

                               up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                               up.ImageUrl AS UserProfileImageUrl
                        
                        FROM Video v 
                               JOIN UserProfile up ON v.UserProfileId = up.Id
                        WHERE v.DateCreated >= '2020-01-01 11:11:11'
                        ORDER BY v.DateCreated