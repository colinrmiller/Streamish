                --SELECT v.Id AS VideoId, v.Title, v.Description, v.Url, 
                --       v.DateCreated AS VideoDateCreated, v.UserProfileId As VideoUserProfileId,
                --       up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                --       up.ImageUrl AS UserProfileImageUrl,
                --       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId

                --FROM Video v 
                --       JOIN UserProfile up ON v.UserProfileId = up.Id
                --       LEFT JOIN Comment c on c.VideoId = v.id
                --ORDER BY  v.DateCreated

                    --SELECT v.Title, v.Description, v.Url, v.DateCreated, v.UserProfileId,
                    --        up.Name, up.Email, up.ImageUrl, up.DateCreated as UserProfileDateCreated
                    --FROM Video v
                    --    LEFT JOIN UserProfile up ON up.Id = v.UserProfileId
                    --WHERE v.Id = 1

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
