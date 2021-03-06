namespace RealWorld
module Convert =
  open RealWorld.Models
  open MongoDB.Bson
  open MongoDB.Driver

  let userRequestToUser (user: UserRequest) = 
    {
      user = {
                username = user.user.username;
                email = user.user.email;
                PasswordHash = "";
                token = "";
                bio = "";
                image = "";
                favorites=[||];
            };
      Id = BsonObjectId.Empty
    }
  
  let updateUser (user:UserDetails) (result : UpdateResult option) : string  =
    match result with
    | Some _ ->  user |> Suave.Json.toJson |> System.Text.Encoding.UTF8.GetString
    | None -> { errors = { body = [|"Error updating this user."|] } } |> Suave.Json.toJson |> System.Text.Encoding.UTF8.GetString

  let defaultProfile =
    { username = ""; bio = ""; image = ""; following = false;}

  let defaultArticle =
    { 
      Id=(BsonObjectId(ObjectId.GenerateNewId()));
      article = 
      { slug = ""; 
        title = ""; 
        description = ""; 
        body = ""; 
        createdAt = ""; 
        updatedAt = ""; 
        favorited = false; 
        favoritesCount = 0u; 
        author = defaultProfile; 
        taglist = [||]  }
    }

  let extractArticleList (result) =
    match result with
    | Some article -> article
    | None -> defaultArticle
    