# TicTacToe
To run the game on a development computer (using Visual Studio Code), follow these steps:

1. Clone the repository to your local machine using the following command:

       git clone https://github.com/Joboa/TicTacToe.git

2. Open the solution file "TicTacToe.sln" in Visual Studio.
3. Build the solution by clicking on "Build" in the top menu and then select "Build Solution" or by pressing Ctrl+Shift+B.
4. Configure your database and update the input of your connection string in ```appsettings.json``` to establish a connection with your database. 
   Refer to the code snippet below for an example in:
    
   ```json
   "ConnectionStrings": {
    "DataContext": <Your Connection String Here>
    }
   ```
4. Set up the database by running the following commands in the Package Manager Console:
    ```
        Add-Migration Initial
        Update-Database
    ```
5. Start the application by clicking on the "Play" button in the toolbar or by pressing F5.
6. The endpoints can be accessed using a tool such as Postman or curl or using the default built in Swagger Tool. Here are the available endpoints:

    <ul>
        <li>Start game: POST /api/games</li>
        <li>Request body:</li>
    </ul>

    ```json
    {
        "player1": {
            "name": "Player 1"
        },
        "player2": {
            "name": "Player 2"
        }
    }
    ```

    <ul>
        <li>Response body:</li>
    </ul>

     ```json
    {
        "id": 1,
        "player1": {
            "id": 1,
            "name": "Player 1"
        },
        "player2": {
            "id": 2,
            "name": "Player 2"
        }
    }

    ```
    <br>

      <ul>
        <li>Register player movement: POST /api/games/{gameId}/players/{playerId}/moves</li>
        <li>Request body:</li>
    </ul>

    ```json
    {
        "boardRow": 0,
        "boardColumn": 0
    }
    ```

    <ul>
        <li>Response body:</li>
    </ul>

     ```json
    {
        "message": "Move registered successfully"
    }
    ```

    <ul>
        <li>Get all running games: GET /api/games</li>
    <ul>
        <li>Response body:</li>
    </ul>

     ```json
    [
        {
            "gameId": 1,
            "player1": {
                "name": "Player 1",
                "moves": 1,
                "lastMove": {
                    "boardRow": 0,
                    "boardColumn": 0,
                    "moveTime": "2023-04-08T09:57:17.6329064"
                }
            },
            "player2": {
                "name": "Player 2",
                "moves": 0,
                "lastMove": null
            }
        }
    ]
    ```

# Appropriate OAuth 2/OIDC grant to use for a web application using a SPA
For a web application using a Single Page Application (SPA), it is important to consider the appropriate OAuth 2.0/OpenID Connect (OIDC) grant to use for secure access token retrieval. The recommended grant for SPAs is the Authorization Code Flow with PKCE (Proof Key for Code Exchange). The Authorization Code Flow with PKCE is preferred because it provides a secure way to obtain an access token without exposing the client's secret. This is particularly important for SPAs because the client's secret cannot be reliably protected in the browser, where it is exposed to the user-agent and can be easily intercepted by attackers.

To address this issue, the PKCE extension was introduced as a mechanism for secure access token retrieval in SPAs. It works by generating a one-time-use code challenge and verifier pair that is exchanged for an authorization code. This ensures that only the client that initiated the authorization flow can exchange the authorization code for an access token. This provides an additional layer of security against interception and code injection attacks. In addition, It ensures the confidentiality of the client's secret. By using this grant, developers can ensure that their SPAs are secure and reliable for end-users.
