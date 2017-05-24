namespace RealWorld.Effects

module Actions = 
  open Suave
  open RealWorld.Models
  open DB
  open MongoDB.Bson

  let jsonToString (json: 'a) = json |> Suave.Json.toJson |> System.Text.Encoding.UTF8.GetString

  let fakeReply email = 
    {User = { Email = email; Token = ""; Username=""; Bio=""; Image=""; PasswordHash=""; }; Id=(BsonObjectId(ObjectId.GenerateNewId()))  }
    
  let registerUserNewUser dbClient = 
    request ( fun inputGraph -> 
      Suave.Json.fromJson<UserRequest> inputGraph.rawForm
      |> registerWithBson dbClient 
      |> Realworld.Convert.userRequestToUser 
      |> jsonToString 
      |> Successful.OK
    )
      
  let getCurrentUser dbClient =
    request (fun inputGraph ->
      Successful.OK (fakeReply "" |> jsonToString)
    )

  let updateUser dbClient = 
    request (fun inputGraph ->
      let userToUpdate = (Suave.Json.fromJson<User> inputGraph.rawForm).User

      userToUpdate
      |> updateRequestedUser dbClient
      |> Realworld.Convert.updateUser userToUpdate
      |> Successful.OK
    )

  open RealWorld.Stubs
  let getUserProfile dbClient username = 
    (Successful.OK Responses.singleProfile)

  let createNewArticle (articleToAdd : Article) dbCLient = 
    
    Responses.singleArticle